import type { RepeaterField } from '../components/RepeaterSection'
import type { Experience } from '../api/types'

export const EXPERIENCE_FIELDS: RepeaterField[] = [
  { key: 'company', label: 'Company / organisation', required: true },
  { key: 'title', label: 'Job title', required: true },
  { key: 'start_date', label: 'Start', type: 'month' },
  { key: 'end_date', label: 'End (leave empty if current)', type: 'month' },
  { key: 'is_current', label: 'I currently work here', type: 'checkbox' },
  { key: 'country', label: 'Country' },
  { key: 'industry', label: 'Industry / sector', placeholder: 'e.g. EPC, Energy, Rail' },
  { key: 'summary', label: 'What you did (projects, scope, tools)', type: 'textarea', span2: true },
]

export const QUALIFICATION_FIELDS: RepeaterField[] = [
  { key: 'institution', label: 'Institution', required: true },
  { key: 'degree', label: 'Degree / award', required: true, placeholder: 'e.g. BSc Civil Engineering' },
  { key: 'field', label: 'Field of study' },
  { key: 'year_completed', label: 'Year completed', type: 'number' },
  { key: 'country', label: 'Country' },
]

export const HELD_CERT_FIELDS: RepeaterField[] = [
  { key: 'name', label: 'Certification', required: true, placeholder: 'e.g. PMP, CCP, PSP' },
  { key: 'issuer', label: 'Issuing body', placeholder: 'e.g. PMI, AACE' },
  { key: 'credential_ref', label: 'Credential ID (optional)' },
  { key: 'issued_year', label: 'Year obtained', type: 'number' },
  { key: 'expires_year', label: 'Expiry year (if any)', type: 'number' },
]

/** Months of experience covered by the entries, counting overlapping months only once —
 * the live eligibility tally PMI's wizard makes you compute in your head. */
export function monthsCovered(rows: Pick<Experience, 'start_date' | 'end_date' | 'is_current'>[]): number {
  const now = new Date()
  const nowKey = now.getFullYear() * 12 + now.getMonth()
  const parse = (v?: string | null) => {
    const m = /^(\d{4})-(\d{2})/.exec(v ?? '')
    return m ? Number(m[1]) * 12 + (Number(m[2]) - 1) : null
  }
  const ivs: [number, number][] = []
  for (const r of rows) {
    const s = parse(r.start_date)
    if (s === null) continue
    const e = r.is_current ? nowKey : parse(r.end_date) ?? nowKey
    if (e >= s) ivs.push([s, Math.min(e, nowKey)])
  }
  ivs.sort((a, b) => a[0] - b[0])
  let total = 0
  let curS = 1
  let curE = 0
  for (const [s, e] of ivs) {
    if (s > curE + 1) {
      if (curE >= curS) total += curE - curS + 1
      curS = s
      curE = e
    } else curE = Math.max(curE, e)
  }
  if (curE >= curS) total += curE - curS + 1
  return total
}
