# E2E Evidence Manifest - 2026-07-23

Branch: `cursor/fix-pml-ai-e2e-d975`

## CI run

- CI run URL: https://github.com/hariswahidkhan-star/PCI/actions/runs/30002525905
- Conclusion: **success**
- e2e job: https://github.com/hariswahidkhan-star/PCI/actions/runs/30002525905/job/89190815700 (**pass**)
- Workflow: `.github/workflows/build.yml` → `e2e`
- CI artifact:
  - Artifact name: `playwright-report`
  - Source path: `frontend/playwright-report`
  - Retention: 7 days

## Playwright runtime configuration

Configured in `frontend/playwright.config.ts`:

- Test directory: `frontend/e2e`
- Projects: `chromium`, `firefox`, `webkit`, `mobile-chrome`, `mobile-safari`
- Screenshots: `only-on-failure`
- Video: `retain-on-failure`
- Trace: `on-first-retry`
- Reporter in CI: `github` plus `list`
- Web server: built backend DLL, waiting on `/api/health`
- CI installs: `npx playwright install --with-deps chromium firefox webkit`

## Evidence artifacts

| Artifact type | Location | Produced by | Status |
|---|---|---|---|
| CI e2e job log | GitHub Actions e2e job URL above | `npm run e2e` | **Published** |
| Playwright report artifact | Actions artifact `playwright-report` | workflow upload step | **Published** |
| Story screenshots | `frontend/test-results/**/<story-name>.png` | `storyScreenshot(...)` in specs | Produced during green CI run; may not be separately uploaded unless included in the report bundle |
| Failure screenshots / videos / traces | `frontend/test-results/**` | Playwright failure retention | Not expected for this green run |

## Execution summary

- Discovery shape: **91 listed executions** / **18 spec files** / **5 projects**
- Runtime: **CI-PASS** on run `30002525905`
- Do not regress this claim without a newer green e2e job URL.
