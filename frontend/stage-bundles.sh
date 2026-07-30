#!/usr/bin/env bash
# Build the three React bundles and stage them into backend/wwwroot exactly as the Dockerfile does.
#
# WHY THIS EXISTS AS A SCRIPT rather than inline workflow steps: Playwright's webServer runs the
# built backend DLL directly, not the Docker image, so the bundles the Dockerfile normally copies in
# are absent and every React route 404s. Both e2e jobs (SQLite and MySQL) need identical staging.
# When this lived as a copy-pasted block it was added to one job and not the other, and the MySQL
# leg silently served 404s for /world-app until the accessibility spec failed on a missing heading.
# One script, called from both jobs, is the fix that cannot drift.
#
# Run from the frontend/ directory.
set -euo pipefail

npm run build

root=../backend/wwwroot
mkdir -p "$root/app" "$root/admin" "$root/world-app" "$root/world-admin-app"

cp -r dist/. "$root/app/"
cp -r dist-admin/. "$root/admin/"
cp dist-admin/admin.html "$root/admin/index.html"
cp -r dist-world/. "$root/world-app/"
cp dist-world/world.html "$root/world-app/index.html"
cp -r dist-worldadmin/. "$root/world-admin-app/"
cp dist-worldadmin/worldadmin.html "$root/world-admin-app/index.html"

# Fail loudly here rather than as a mystery 404 inside a browser test.
for shell in "$root/app/index.html" "$root/admin/index.html" "$root/world-app/index.html" "$root/world-admin-app/index.html"; do
  [ -s "$shell" ] || { echo "stage-bundles: missing or empty $shell" >&2; exit 1; }
done
echo "stage-bundles: staged /app, /admin, /world-app and /world-admin-app into backend/wwwroot"
