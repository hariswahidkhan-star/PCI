#!/usr/bin/env python3
"""Render the PCI AI social graphics to PNG at 2x for LinkedIn image posts.

Each .html in this directory is one 1080x1350 (4:5) graphic. Output goes to
backend/wwwroot/assets/social/ so the site can serve it and the file is versioned
with everything else.

Why the window is taller than the target: headless Chromium reserves a fixed strip
of the window, so the layout viewport comes out shorter than --window-size while the
screenshot is taken at the full window height. A graphic sized in vh therefore stops
short and the page background fills the gap. We measure that overhead once, add it to
the window, then crop back to the exact frame — which keeps the HTML free of magic
numbers and keeps `height: 100vh` meaning what it says.

Usage:
    python3 build_social.py            # build every graphic
    python3 build_social.py 40-40-20   # build one
"""
import json
import pathlib
import subprocess
import sys
import tempfile

from PIL import Image

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parent.parent.parent
OUT = ROOT / "backend" / "wwwroot" / "assets" / "social"
CHROME = "/opt/pw-browsers/chromium-1194/chrome-linux/chrome"
PORT = 8899
W, H, SCALE = 1080, 1350, 2

FLAGS = [
    "--headless", "--disable-gpu", "--no-sandbox", "--hide-scrollbars",
    f"--force-device-scale-factor={SCALE}",
]


def viewport_overhead() -> int:
    """How many pixels shorter the layout viewport is than the requested window."""
    probe = pathlib.Path(tempfile.mkdtemp()) / "probe.html"
    probe.write_text(
        "<body style='margin:0'><div id=d style='height:100vh'></div>"
        "<script>document.title=document.getElementById('d').clientHeight</script>"
    )
    out = subprocess.run(
        [CHROME, *FLAGS, f"--window-size={W},{H}", "--dump-dom", probe.as_uri()],
        capture_output=True, text=True,
    ).stdout
    # the script writes the measured height into <title>
    try:
        measured = int(out.split("<title>")[1].split("</title>")[0])
    except (IndexError, ValueError):
        return 0
    return max(0, H - measured)


def render(name: str, overhead: int) -> pathlib.Path:
    OUT.mkdir(parents=True, exist_ok=True)
    raw = pathlib.Path(tempfile.mkdtemp()) / "raw.png"
    url = f"http://localhost:{PORT}/docs/formula-sheets/social/{name}.html"
    subprocess.run(
        [CHROME, *FLAGS, f"--window-size={W},{H + overhead}",
         f"--screenshot={raw}", url],
        capture_output=True, check=True,
    )
    img = Image.open(raw).crop((0, 0, W * SCALE, H * SCALE))
    dest = OUT / f"{name}.png"
    img.save(dest, optimize=True)
    return dest


def verify(path: pathlib.Path) -> None:
    """The frame must be fully painted — no background strip left at the foot."""
    im = Image.open(path).convert("RGB")
    prev, steps = None, []
    for y in range(im.height - 1, int(im.height * 0.85), -4):
        px = im.getpixel((40, y))
        if prev and sum(abs(a - b) for a, b in zip(px, prev)) > 12:
            steps.append(y)
        prev = px
    if steps:
        raise SystemExit(f"FAIL {path.name}: unpainted strip, discontinuity at y={steps[0]}")
    print(f"OK  {path.relative_to(ROOT)}  {im.width}x{im.height}")


if __name__ == "__main__":
    names = sys.argv[1:] or [p.stem for p in sorted(HERE.glob("*.html"))]
    over = viewport_overhead()
    print(f"viewport overhead: {over}px")
    for n in names:
        verify(render(n, over))
