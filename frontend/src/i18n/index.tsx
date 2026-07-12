import { createContext, useContext, useEffect, useState, type ReactNode } from 'react'
import { CATALOG } from './catalog'

// Supported languages. `dir: 'rtl'` drives the Arabic right-to-left layout.
export interface LangDef { code: Lang; label: string; native: string; dir: 'ltr' | 'rtl' }
export type Lang = 'en' | 'ko' | 'ar' | 'es' | 'fr' | 'zh' | 'ru'

export const LANGS: LangDef[] = [
  { code: 'en', label: 'English', native: 'English', dir: 'ltr' },
  { code: 'ko', label: 'Korean', native: '한국어', dir: 'ltr' },
  { code: 'ar', label: 'Arabic', native: 'العربية', dir: 'rtl' },
  { code: 'es', label: 'Spanish', native: 'Español', dir: 'ltr' },
  { code: 'fr', label: 'French', native: 'Français', dir: 'ltr' },
  { code: 'zh', label: 'Chinese', native: '中文', dir: 'ltr' },
  { code: 'ru', label: 'Russian', native: 'Русский', dir: 'ltr' },
]

const STORAGE_KEY = 'pci.lang'
const isLang = (v: string | null): v is Lang => !!v && LANGS.some((l) => l.code === v)

function initialLang(): Lang {
  try {
    const stored = localStorage.getItem(STORAGE_KEY)
    if (isLang(stored)) return stored
    const nav = (navigator.language || 'en').slice(0, 2).toLowerCase()
    if (isLang(nav)) return nav
  } catch { /* storage blocked */ }
  return 'en'
}

/** Reflect the language on <html> so CSS (dir=rtl) and screen readers pick it up. */
function applyToDocument(lang: Lang) {
  const def = LANGS.find((l) => l.code === lang) ?? LANGS[0]
  try {
    document.documentElement.lang = lang
    document.documentElement.dir = def.dir
  } catch { /* SSR / no document */ }
}

interface I18nValue {
  lang: Lang
  dir: 'ltr' | 'rtl'
  setLang: (l: Lang) => void
  t: (key: string, vars?: Record<string, string | number>) => string
}

const I18nContext = createContext<I18nValue | null>(null)

export function I18nProvider({ children }: { children: ReactNode }) {
  const [lang, setLangState] = useState<Lang>(initialLang)

  useEffect(() => { applyToDocument(lang) }, [lang])

  const setLang = (l: Lang) => {
    setLangState(l)
    try { localStorage.setItem(STORAGE_KEY, l) } catch { /* ignore */ }
  }

  // Look up a key for the active language; fall back to English, then the key itself. `vars`
  // interpolates {name} placeholders so counts/names can sit inside a translated sentence.
  const t = (key: string, vars?: Record<string, string | number>) => {
    const entry = CATALOG[key]
    let out = (entry && (entry[lang] ?? entry.en)) ?? key
    if (vars) for (const [k, v] of Object.entries(vars)) out = out.replace(new RegExp(`\\{${k}\\}`, 'g'), String(v))
    return out
  }

  const dir = LANGS.find((l) => l.code === lang)?.dir ?? 'ltr'
  return <I18nContext.Provider value={{ lang, dir, setLang, t }}>{children}</I18nContext.Provider>
}

export function useI18n(): I18nValue {
  const ctx = useContext(I18nContext)
  if (!ctx) throw new Error('useI18n must be used within I18nProvider')
  return ctx
}

/** Convenience: const t = useT() */
export function useT() {
  return useI18n().t
}
