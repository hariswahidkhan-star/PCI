import { useI18n, LANGS, type Lang } from '../i18n'

/** Compact language selector. Persists the choice and flips the document to RTL for Arabic. */
export default function LanguageSwitcher({ className }: { className?: string }) {
  const { lang, setLang, t } = useI18n()
  return (
    <label className={'lang-switch ' + (className ?? '')} title={t('common.language')}>
      <span className="sr-only">{t('common.language')}</span>
      <svg aria-hidden="true" viewBox="0 0 24 24" width="16" height="16" fill="none" stroke="currentColor" strokeWidth="1.8">
        <circle cx="12" cy="12" r="9" />
        <path d="M3 12h18M12 3c2.5 2.5 2.5 15 0 18M12 3c-2.5 2.5-2.5 15 0 18" />
      </svg>
      <select value={lang} onChange={(e) => setLang(e.target.value as Lang)} aria-label={t('common.language')}>
        {LANGS.map((l) => (
          <option key={l.code} value={l.code}>{l.native}</option>
        ))}
      </select>
    </label>
  )
}
