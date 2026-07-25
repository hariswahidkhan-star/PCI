// Simulation Lab load profile — the student write path under sustained concurrency.
//
// WHAT IT DRIVES, AND WHY THIS SHAPE
// A lab task is not one request. A student opens the catalogue, starts (or resumes) an attempt,
// autosaves working answers several times as they think, then submits for grading. Autosave is by far
// the most frequent write and submit is by far the most expensive one, so a profile that only hammered
// a read endpoint would report a latency the product never actually pays. Each iteration here is one
// completed task: start -> N autosaves -> submit -> read back.
//
// WHY A FRESH STUDENT EVERY ITERATION
// Endpoints/SimLab.cs throttles attempt writes to 30 per action per user per 10 minutes. Driving the
// target through a handful of accounts would measure that limiter and score its 429s as failures.
// The cohort is therefore rotated per ITERATION, not per VU: at 10k tasks over 300 VUs each VU runs
// ~33 iterations, so a student pinned to a VU would issue ~133 autosaves and be throttled four times
// over no matter how large the cohort was. Sizing follows from the same arithmetic and is checked
// before the first request rather than discovered mid-run — see requiredCohort() and README.md.
//
//   k6 run -e BASE=http://127.0.0.1:8080 -e COHORT=cohort.json simlab_tasks.js
//   k6 run -e TARGET_TASKS=10000 -e VUS=300 ...
import http from 'k6/http'
import { check, fail } from 'k6'
import { Counter, Trend } from 'k6/metrics'
import { SharedArray } from 'k6/data'

const BASE = __ENV.BASE || 'http://127.0.0.1:8080'
const COHORT_FILE = __ENV.COHORT || 'cohort.json'
const VUS = parseInt(__ENV.VUS || '50', 10)
const TARGET_TASKS = parseInt(__ENV.TARGET_TASKS || '1000', 10)
const AUTOSAVES_PER_TASK = parseInt(__ENV.AUTOSAVES || '4', 10)

// Mirrors Endpoints/SimLab.cs (RL_LIMIT / RL_WINDOW_MS). Kept here so the sizing check below is
// arithmetic the reader can follow, not a magic number.
const RL_LIMIT = 30
const RL_WINDOW_MINUTES = 10

/** Smallest cohort that can absorb the target without any student hitting the limiter.
 *  Autosave is the binding action: one task costs AUTOSAVES_PER_TASK autosave writes against the
 *  same 30-per-window budget that start and submit each spend only once. Assumes the whole run lands
 *  inside one window, which is the conservative reading — a longer run only gets more headroom. */
function requiredCohort(tasks, autosaves) {
  return Math.ceil((tasks * Math.max(1, autosaves)) / RL_LIMIT)
}

// Parsed once per process rather than per VU — a few hundred tokens copied into every VU would
// dominate memory at high concurrency.
const cohort = new SharedArray('cohort', () => {
  const c = JSON.parse(open(COHORT_FILE))
  if (!c.tokens || !c.tokens.length) throw new Error('cohort file has no tokens — run seed_load_cohort.py')
  return c.tokens.map((t) => ({ token: t, scenario: c.scenario_code }))
})

// One counter per stage of the task, so a regression can be attributed rather than merely noticed.
const tasksCompleted = new Counter('sim_tasks_completed')
const throttled = new Counter('sim_throttled_429')
const startTrend = new Trend('sim_start_ms', true)
const autosaveTrend = new Trend('sim_autosave_ms', true)
const submitTrend = new Trend('sim_submit_ms', true)

export const options = {
  scenarios: {
    tasks: {
      executor: 'shared-iterations',
      vus: VUS,
      iterations: TARGET_TASKS,
      maxDuration: __ENV.MAX_DURATION || '30m',
    },
  },
  // Thresholds make this a gate, not a graph: strict on correctness, generous on absolute latency.
  // A run that returns errors, or that the app throttles, has not measured what it claims to measure.
  //
  // The latency gates are set at roughly 2x the measured baseline (see README.md) so they catch a
  // structural regression — an N+1 query, a lost index, a lock introduced on the write path — without
  // failing on the order-of-magnitude spread between CI runners. They are NOT a service objective.
  // The three actions land within ~20% of each other under saturation because the bottleneck is the
  // host, not any one endpoint, so their gates are proportionate rather than individually tuned.
  thresholds: {
    checks: ['rate>0.99'],
    http_req_failed: ['rate<0.01'],
    sim_throttled_429: ['count==0'],
    'sim_start_ms': ['p(95)<5000'],
    'sim_autosave_ms': ['p(95)<4000'],
    'sim_submit_ms': ['p(95)<5000'],
  },
}

// Refuse a mis-sized cohort before the first request instead of after ten minutes of numbers that
// only measure the limiter. setup() runs once and aborts the run when it throws.
export function setup() {
  const need = requiredCohort(TARGET_TASKS, AUTOSAVES_PER_TASK)
  if (cohort.length < need) {
    throw new Error(
      `cohort of ${cohort.length} is too small for ${TARGET_TASKS} tasks x ${AUTOSAVES_PER_TASK} ` +
      `autosaves: that is ${TARGET_TASKS * AUTOSAVES_PER_TASK} autosave writes against a limit of ` +
      `${RL_LIMIT} per student per ${RL_WINDOW_MINUTES} minutes, so it needs at least ${need} ` +
      `students. Re-seed with: seed_load_cohort.py --tasks ${TARGET_TASKS} --autosaves ${AUTOSAVES_PER_TASK}`)
  }
  return { cohortSize: cohort.length, required: need }
}

function auth(token) {
  return { headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' }, timeout: '60s' }
}

/** Count a 429 explicitly. It is not an error the app got wrong — it is the run outgrowing its
 *  cohort, which invalidates the numbers, so it must fail loudly rather than average away. */
function guard(res, label) {
  if (res.status === 429) {
    throttled.add(1)
    fail(`${label} was rate-limited (429) — the cohort is too small for this target; see README.md`)
  }
  return res
}

export default function () {
  // (__VU, __ITER) is unique across the whole run, so this ordinal is a distinct number per iteration
  // and the modulo deals each student out in turn: no student is used more than ceil(tasks/cohort)
  // times however the shared-iterations executor happens to distribute work between VUs.
  const me = cohort[(__ITER * VUS + (__VU - 1)) % cohort.length]
  const opts = auth(me.token)

  const started = guard(
    http.post(`${BASE}/api/me/lab/attempts`,
      JSON.stringify({ scenario_code: me.scenario, mode: 'training' }), opts),
    'start')
  startTrend.add(started.timings.duration)
  if (!check(started, { 'start 200': (r) => r.status === 200 })) return

  const attemptId = started.json('attempt_id')
  if (!attemptId) return

  // Autosaves carry a growing answer set, as a real student's would: a fixed one-key body would
  // under-state the JSON the column actually stores and the row it actually rewrites.
  const answers = {}
  for (let i = 0; i < AUTOSAVES_PER_TASK; i++) {
    answers[`field_${i}`] = `${__VU}-${__ITER}-${i}`
    const saved = guard(
      http.post(`${BASE}/api/me/lab/attempts/${attemptId}/autosave`,
        JSON.stringify({ answers, period: 0 }), opts),
      'autosave')
    autosaveTrend.add(saved.timings.duration)
    check(saved, { 'autosave 200': (r) => r.status === 200 })
  }

  const submitted = guard(
    http.post(`${BASE}/api/me/lab/attempts/${attemptId}/submit`,
      JSON.stringify({ answers }), opts),
    'submit')
  submitTrend.add(submitted.timings.duration)
  // A submit may legitimately be refused as already-submitted when a VU resumes an attempt a previous
  // iteration finished; that is the app being correct, not a failure.
  check(submitted, { 'submit 200 or already-submitted': (r) => r.status === 200 || r.status === 409 })

  // Read the attempt back: grading must be durable and visible, not merely accepted.
  const readBack = http.get(`${BASE}/api/me/lab/attempts/${attemptId}`, opts)
  check(readBack, { 'read-back 200': (r) => r.status === 200 })

  tasksCompleted.add(1)
}
