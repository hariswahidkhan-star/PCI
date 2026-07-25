#!/usr/bin/env bash
# Refresh the self-hosted PCI World brand fonts in wwwroot/assets/fonts/.
#
# PCI World serves Archivo and Inter from its own origin rather than fonts.googleapis.com, so that
# the brand typeface is present on first paint, so a network that cannot reach Google still gets the
# design, and so no third party is told who is reading an Institute page. See the FontLoader comment
# in backend/Core/WorldPages.cs.
#
# Run this only when the weights in the design system change or the upstream fonts are updated. The
# .woff2 files are committed, so a normal build needs no network access.
#
#   ./tools/fetch-brand-fonts.sh
#
# Both families are requested as VARIABLE fonts (wght@a..b), which is why one file covers the whole
# weight range instead of one per weight. Only the latin and latin-ext subsets are kept: they carry
# English plus Western European accents, and the browser falls back per-glyph for anything else.
set -euo pipefail

DEST="$(cd "$(dirname "$0")/.." && pwd)/wwwroot/assets/fonts"
# Google serves woff2 only to browsers that advertise support; a default curl UA gets ttf.
UA="Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120 Safari/537.36"
API="https://fonts.googleapis.com/css2?family=Archivo:wght@700..900&family=Inter:wght@400..700&display=swap"

mkdir -p "$DEST"
tmp="$(mktemp)"; trap 'rm -f "$tmp"' EXIT
curl -fsS -A "$UA" "$API" -o "$tmp"

python3 - "$tmp" "$DEST" "$UA" <<'PY'
import re, subprocess, sys
css_path, dest, ua = sys.argv[1], sys.argv[2], sys.argv[3]
css = open(css_path).read()
kept = 0
for subset, block in re.findall(r'/\* (\S+) \*/\s*@font-face \{(.*?)\}', css, re.S):
    if subset not in ('latin', 'latin-ext'):
        continue
    family = re.search(r"font-family: '([^']+)'", block).group(1).lower()
    url = re.search(r'url\((https://[^)]+)\)', block).group(1)
    out = f'{dest}/{family}-{subset}.woff2'
    subprocess.run(['curl', '-fsS', '-A', ua, url, '-o', out], check=True)
    with open(out, 'rb') as fh:
        if fh.read(4) != b'wOF2':
            sys.exit(f'{out} is not a woff2 file — upstream served something else')
    print(f'  {out.rsplit("/", 1)[-1]}')
    kept += 1
if kept != 4:
    sys.exit(f'expected 4 subset files (2 families x latin/latin-ext), wrote {kept}')
PY

echo
echo "Wrote 4 files to $DEST."
echo "If the unicode-range values changed upstream, copy them into the @font-face block in"
echo "backend/Core/WorldPages.cs — the ranges there must match the files, or glyphs silently vanish."
