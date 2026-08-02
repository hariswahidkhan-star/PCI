// Renders the three social cover art boards to PNG at 2x device scale.
// Usage: npm i playwright-core && node shoot.js
// Set CHROME_PATH if Chromium is not at the default Playwright location.
const { chromium } = require('playwright-core');
const path = require('path');

const boards = [
  { file: 'facebook.html',          w: 820,  h: 312, out: 'pci-ai-facebook-cover-1640x624.png' },
  { file: 'linkedin-company.html',  w: 1128, h: 191, out: 'pci-ai-linkedin-company-cover-2256x382.png' },
  { file: 'linkedin-personal.html', w: 1584, h: 396, out: 'pci-ai-linkedin-personal-banner-3168x792.png' },
  { file: 'world-facebook.html',          w: 820,  h: 312, out: 'pci-world-facebook-cover-1640x624.png' },
  { file: 'world-linkedin-company.html',  w: 1128, h: 191, out: 'pci-world-linkedin-company-cover-2256x382.png' },
  { file: 'world-linkedin-personal.html', w: 1584, h: 396, out: 'pci-world-linkedin-personal-banner-3168x792.png' },
];

(async () => {
  const browser = await chromium.launch({
    executablePath: process.env.CHROME_PATH || '/opt/pw-browsers/chromium-1194/chrome-linux/chrome',
    args: ['--no-sandbox'],
  });
  for (const b of boards) {
    const ctx = await browser.newContext({
      viewport: { width: b.w, height: b.h },
      deviceScaleFactor: 2,
    });
    const page = await ctx.newPage();
    await page.goto('file://' + path.resolve(__dirname, b.file));
    await page.evaluate(() => document.fonts.ready);
    await page.waitForTimeout(200);
    await page.screenshot({ path: path.join(__dirname, b.out) });
    await ctx.close();
    console.log('done', b.out);
  }
  await browser.close();
})();
