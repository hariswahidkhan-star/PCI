// SQ-2 — ESLint flat config for the React/TypeScript frontend (student + admin SPAs).
//
// Scope and intent: this is a *correctness* gate, not a style gate (formatting is owned by
// .editorconfig / the editor, not enforced here). The build already runs `tsc --noEmit` with
// strict + noUnusedLocals/Parameters, so type-level and unused-symbol checks are covered there;
// ESLint adds the things the type-checker cannot see — chiefly the React Hooks rules, which catch
// real runtime bugs (conditional hooks, missing deps). The high-noise stylistic rules that would
// otherwise flood a 110-file existing codebase are dialled down to warnings so the gate stays
// meaningful (errors == real problems) and green.
import js from '@eslint/js'
import tseslint from 'typescript-eslint'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import globals from 'globals'

export default tseslint.config(
  {
    // Build output, deps, coverage, and Playwright report are never linted.
    ignores: ['dist', 'dist-admin', 'node_modules', 'coverage', 'playwright-report', 'test-results'],
  },
  js.configs.recommended,
  ...tseslint.configs.recommended,
  {
    files: ['**/*.{ts,tsx}'],
    languageOptions: {
      ecmaVersion: 2022,
      sourceType: 'module',
      globals: { ...globals.browser },
    },
    plugins: {
      'react-hooks': reactHooks,
      'react-refresh': reactRefresh,
    },
    rules: {
      // Real-bug rules — keep as errors.
      ...reactHooks.configs.recommended.rules,
      'react-refresh/only-export-components': ['warn', { allowConstantExport: true }],

      // Covered more precisely by tsconfig (noUnusedLocals/Parameters) — avoid double-reporting,
      // but still allow intentionally-unused args prefixed with _.
      '@typescript-eslint/no-unused-vars': ['warn', { argsIgnorePattern: '^_', varsIgnorePattern: '^_' }],
      // The codebase uses side-effecting ternaries/short-circuits as compact if/else
      // (e.g. `cond ? set.add(x) : set.delete(x)`). Permit those while still catching
      // genuinely dead expressions.
      '@typescript-eslint/no-unused-expressions': ['error', { allowShortCircuit: true, allowTernary: true }],
      // Pragmatic downgrades: these fire widely on the existing code and are not correctness bugs.
      '@typescript-eslint/no-explicit-any': 'warn',
      '@typescript-eslint/no-empty-object-type': 'warn',
      '@typescript-eslint/ban-ts-comment': 'warn',
    },
  },
  {
    // Node-context config files.
    files: ['*.config.{ts,js}', 'vite.*.config.ts'],
    languageOptions: { globals: { ...globals.node } },
  },
)
