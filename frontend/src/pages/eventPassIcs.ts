// Client-side ICS (RFC 5545) generation for one event pass — used by the "Add to calendar" action
// on My Event Passes. Generated from the typed pass the page already holds, so it works in
// fixtures mode today and unchanged once the backend ships. Times are emitted as wall-clock with
// TZID (the pass carries the event's IANA timezone), which is exactly what calendar apps expect
// for a physical event at a venue.

import type { EventPass } from '../api/types'

/** Escape TEXT per RFC 5545 §3.3.11: backslash, semicolon, comma, newline. */
export function icsEscape(s: string): string {
  return s.replace(/\\/g, '\\\\').replace(/;/g, '\\;').replace(/,/g, '\\,').replace(/\r?\n/g, '\\n')
}

/** 'YYYY-MM-DD HH:MM:SS' -> 'YYYYMMDDTHHMMSS' (local wall-clock form, paired with TZID). */
export function toIcsLocal(dt: string): string {
  const m = /^(\d{4})-(\d{2})-(\d{2})[ T](\d{2}):(\d{2})(?::(\d{2}))?/.exec(dt.trim())
  if (!m) return ''
  return `${m[1]}${m[2]}${m[3]}T${m[4]}${m[5]}${m[6] ?? '00'}`
}

export function buildPassIcs(pass: EventPass, now: Date = new Date()): string {
  const tz = pass.event.timezone
  const start = toIcsLocal(pass.event.startsAt)
  // Calendar entries need an end; fall back to the entry-window close if the event has none.
  const end = toIcsLocal(pass.event.endsAt ?? pass.validity.closesAt)
  const stamp = now.toISOString().replace(/[-:]/g, '').replace(/\.\d{3}Z$/, 'Z')
  const location = pass.event.venueAddress
    ? `${pass.event.venueName}, ${pass.event.venueAddress}`
    : pass.event.venueName
  const description =
    `PCI event pass ${pass.passReference}. ` +
    `Entry window: ${pass.validity.opensAt} to ${pass.validity.closesAt} (${tz}). ` +
    `Bring your pass from the PCI student portal (My event passes).`

  const lines = [
    'BEGIN:VCALENDAR',
    'VERSION:2.0',
    'PRODID:-//Project Controls Institute//Event Pass//EN',
    'CALSCALE:GREGORIAN',
    'METHOD:PUBLISH',
    'BEGIN:VEVENT',
    `UID:pci-event-pass-${pass.passId}@projectcontrolsinstitute.org`,
    `DTSTAMP:${stamp}`,
    `DTSTART;TZID=${tz}:${start}`,
    ...(end ? [`DTEND;TZID=${tz}:${end}`] : []),
    `SUMMARY:${icsEscape(pass.event.title)}`,
    `LOCATION:${icsEscape(location)}`,
    `DESCRIPTION:${icsEscape(description)}`,
    'END:VEVENT',
    'END:VCALENDAR',
  ]
  // RFC 5545 requires CRLF line endings.
  return lines.join('\r\n') + '\r\n'
}
