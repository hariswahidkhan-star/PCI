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

# Primary blue, crimson, ink — the three colours sampled from the rendered site. A
# graphic that carries none of them did not render.
BRAND = ((0x1D, 0x4E, 0xD8), (0xC1, 0x33, 0x29), (0x0F, 0x17, 0x2A))

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


GUARD = """<script>
for (const el of document.querySelectorAll('*')) {
  if (el.scrollHeight - el.clientHeight > 2 &&
      getComputedStyle(el).overflowY === 'hidden') {
    document.title = 'OVERFLOW ' + (el.className || el.tagName) +
      ' by ' + (el.scrollHeight - el.clientHeight) + 'px';
    break;
  }
}
</script>"""


def url_for(path: pathlib.Path) -> str:
    """The served URL for a file in the repo. Everything is served from the repo root
    because the graphics reference fonts and the logo by absolute path."""
    return f"http://localhost:{PORT}/{path.relative_to(ROOT).as_posix()}"


def overflow(src: pathlib.Path, overhead: int) -> None:
    """Fail on content that a hidden overflow silently ate.

    A graphic whose grid overruns is clipped, not broken: the frame is fully painted,
    the footer still carries ink, and the lost row simply is not there. No pixel test
    finds that, because nothing about the image is wrong — a row is just missing.

    So ask the browser instead. We render a throwaway copy of the page carrying a
    script that looks for any element clipping its own content, and read the verdict
    out of the title. The copy sits beside the original so every relative and absolute
    URL in it still resolves, and injecting here rather than in each page means the
    hand-written graphics get the same check as the generated ones.
    """
    probe = src.with_name(f".{src.stem}.guard.html")
    probe.write_text(src.read_text(encoding="utf-8") + GUARD, encoding="utf-8")
    try:
        dom = subprocess.run(
            [CHROME, *FLAGS, f"--window-size={W},{H + overhead}", "--dump-dom",
             url_for(probe)],
            capture_output=True, text=True,
        ).stdout
    finally:
        probe.unlink(missing_ok=True)
    if "<title>OVERFLOW " in dom:
        detail = dom.split("<title>OVERFLOW ")[1].split("</title>")[0]
        raise SystemExit(f"FAIL {src.stem}: content is clipped — {detail}")


def render(src: pathlib.Path, dest: pathlib.Path, overhead: int) -> pathlib.Path:
    dest.parent.mkdir(parents=True, exist_ok=True)
    raw = pathlib.Path(tempfile.mkdtemp()) / "raw.png"
    subprocess.run(
        [CHROME, *FLAGS, f"--window-size={W},{H + overhead}",
         f"--screenshot={raw}", url_for(src)],
        capture_output=True, check=True,
    )
    Image.open(raw).crop((0, 0, W * SCALE, H * SCALE)).save(dest, optimize=True)
    return dest


def build(src: pathlib.Path, dest: pathlib.Path, overhead: int) -> pathlib.Path:
    """Check, render, verify. The one entry point callers should use."""
    overflow(src, overhead)
    verify(render(src, dest, overhead))
    return dest


def verify(path: pathlib.Path) -> None:
    """The frame must be fully painted, and must actually be the graphic.

    A wrong URL renders Chromium's error page: white, near-empty, and it sails
    through a completeness check because there is nothing to be incomplete. Test for
    the brand instead — every graphic carries a substantial field of blue, crimson or
    ink, and an error page carries none of them.

    Check all three, not blue alone: a slide deliberately set on the dark ground has no
    blue on it at all, and a blue-only test called that a failed render.
    """
    im = Image.open(path).convert("RGB")
    # Sample the full-resolution frame on a stride. Resizing first averages a small
    # brand element into a white ground and hides it, which failed this check once.
    pts = [(x, y) for y in range(0, im.height, 9) for x in range(0, im.width, 9)]
    brand = 0
    for x, y in pts:
        px = im.getpixel((x, y))
        if any(all(abs(px[i] - c[i]) < 70 for i in range(3)) for c in BRAND):
            brand += 1
    if brand < len(pts) * 0.02:
        raise SystemExit(
            f"FAIL {path.name}: no brand colour in the frame — bad URL or failed render")

    # The footer must actually be on the frame. A layout that overruns pushes it off
    # the bottom, leaving only its rule behind — which looks fine until you read the
    # edge. Test the whole tail, not a fixed strip: graphics carry different amounts of
    # bottom padding, and pinning this to one layout's footer position failed a good
    # render whose footer simply sat higher.
    band = im.crop((0, int(im.height * 0.88), im.width, im.height - 4))
    ink = sum(1 for px in band.getdata() if sum(px) < 620)
    if ink < band.width * band.height * 0.002:
        raise SystemExit(f"FAIL {path.name}: nothing in the footer region — content overran the frame")

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
        build(HERE / f"{n}.html", OUT / f"{n}.png", over)
