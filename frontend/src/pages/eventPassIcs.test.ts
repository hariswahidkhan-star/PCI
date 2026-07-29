import { describe, it, expect } from 'vitest'
import { buildPassIcs, icsEscape, toIcsLocal } from './eventPassIcs'
import type { EventPass } from '../api/types'

// ICS generation is fully client-side (fixtures mode must produce a working calendar file today),
// so the content is pinned here: RFC 5545 skeleton, TZID-qualified wall-clock times, escaping of
// TEXT fields, CRLF line endings, and the DTEND fallback to the entry-window close.

const pass: EventPass = {
  passId: 'pass_01',
  passReference: 'PCI-EV-2026-000731',
  state: 'active',
  event: {
    id: 501,
    title: 'Summit 2026; Controls, Cost & Risk',
    startsAt: '2026-09-14 09:00:00',
    endsAt: '2026-09-14 17:30:00',
    timezone: 'Europe/London',
    venueName: 'QEII Centre',
    venueAddress: 'Broad Sanctuary, London',
  },
  registrationState: 'registered',
  paymentState: 'paid',
  admissionMode: 'hybrid',
  validity: { opensAt: '2026-09-14 07:30:00', closesAt: '2026-09-14 12:00:00' },
  entryToken: 'EVT-OK-501-x',
  history: [],
}

describe('buildPassIcs', () => {
  it('produces a VCALENDAR/VEVENT with TZID wall-clock start and end', () => {
    const ics = buildPassIcs(pass, new Date('2026-07-29T10:00:00Z'))
    expect(ics.startsWith('BEGIN:VCALENDAR\r\n')).toBe(true)
    expect(ics).toContain('BEGIN:VEVENT')
    expect(ics).toContain('DTSTART;TZID=Europe/London:20260914T090000')
    expect(ics).toContain('DTEND;TZID=Europe/London:20260914T173000')
    expect(ics).toContain('DTSTAMP:20260729T100000Z')
    expect(ics).toContain('UID:pci-event-pass-pass_01@projectcontrolsinstitute.org')
    expect(ics.trimEnd().endsWith('END:VCALENDAR')).toBe(true)
  })

  it('escapes commas and semicolons in SUMMARY and LOCATION', () => {
    const ics = buildPassIcs(pass)
    expect(ics).toContain('SUMMARY:Summit 2026\\; Controls\\, Cost & Risk')
    expect(ics).toContain('LOCATION:QEII Centre\\, Broad Sanctuary\\, London')
  })

  it('uses CRLF line endings throughout', () => {
    const ics = buildPassIcs(pass)
    // No bare \n: every newline is preceded by \r.
    expect(ics.replace(/\r\n/g, '')).not.toContain('\n')
  })

  it('mentions the pass reference and entry window in the description', () => {
    const ics = buildPassIcs(pass)
    expect(ics).toContain('PCI-EV-2026-000731')
    expect(ics).toContain('2026-09-14 07:30:00 to 2026-09-14 12:00:00')
  })

  it('falls back to the entry-window close when the event has no end time', () => {
    const noEnd: EventPass = { ...pass, event: { ...pass.event, endsAt: null } }
    const ics = buildPassIcs(noEnd)
    expect(ics).toContain('DTEND;TZID=Europe/London:20260914T120000')
  })

  it('icsEscape and toIcsLocal handle edge input', () => {
    expect(icsEscape('a,b;c\\d\ne')).toBe('a\\,b\\;c\\\\d\\ne')
    expect(toIcsLocal('2026-01-02 03:04:05')).toBe('20260102T030405')
    expect(toIcsLocal('garbage')).toBe('')
  })
})
