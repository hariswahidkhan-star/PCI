# Load harness — Simulation Lab student write path

A k6 profile that drives 10,000 completed lab tasks through the real HTTP API, against either
provider, and fails loudly rather than reporting a comfortable average.

The Simulation Lab is where the product does its heaviest authenticated writing: every student
keystroke session produces autosaves, and every submit runs grading. It is the surface most likely to
fall over first under a cohort, so it is the surface with a load profile.

```
backend/tests/load/
  seed_load_cohort.py   # creates the student cohort and hands k6 their session tokens
  simlab_tasks.js       # the k6 profile
```

## What one iteration does

A lab task is not one request. Each iteration is a whole task:

    POST /api/me/lab/attempts                      start (or resume)
    POST /api/me/lab/attempts/{id}/autosave   x4   working answers, growing each time
    POST /api/me/lab/attempts/{id}/submit          grade
    GET  /api/me/lab/attempts/{id}                 read back — grading must be durable, not merely accepted

Seven requests per task, five of them writes. A profile that only hammered a read endpoint would
report a latency the product never actually pays.

## Sizing the cohort — the arithmetic that matters

`Endpoints/SimLab.cs` throttles attempt writes to **30 per action per user per 10 minutes**
(`RL_LIMIT` / `RL_WINDOW_MS`). The limiter is keyed on `userId|action`, so each action has its own
budget of 30.

Autosave is therefore the binding constraint: one task spends one `start` and one `submit` but
**four** `autosave` writes. The cohort must be sized from the autosave count, not the task count:

    students  >=  tasks x autosaves / 30

For 10,000 tasks at 4 autosaves that is 40,000 autosave writes, needing **1,334 students** minimum;
the seeder applies a 1.25 safety factor (the executor does not deal iterations perfectly evenly
between VUs) and creates **1,667**.

This is computed for you — pass the target, not the cohort:

```bash
python3 seed_load_cohort.py --tasks 10000 --autosaves 4 --out cohort.json
```

Two independent mechanisms keep a mis-sized run from producing meaningless numbers:

- `setup()` in the profile recomputes the requirement and **aborts before the first request** if the
  cohort file is too small, printing the exact re-seed command.
- `guard()` counts any 429 into `sim_throttled_429` and calls `fail()`. The threshold is
  `count==0`. A throttled run has measured the limiter, not the lab, and must not be averaged away
  into a passing report.

The profile also rotates the cohort **per iteration**, not per VU:

```js
const me = cohort[(__ITER * VUS + (__VU - 1)) % cohort.length]
```

`(__VU, __ITER)` is unique across the run, so no student is used more than `ceil(tasks/cohort)` times
however the `shared-iterations` executor distributes work. Pinning a student to a VU — the obvious
first implementation — is wrong at scale for a reason worth stating: at 10,000 tasks over 300 VUs each
VU runs ~33 iterations, so a pinned student issues ~133 autosaves and is throttled four times over
**no matter how large the cohort is**. Growing the cohort cannot fix it; only rotating can.

## Running it

Boot a backend dedicated to the run — this seeds real rows and should not share a database with
anything you care about.

**SQLite**

```bash
cd backend
PORT=8150 DATABASE_FILE=./load.db STORAGE_ROOT=./load_storage \
  CREDENTIAL_ENCRYPTION_KEY=load-harness-key-v1 \
  dotnet bin/Release/net8.0/PCI.Backend.dll &

DATABASE_FILE=./load.db python3 tests/load/seed_load_cohort.py --tasks 10000 --out tests/load/cohort.json

cd tests/load
k6 run -e BASE=http://127.0.0.1:8150 -e COHORT=cohort.json -e TARGET_TASKS=10000 -e VUS=300 simlab_tasks.js
```

**MySQL / MariaDB**

```bash
cd backend
PORT=8151 DB_PROVIDER=mysql MYSQL_HOST=127.0.0.1 MYSQL_USER=pci MYSQL_PASSWORD=... MYSQL_DATABASE=pci_load \
  STORAGE_ROOT=./load_storage_mysql CREDENTIAL_ENCRYPTION_KEY=load-harness-key-v1 \
  dotnet bin/Release/net8.0/PCI.Backend.dll &

DB_PROVIDER=mysql MYSQL_HOST=127.0.0.1 MYSQL_USER=pci MYSQL_PASSWORD=... MYSQL_DATABASE=pci_load \
  python3 tests/load/seed_load_cohort.py --tasks 10000 --out tests/load/cohort_mysql.json

cd tests/load
k6 run -e BASE=http://127.0.0.1:8151 -e COHORT=cohort_mysql.json -e TARGET_TASKS=10000 -e VUS=300 simlab_tasks.js
```

Set `PORT`, not `ASPNETCORE_URLS` — the app reads `PORT` and will otherwise bind 8080 and leave you
staring at connection-refused from k6.

Set `CREDENTIAL_ENCRYPTION_KEY` explicitly. Without it `Security.ResolveCredKey()` derives the
at-rest key from whichever of `STRIPE_WEBHOOK_SECRET | DATABASE_FILE | MYSQL_DATABASE` is set, so the
key silently changes when you switch provider and previously-written objects stop being readable.

The seeder writes rows directly rather than going through `/api/register`, which is rate-limited to
10/min/IP — registering 1,667 students through it would take the best part of three hours and would
be measuring the registration limiter. Sessions are stored as `sha256(token)`, so the seeder generates
a token, stores the hash, and hands k6 the plaintext. It writes exactly the rows a real login writes,
using database access the operator already has.

Cohort accounts are all `loadlab-*@load.pci.local`, so they are identifiable and removable:

```sql
DELETE FROM users WHERE email LIKE 'loadlab-%@load.pci.local';
```

## Measured baseline

10,000 tasks / 70,000 requests / 300 VUs, both providers, same container, same binary, same
`Information`-level request logging in both processes:

| | SQLite | MariaDB 10.11 |
|---|---|---|
| wall clock | 5m08s | 6m16s |
| throughput | 227 req/s | 186 req/s |
| checks | 100.00% (70000/70000) | 100.00% (70000/70000) |
| `http_req_failed` | 0.00% | 0.00% |
| `sim_throttled_429` | 0 | 0 |
| `sim_start_ms` p95 | 1.99s | 2.22s |
| `sim_autosave_ms` p95 | 1.72s | 1.99s |
| `sim_submit_ms` p95 | 1.81s | 2.38s |
| k6 exit code | 0 | 0 |

Read these as a **relative** baseline, not a service objective. Two things inflate the absolute
numbers: the container is small and fully saturated at 300 VUs (all three actions land within ~20% of
each other, which is the signature of a host-bound run rather than a slow endpoint), and both
processes were logging every request at `Information` level — roughly 50 MB of log per run — which
production does not do.

The useful finding in that table is the **ratio**: on identical hardware, binary and workload,
MariaDB sustains about 80% of SQLite's throughput (a repeat pair of runs put it at 75%). That is the
expected shape for a single-node box where SQLite pays no network or connection cost. Watch it for
drift; do not quote it as capacity.

The latency thresholds in `simlab_tasks.js` are set at roughly 2x the MariaDB column so that a
structural regression — an N+1 query, a dropped index, a new lock on the write path — fails the run
while ordinary runner-to-runner variance does not.

## Not wired into CI

This is an operator tool, run deliberately. A 10,000-task run takes 5–6 minutes of saturated CPU
*after* several minutes of seeding, on a runner shared with every other job, and its latency numbers
would be dominated by whatever else that runner happened to be doing. Gating merges on a number that
noisy trains people to re-run red builds until they go green, which is worse than not measuring at
all.

Run it against a release candidate on hardware that resembles production, and compare to the table
above. If you do wire it up, give it a dedicated runner and a schedule, not a pull-request trigger.
