# Social cover images

Ready-to-upload cover/banner images for the PCI Global and PCI World social profiles, built from the
site's own brand assets (`backend/wwwroot/assets`: Archivo/Inter fonts, the PCI plate logo, the
navy/crimson palette) so they match the website exactly. The PCI World set swaps the crimson accent
for the World gilt gold and the Gantt/S-curve motif for an orbit-and-constellation motif.

## The images

**PCI Global** (www.projectcontrolsinstitute.org · pciglobal.ai · pciworld.org):

| File | Upload as | Rendered at |
|---|---|---|
| `pci-facebook-cover-1640x624.png` | Facebook Page cover (displays 820×312 desktop) | 2× (1640×624) |
| `pci-linkedin-company-cover-2256x382.png` | LinkedIn company page cover (displays 1128×191) | 2× (2256×382) |
| `pci-linkedin-personal-banner-3168x792.png` | LinkedIn personal profile banner (displays 1584×396) | 2× (3168×792) |

**PCI World** (pciworld.org):

| File | Upload as | Rendered at |
|---|---|---|
| `pci-world-facebook-cover-1640x624.png` | Facebook Page cover | 2× (1640×624) |
| `pci-world-linkedin-company-cover-2256x382.png` | LinkedIn company page cover | 2× (2256×382) |
| `pci-world-linkedin-personal-banner-3168x792.png` | LinkedIn personal profile banner | 2× (3168×792) |

All three are rendered at twice the platform's display size so they stay sharp on high-DPI screens;
the platforms downscale them automatically.

Safe zones are already respected in the layouts:

- **Facebook** crops to the central ~640px on mobile — all content sits inside that band.
- **LinkedIn company** overlays the square company logo on the lower left — content starts right of it.
- **LinkedIn personal** overlays the round profile photo on the lower left — content sits centre-right.

## Editing / regenerating

Each cover is a plain HTML art board (`facebook.html`, `linkedin-company.html`,
`linkedin-personal.html` + `shared.css`) sized to the platform's exact CSS pixel dimensions. Edit the
text/layout there, then re-render:

```bash
cd branding/social-covers
npm i playwright-core   # one-off; browsers are not downloaded
node shoot.js           # set CHROME_PATH=/path/to/chrome if needed
```
