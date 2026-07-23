import { test, expect } from '@playwright/test'

// Public-website internationalisation: the backend renders every page in the active language
// (?lang= wins and is persisted to the pci_lang cookie — Core/I18nContent.cs), rewriting
// <html lang dir>. The visible switcher is the top-bar globe button injected by assets/premium.js.
test.describe('public site i18n', () => {
  test('?lang=fr serves the page as French (html lang)', async ({ page }) => {
    const resp = await page.goto('/?lang=fr')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.locator('html')).toHaveAttribute('lang', 'fr')
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
  })

  test('@cross-browser ?lang=ar serves Arabic right-to-left (html lang + dir)', async ({ page }) => {
    await page.goto('/?lang=ar')
    await expect(page.locator('html')).toHaveAttribute('lang', 'ar')
    await expect(page.locator('html')).toHaveAttribute('dir', 'rtl')
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
