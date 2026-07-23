import { test, expect } from '@playwright/test'

// Public-website internationalisation: the backend renders every page in the active language
// (?lang= wins and is persisted to the pci_lang cookie — Core/I18nContent.cs), rewriting
// <html lang dir>. The visible switcher is the top-bar globe button injected by assets/premium.js.
test.describe('public site i18n', () => {
  for (const lang of ['en', 'ko', 'ar', 'es', 'fr', 'zh', 'ru'] as const) {
    test(`?lang=${lang} serves the requested language metadata`, async ({ page }) => {
      const resp = await page.goto(`/?lang=${lang}`)
      expect(resp?.status() ?? 0).toBeLessThan(400)
      const expectedLang = lang === 'en' ? /^(en|en-GB)$/ : lang
      await expect(page.locator('html')).toHaveAttribute('lang', expectedLang)
      if (lang === 'ar') {
        await expect(page.locator('html')).toHaveAttribute('dir', 'rtl')
      } else {
        await expect(page.locator('html')).not.toHaveAttribute('dir', 'rtl')
      }
      await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
    })
  }

  test('/certifications/pml-ai serves the third certification slug', async ({ page }) => {
    const resp = await page.goto('/certifications/pml-ai')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.getByRole('heading', { level: 1 }).first()).toContainText(/PML-AI|Project Management/i)
  })

  test('the language choice persists across navigation via the cookie', async ({ page }) => {
    await page.goto('/?lang=es')
    await expect(page.locator('html')).toHaveAttribute('lang', 'es')
    // No ?lang here — the pci_lang cookie alone must keep the site in Spanish.
    await page.goto('/about.html')
    await expect(page.locator('html')).toHaveAttribute('lang', 'es')
  })

  test('the visible language switcher navigates to a translated page', async ({ page }) => {
    await page.goto('/')
    await expect(page.locator('html')).toHaveAttribute('lang', 'en-GB')
    await page.getByRole('button', { name: 'Select language' }).click()
    await page.getByRole('menuitem', { name: 'Français' }).click()
    await expect(page).toHaveURL(/[?&]lang=fr/)
    await expect(page.locator('html')).toHaveAttribute('lang', 'fr')
  })
})
