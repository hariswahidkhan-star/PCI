// Open a self-contained, print-friendly HTML document in a new tab (for receipts / certificates).
// The token-authenticated API returns JSON, not files, so we render these client-side from data the
// app already holds, then let the user Print / Save-as-PDF. Triggered from a click (user gesture),
// so the blob-URL tab is not popup-blocked.

function esc(s: unknown): string {
  return String(s ?? '')
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
}

export function openPrintable(title: string, bodyHtml: string, autoPrint = true): void {
  const doc = `<!doctype html><html><head><meta charset="utf-8"><title>${esc(title)}</title>
<style>
  body{font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,Helvetica,Arial,sans-serif;color:#0f172a;margin:0;padding:2.5rem;}
  .doc{max-width:720px;margin:0 auto;border:1px solid #e3e8ef;border-radius:12px;padding:2rem 2.25rem;}
  h1{font-size:1.4rem;margin:0 0 .25rem;} .muted{color:#64748b;} table{width:100%;border-collapse:collapse;margin-top:1rem;}
  td,th{text-align:left;padding:.5rem .25rem;border-bottom:1px solid #e3e8ef;} .r{text-align:right;}
  .brand{color:#1d4ed8;font-weight:800;letter-spacing:.02em;} .big{font-size:1.6rem;font-weight:800;text-align:center;margin:1.5rem 0;}
  @media print{.noprint{display:none;} body{padding:0;} .doc{border:0;}}
  .btn{display:inline-block;margin-top:1.5rem;padding:.55rem 1rem;background:#1d4ed8;color:#fff;border-radius:9px;border:0;font:inherit;font-weight:600;cursor:pointer;}
</style></head>
<body><div class="doc">${bodyHtml}
<div class="noprint" style="text-align:center"><button class="btn" onclick="window.print()">Print / Save as PDF</button></div>
</div>${autoPrint ? '<script>window.onload=function(){setTimeout(function(){try{window.print()}catch(e){}},300)}</' + 'script>' : ''}</body></html>`
  const url = URL.createObjectURL(new Blob([doc], { type: 'text/html' }))
  const w = window.open(url, '_blank')
  // revoke shortly after the tab has had time to load
  setTimeout(() => URL.revokeObjectURL(url), 60_000)
  if (!w) {
    // popup blocked — fall back to same-tab navigation
    location.href = url
  }
}

export { esc as escapeHtml }
