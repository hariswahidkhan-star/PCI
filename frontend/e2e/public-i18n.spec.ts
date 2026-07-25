import { test, expect } from '@playwright/test'
import { captureStoryEvidence, preparePublicJourney } from './util'

// Public-website internationalisation: the backend renders every page in the active language
// (?lang= wins and is persisted to the pci_lang cookie — Core/I18nContent.cs), rewriting
// <html lang dir>. The visible switcher is the top-bar globe button injected by assets/premium.js.
test.describe('public site i18n', () => {
  test('every supported language renders translated home copy and navigation in sequence', async ({ page }, testInfo) => {
    await preparePublicJourney(page)
    const languages = [
      { code: 'en', htmlLang: 'en-GB', heading: 'The credential for the people who control projects', about: 'About', rtl: false },
      { code: 'ko', htmlLang: 'ko', heading: '프로젝트를 관리하는 사람들을 위한 자격증', about: '개요', rtl: false },
      { code: 'ar', htmlLang: 'ar', heading: 'الشهادة المخصصة للأشخاص الذين يديرون المشاريع', about: 'نظرة عامة', rtl: true },
      { code: 'es', htmlLang: 'es', heading: 'La credencial para las personas que controlan los proyectos', about: 'Resumen', rtl: false },
      { code: 'fr', htmlLang: 'fr', heading: 'Le titre de compétence pour les personnes qui contrôlent les projets', about: 'Aperçu', rtl: false },
      { code: 'zh', htmlLang: 'zh', heading: '为掌控项目的人士打造的资质认证', about: '概述', rtl: false },
      { code: 'ru', htmlLang: 'ru', heading: 'Квалификация для тех, кто управляет проектами', about: 'Обзор', rtl: false },
    ]

    for (const language of languages) {
      const response = await page.goto(`/?lang=${language.code}`)
      expect(response?.status() ?? 0, `${language.code} home should load`).toBeLessThan(400)
      await expect(page.locator('html')).toHaveAttribute('lang', language.htmlLang)
      if (language.rtl) await expect(page.locator('html')).toHaveAttribute('dir', 'rtl')
      else await expect(page.locator('html')).not.toHaveAttribute('dir', 'rtl')
      await expect(page.getByRole('heading', { level: 1 }).first()).toContainText(language.heading)
      await expect(page.locator('header').getByText(language.about, { exact: true }).first()).toBeAttached()

      const path = testInfo.outputPath(`A3-${language.code}-home.png`)
      await page.screenshot({ path, animations: 'disabled' })
      await testInfo.attach(`A3-${language.code}-home`, { path, contentType: 'image/png' })
    }
  })

  test('?lang=fr serves the page as French (html lang)', async ({ page }) => {
    const resp = await page.goto('/?lang=fr')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.locator('html')).toHaveAttribute('lang', 'fr')
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
  })

  test('@cross-browser ?lang=ar serves Arabic right-to-left (html lang + dir)', async ({ page }, testInfo) => {
    await page.goto('/?lang=ar')
    await expect(page.locator('html')).toHaveAttribute('lang', 'ar')
    await expect(page.locator('html')).toHaveAttribute('dir', 'rtl')
    await captureStoryEvidence(page, testInfo, 'A3', 'arabic-rtl')
  })

  test('the language choice persists across navigation via the cookie', async ({ page }) => {
    await page.goto('/?lang=es')
    await expect(page.locator('html')).toHaveAttribute('lang', 'es')
    // No ?lang here — the pci_lang cookie alone must keep the site in Spanish.
    await page.goto('/about.html')
    await expect(page.locator('html')).toHaveAttribute('lang', 'es')
  })

  test('the visible language switcher navigates to a translated page', async ({ page }) => {
    await preparePublicJourney(page)
    await page.goto('/')
    await expect(page.locator('html')).toHaveAttribute('lang', 'en-GB')
    await page.getByRole('button', { name: 'Select language' }).click()
    await page.getByRole('menuitem', { name: 'Français' }).click()
    await expect(page).toHaveURL(/[?&]lang=fr/)
    await expect(page.locator('html')).toHaveAttribute('lang', 'fr')
  })

  test('the announcement follows the active visitor language and keeps a language-stable dismissal key', async ({ page }, testInfo) => {
    await page.addInitScript(() => {
      try { localStorage.setItem('pci-cookie-consent', 'essential') } catch { /* no storage */ }
    })
    const announcementResponse = page.waitForResponse((response) => response.url().endsWith('/api/announcement'))
    await page.goto('/?lang=ar')
    const configResponse = await announcementResponse
    expect(configResponse.ok()).toBeTruthy()
    const config = (await configResponse.json()) as { key: string; title: string; dismiss: string }
    expect(config.title).toContain('تُفتح الشهادات للامتحان')
    expect(config.dismiss).toBe('متابعة التصفح')

    const dialog = page.locator('#pciAnx')
    await expect(dialog).toBeVisible()
    await expect(dialog.getByRole('heading')).toHaveText(config.title)
    await captureStoryEvidence(page, testInfo, 'A4', 'arabic-announcement')
    await dialog.locator('.pci-anx-secondary').click()
    expect(await page.evaluate((key) => sessionStorage.getItem(`pci.anx.${key}`), config.key)).toBe('1')
  })
})
