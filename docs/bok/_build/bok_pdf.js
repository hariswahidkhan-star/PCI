// Render the combined BoK HTML to a paginated A4 PDF using headless Chromium.
// Usage: node bok_pdf.js <input.html> <output.pdf>
const { chromium } = require('playwright-core');
const EXE = '/opt/pw-browsers/chromium-1194/chrome-linux/chrome';
const [inHtml, outPdf] = process.argv.slice(2);
(async () => {
  const browser = await chromium.launch({ executablePath: EXE, args: ['--no-sandbox'] });
  const page = await browser.newPage();
  await page.goto('file://' + inHtml, { waitUntil: 'networkidle', timeout: 120000 });
  await page.pdf({
    path: outPdf,
    format: 'A4',
    printBackground: true,
    margin: { top: '20mm', bottom: '18mm', left: '18mm', right: '18mm' },
    displayHeaderFooter: true,
    headerTemplate: '<div style="font-size:7pt;color:#94a3b8;width:100%;padding:0 16mm;font-family:sans-serif;">PCP‑AI Body of Knowledge — v1 (draft for SME verification)</div>',
    footerTemplate: '<div style="font-size:7pt;color:#94a3b8;width:100%;padding:0 16mm;text-align:center;font-family:sans-serif;">Project Controls Institute &nbsp;·&nbsp; Page <span class="pageNumber"></span> of <span class="totalPages"></span></div>',
  });
  await browser.close();
  console.log('PDF written:', outPdf);
})().catch(e => { console.error(e); process.exit(1); });
