/* PCI motion runtime — animations on top of the existing design.
   html.pv gates all motion; without JS (or with reduced motion) the site is
   fully visible and static. Adds scroll reveals, header shadow, progress bar,
   gentle hero parallax and cursor-following card glow. */
(function () {
  'use strict';
  try {
    if (!('IntersectionObserver' in window)) return;
    if (window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches) return;

    var root = document.documentElement;
    root.classList.add('pv');

    var start = function () {
      try {
        /* scroll reveal */
        var targets = document.querySelectorAll(
          '.sec>.wrap>.hblock,.gcard,.cert-card,.pcard,.mcard,.dcard,details.faq,.nl-band .wrap,.ftcols>div,.notice'
        );
        if (targets.length) {
          var io = new IntersectionObserver(function (entries) {
            for (var i = 0; i < entries.length; i++) {
              if (entries[i].isIntersecting) {
                entries[i].target.classList.add('pv-in');
                io.unobserve(entries[i].target);
              }
            }
          }, { threshold: 0.12, rootMargin: '0px 0px -36px 0px' });
          for (var t = 0; t < targets.length; t++) {
            var r = targets[t].getBoundingClientRect();
            if (r.top < window.innerHeight && r.bottom > 0) targets[t].classList.add('pv-in');
            else io.observe(targets[t]);
          }
        }

        /* header shadow, progress bar, gentle hero parallax */
        var hdr = document.querySelector('.hdr');
        var heroBg = document.querySelector('.hero .hero-bg');
        var bar = document.createElement('div');
        bar.id = 'pvbar';
        document.body.appendChild(bar);
        var ticking = false;
        var onScroll = function () {
          if (ticking) return;
          ticking = true;
          requestAnimationFrame(function () {
            var y = window.scrollY || document.documentElement.scrollTop || 0;
            if (hdr) hdr.classList.toggle('pv-scrolled', y > 8);
            var doc = document.documentElement;
            var max = (doc.scrollHeight - doc.clientHeight) || 1;
            bar.style.transform = 'scaleX(' + Math.min(1, y / max) + ')';
            if (heroBg && y < window.innerHeight * 1.2) {
              heroBg.style.transform = 'translate3d(0,' + (y * 0.18) + 'px,0)';
            }
            ticking = false;
          });
        };
        window.addEventListener('scroll', onScroll, { passive: true });
        onScroll();

        /* cursor-following glow inside cards */
        if (window.matchMedia('(hover: hover) and (pointer: fine)').matches) {
          document.addEventListener('mousemove', function (e) {
            var card = e.target && e.target.closest &&
              e.target.closest('.gcard,.qcard,.cert-card,.feat');
            if (!card) return;
            var b = card.getBoundingClientRect();
            card.style.setProperty('--mx', ((e.clientX - b.left) / b.width * 100) + '%');
            card.style.setProperty('--my', ((e.clientY - b.top) / b.height * 100) + '%');
          }, { passive: true });

          /* magnetic primary CTAs: the button leans gently toward the cursor */
          var mags = document.querySelectorAll('.hero-cta .btn, .redband .btn, .cta-row .btn-red');
          for (var m = 0; m < mags.length; m++) {
            (function (btn) {
              btn.classList.add('pv-mag');
              btn.addEventListener('mousemove', function (e) {
                var b = btn.getBoundingClientRect();
                var dx = (e.clientX - b.left - b.width / 2) / (b.width / 2);
                var dy = (e.clientY - b.top - b.height / 2) / (b.height / 2);
                btn.style.transform = 'translate(' + (dx * 4).toFixed(1) + 'px,' + (dy * 3).toFixed(1) + 'px)';
              }, { passive: true });
              btn.addEventListener('mouseleave', function () { btn.style.transform = ''; });
            })(mags[m]);
          }
        }
      } catch (e) { /* static page remains fully usable */ }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', start);
    else start();
  } catch (e) { /* no-op */ }
})();

/* Mobile nav accordion — the header dropdown groups (Membership, Certifications, …) collapse by
   default in the drawer; the caret button toggles each group. The label itself stays a real link. */
(function () {
  try {
    document.addEventListener('click', function (e) {
      var t = e.target.closest ? e.target.closest('.nav .sub-toggle') : null;
      if (!t) return;
      e.preventDefault();
      var item = t.closest('.nav-item.has-sub');
      if (!item) return;
      var open = item.classList.toggle('open');
      t.setAttribute('aria-expanded', open ? 'true' : 'false');
    });
  } catch (e) { /* no-op */ }
})();

/* ===== PCI-LANGBAR: top-bar language button (multilingual / i18n) =====
   A compact globe+code button pinned at the right of the top promo bar (fixed top-right on the
   few pages without one). Opens a dropdown of the 7 supported languages (native names mirror
   Core/I18nContent.Menu); selecting one navigates to ?lang=<code> on the current page — the
   server persists the choice to the pci_lang cookie, injects translations and sets
   <html lang dir> (dir="rtl" for Arabic). The choice is also kept in localStorage. */
(function () {
  'use strict';
  try {
    var LANGS = [
      ['en', 'English'], ['ko', '한국어'], ['ar', 'العربية'],
      ['es', 'Español'], ['fr', 'Français'], ['zh', '中文'], ['ru', 'Русский']
    ];
    function currentLang() {
      var m = document.cookie.match(/(?:^|;\s*)pci_lang=([a-z]{2})/);
      var c = m ? m[1] : ((document.documentElement.lang || 'en').slice(0, 2).toLowerCase());
      for (var i = 0; i < LANGS.length; i++) if (LANGS[i][0] === c) return c;
      return 'en';
    }
    function urlFor(code) {
      var p = new URLSearchParams(location.search);
      p.set('lang', code);
      return location.pathname + '?' + p.toString() + location.hash;
    }
    function build() {
      if (document.querySelector('.pci-langwrap')) return;
      var cur = currentLang();
      var wrap = document.createElement('div');
      wrap.className = 'pci-langwrap';
      var btn = document.createElement('button');
      btn.type = 'button';
      btn.className = 'pci-langbtn';
      btn.setAttribute('aria-haspopup', 'true');
      btn.setAttribute('aria-expanded', 'false');
      btn.setAttribute('aria-label', 'Select language');
      btn.innerHTML =
        '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" aria-hidden="true">' +
        '<circle cx="12" cy="12" r="9"/><path d="M3 12h18M12 3a15 15 0 0 1 0 18a15 15 0 0 1 0-18z"/></svg>' +
        '<span class="pci-langcode"></span><span class="pci-langcaret" aria-hidden="true">▾</span>';
      btn.querySelector('.pci-langcode').textContent = cur.toUpperCase();
      var menu = document.createElement('div');
      menu.className = 'pci-langmenu';
      menu.setAttribute('role', 'menu');
      menu.hidden = true;
      for (var i = 0; i < LANGS.length; i++) {
        (function (code, native) {
          var a = document.createElement('a');
          a.href = urlFor(code);
          a.setAttribute('role', 'menuitem');
          a.lang = code;
          a.textContent = native;
          if (code === cur) a.setAttribute('aria-current', 'true');
          a.addEventListener('click', function () {
            try { localStorage.setItem('pci_lang', code); } catch (e) { /* private mode */ }
          });
          menu.appendChild(a);
        })(LANGS[i][0], LANGS[i][1]);
      }
      btn.addEventListener('click', function (e) {
        e.stopPropagation();
        var open = menu.hidden;
        menu.hidden = !open;
        btn.setAttribute('aria-expanded', open ? 'true' : 'false');
      });
      document.addEventListener('click', function () {
        if (!menu.hidden) { menu.hidden = true; btn.setAttribute('aria-expanded', 'false'); }
      });
      document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && !menu.hidden) { menu.hidden = true; btn.setAttribute('aria-expanded', 'false'); btn.focus(); }
      });
      wrap.appendChild(btn);
      wrap.appendChild(menu);
      var promo = document.querySelector('.promo');
      if (promo) { promo.classList.add('has-langbtn'); promo.appendChild(wrap); }
      else { wrap.classList.add('pci-langfixed'); document.body.appendChild(wrap); }
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', build);
    else build();
  } catch (e) { /* no-op: page stays fully usable */ }
})();
/* ===== /PCI-LANGBAR ===== */

/* ===== PCI-SOCIAL: footer social-media icons (admin-controlled) =====
   Fetches GET /api/social once (cached in sessionStorage for 10 minutes) and injects monochrome
   inline-SVG icons into the footer's bottom row on every page. The endpoint returns only
   enabled + non-empty links (Admin Console -> Social media); nothing renders when disabled/empty. */
(function () {
  'use strict';
  try {
    var CACHE_KEY = 'pci_social_v1', TTL = 10 * 60 * 1000;
    var ICONS = {
      linkedin: ['LinkedIn', '<path d="M4.98 3.5a2.49 2.49 0 1 1-.02 4.98 2.49 2.49 0 0 1 .02-4.98zM3 9h4v12H3zM9 9h3.8v1.7h.06c.53-1 1.83-2.06 3.77-2.06 4.03 0 4.77 2.65 4.77 6.1V21h-4v-5.6c0-1.34-.02-3.06-1.86-3.06-1.87 0-2.16 1.46-2.16 2.96V21H9z"/>'],
      x: ['X (Twitter)', '<path d="M18.24 2.25h3.31l-7.23 8.26L22.83 21.75h-6.66l-5.22-6.82-5.97 6.82H1.66l7.73-8.83L1.25 2.25h6.83l4.71 6.23zm-1.16 17.52h1.83L7.08 4.13H5.12z"/>'],
      facebook: ['Facebook', '<path d="M13.5 21v-8h2.7l.4-3.1h-3.1V7.9c0-.9.25-1.5 1.54-1.5h1.65V3.6c-.29-.04-1.27-.12-2.4-.12-2.38 0-4 1.45-4 4.1v2.3H7.6V13h2.68v8z"/>'],
      instagram: ['Instagram', '<path d="M12 2.2c3.19 0 3.57.01 4.83.07 1.17.05 1.8.25 2.22.41.56.22.96.48 1.38.9.42.42.68.82.9 1.38.16.42.36 1.05.41 2.22.06 1.26.07 1.64.07 4.83s-.01 3.57-.07 4.83c-.05 1.17-.25 1.8-.41 2.22-.22.56-.48.96-.9 1.38-.42.42-.82.68-1.38.9-.42.16-1.05.36-2.22.41-1.26.06-1.64.07-4.83.07s-3.57-.01-4.83-.07c-1.17-.05-1.8-.25-2.22-.41a3.72 3.72 0 0 1-1.38-.9 3.72 3.72 0 0 1-.9-1.38c-.16-.42-.36-1.05-.41-2.22C2.21 15.57 2.2 15.19 2.2 12s.01-3.57.07-4.83c.05-1.17.25-1.8.41-2.22.22-.56.48-.96.9-1.38.42-.42.82-.68 1.38-.9.42-.16 1.05-.36 2.22-.41C8.43 2.21 8.81 2.2 12 2.2zm0 1.8c-3.13 0-3.5.01-4.74.07-1.08.05-1.67.23-2.06.38-.52.2-.89.44-1.28.83-.39.39-.63.76-.83 1.28-.15.39-.33.98-.38 2.06-.06 1.24-.07 1.61-.07 4.74s.01 3.5.07 4.74c.05 1.08.23 1.67.38 2.06.2.52.44.89.83 1.28.39.39.76.63 1.28.83.39.15.98.33 2.06.38 1.24.06 1.61.07 4.74.07s3.5-.01 4.74-.07c1.08-.05 1.67-.23 2.06-.38.52-.2.89-.44 1.28-.83.39-.39.63-.76.83-1.28.15-.39.33-.98.38-2.06.06-1.24.07-1.61.07-4.74s-.01-3.5-.07-4.74c-.05-1.08-.23-1.67-.38-2.06-.2-.52-.44-.89-.83-1.28a3.44 3.44 0 0 0-1.28-.83c-.39-.15-.98-.33-2.06-.38-1.24-.06-1.61-.07-4.74-.07zm0 3.06a4.94 4.94 0 1 1 0 9.88 4.94 4.94 0 0 1 0-9.88zm0 1.8a3.14 3.14 0 1 0 0 6.28 3.14 3.14 0 0 0 0-6.28zm5.14-3.2a1.15 1.15 0 1 1 0 2.3 1.15 1.15 0 0 1 0-2.3z"/>'],
      youtube: ['YouTube', '<path d="M21.58 7.19a2.5 2.5 0 0 0-1.76-1.77C18.25 5 12 5 12 5s-6.25 0-7.82.42A2.5 2.5 0 0 0 2.42 7.2C2 8.76 2 12 2 12s0 3.24.42 4.81c.23.86.9 1.53 1.76 1.76C5.75 19 12 19 12 19s6.25 0 7.82-.42a2.5 2.5 0 0 0 1.76-1.77C22 15.24 22 12 22 12s0-3.24-.42-4.81zM10 15.2V8.8L15.5 12z"/>'],
      whatsapp: ['WhatsApp', '<path d="M12.04 2a9.9 9.9 0 0 0-8.42 15.16L2.05 22l4.98-1.54A9.9 9.9 0 1 0 12.04 2zm0 1.67a8.23 8.23 0 1 1-4.2 15.3l-.3-.18-2.95.91.93-2.87-.2-.31a8.23 8.23 0 0 1 6.72-12.85zm-3.1 3.6c-.19 0-.5.07-.76.36-.26.28-1 .97-1 2.38 0 1.4 1.02 2.76 1.17 2.95.14.19 1.97 3.15 4.86 4.29 2.4.95 2.89.76 3.41.71.52-.05 1.68-.68 1.92-1.35.24-.66.24-1.23.17-1.35-.07-.12-.26-.19-.55-.33-.28-.14-1.68-.83-1.94-.92-.26-.1-.45-.14-.64.14-.19.28-.74.92-.9 1.11-.17.19-.34.21-.62.07a7.85 7.85 0 0 1-2.29-1.41 8.62 8.62 0 0 1-1.58-1.97c-.17-.28-.02-.44.12-.58.13-.13.29-.33.43-.5.14-.16.19-.28.28-.47.1-.19.05-.35-.02-.5-.07-.14-.62-1.54-.88-2.1-.23-.5-.47-.5-.65-.51z"/>']
    };
    function render(links) {
      if (!links || document.querySelector('.pci-social')) return;
      var keys = ['linkedin', 'x', 'facebook', 'instagram', 'youtube', 'whatsapp'];
      var row = document.querySelector('footer .ft-bottom');
      if (!row) return;
      var box = document.createElement('span');
      box.className = 'pci-social';
      var any = false;
      for (var i = 0; i < keys.length; i++) {
        var k = keys[i], url = links[k];
        if (!url || !/^https?:\/\//i.test(url) || !ICONS[k]) continue;
        var a = document.createElement('a');
        a.href = url;
        a.target = '_blank';
        a.rel = 'noopener noreferrer';
        a.setAttribute('aria-label', ICONS[k][0]);
        a.innerHTML = '<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true">' + ICONS[k][1] + '</svg>';
        box.appendChild(a);
        any = true;
      }
      if (any) row.appendChild(box);
    }
    function load() {
      try {
        var c = JSON.parse(sessionStorage.getItem(CACHE_KEY) || 'null');
        if (c && c.t && (Date.now() - c.t) < TTL) { render(c.links); return; }
      } catch (e) { /* bad cache: refetch */ }
      var base = (window.PCI_API_BASE || '').replace(/\/$/, '');
      fetch(base + '/api/social').then(function (r) { return r.json(); }).then(function (d) {
        var links = (d && d.links) || {};
        try { sessionStorage.setItem(CACHE_KEY, JSON.stringify({ t: Date.now(), links: links })); } catch (e) { /* full/private */ }
        render(links);
      }).catch(function () { /* backend unreachable: footer stays as-is */ });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', load);
    else load();
  } catch (e) { /* no-op */ }
})();
/* ===== /PCI-SOCIAL ===== */

/* ===== PCI-CHAT-INJECT: load the site chat widget on public pages =====
   Injects <script src="assets/chat.js" defer> exactly once, on every public page EXCEPT the
   admin console, exam UI and student portal (which have their own tooling and shouldn't show a
   visitor chat bubble). Guarded against double-injection so it is safe to include more than once. */
(function () {
  'use strict';
  try {
    var path = (location.pathname || '').toLowerCase();
    if (path.indexOf('admin') !== -1 || path.indexOf('exam-ui') !== -1 || path.indexOf('student') !== -1) return;
    if (window.__pciChatLoaded || document.querySelector('script[data-pci-chat]')) return;
    var s = document.createElement('script');
    s.src = 'assets/chat.js';
    s.defer = true;
    s.setAttribute('data-pci-chat', '1');
    document.head.appendChild(s);
  } catch (e) { /* no-op */ }
})();
/* ===== /PCI-CHAT-INJECT ===== */

/* ===== PCI-ANNOUNCE: site-wide launch announcement modal =====
   A premium, dismissible announcement shown once per visit (sessionStorage) on public pages only.
   Self-contained: injects its own scoped styles (all classes prefixed `pci-anx-`) and markup, so it
   works on every static page without touching any HTML. Skips the admin console, exam UI and student
   portal. Accessible: role=dialog, focus trap, Esc + backdrop + button dismissal, restores focus,
   honours prefers-reduced-motion. Bump ANX_KEY to re-show a new announcement to returning visitors. */
(function () {
  'use strict';
  try {
    var path = (location.pathname || '').toLowerCase();
    if (path.indexOf('admin') !== -1 || path.indexOf('exam-ui') !== -1 || path.indexOf('/app') === 0 ||
        path.indexOf('student') !== -1 || path.indexOf('launcher') !== -1) return;

    var ANX_KEY = 'pci.anx.dismissed.2026-09-15';   // bump to re-announce
    try { if (sessionStorage.getItem(ANX_KEY) === '1') return; } catch (e) { /* private mode: still show */ }
    if (window.__pciAnnounceLoaded || document.getElementById('pciAnx')) return;
    window.__pciAnnounceLoaded = true;

    var reduce = false;
    try { reduce = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches; } catch (e) {}

    var css = ''
      + '#pciAnx{position:fixed;inset:0;z-index:2147483600;display:flex;align-items:center;justify-content:center;'
      + 'padding:24px;opacity:0;transition:opacity .45s ease;}'
      + '#pciAnx.pci-anx-in{opacity:1;}'
      + '#pciAnx .pci-anx-back{position:absolute;inset:0;background:rgba(9,14,28,.62);backdrop-filter:blur(6px) saturate(140%);-webkit-backdrop-filter:blur(6px) saturate(140%);}'
      + '#pciAnx .pci-anx-card{position:relative;width:100%;max-width:544px;max-height:calc(100vh - 48px);max-height:calc(100dvh - 48px);'
      + 'overflow-y:auto;background:linear-gradient(163deg,#111C34 0%,#0E1525 62%,#0B1120 100%);'
      + 'color:#EEF2F9;border:1px solid rgba(201,162,75,.34);box-shadow:0 40px 120px -30px rgba(0,0,0,.75),0 0 0 1px rgba(255,255,255,.03) inset;'
      + 'padding:44px 44px 38px;transform:translateY(16px) scale(.985);transition:transform .5s cubic-bezier(.16,.84,.44,1);}'
      + '#pciAnx .pci-anx-card{scrollbar-width:thin;scrollbar-color:rgba(201,162,75,.5) transparent;}'
      + '#pciAnx .pci-anx-card::-webkit-scrollbar{width:8px;}'
      + '#pciAnx .pci-anx-card::-webkit-scrollbar-thumb{background:rgba(201,162,75,.4);border-radius:8px;}'
      + '#pciAnx.pci-anx-in .pci-anx-card{transform:none;}'
      + '#pciAnx .pci-anx-card::before{content:"";position:absolute;top:0;left:0;right:0;height:3px;'
      + 'background:linear-gradient(90deg,transparent,#C9A24B 22%,#E7CE92 50%,#C9A24B 78%,transparent);}'
      + '#pciAnx .pci-anx-glow{position:absolute;top:-140px;right:-120px;width:320px;height:320px;pointer-events:none;'
      + 'background:radial-gradient(circle,rgba(59,130,246,.22),transparent 66%);}'
      + '#pciAnx .pci-anx-x{position:absolute;top:14px;right:14px;width:40px;height:40px;display:flex;align-items:center;justify-content:center;'
      + 'background:transparent;border:0;border-radius:50%;color:#9FB0C9;cursor:pointer;transition:background .15s,color .15s;z-index:2;}'
      + '#pciAnx .pci-anx-x:hover{background:rgba(255,255,255,.08);color:#fff;}'
      + '#pciAnx .pci-anx-x svg{width:19px;height:19px;}'
      + '#pciAnx .pci-anx-eyebrow{display:inline-flex;align-items:center;gap:9px;font-family:\'Inter\',system-ui,sans-serif;'
      + 'font-weight:600;font-size:11.5px;letter-spacing:.19em;text-transform:uppercase;color:#D9BE7E;margin:0 0 20px;}'
      + '#pciAnx .pci-anx-eyebrow .pci-anx-pip{width:6px;height:6px;background:#C9A24B;border-radius:50%;box-shadow:0 0 0 4px rgba(201,162,75,.16);}'
      + '#pciAnx .pci-anx-h{font-family:\'Archivo\',system-ui,sans-serif;font-weight:800;letter-spacing:-.022em;line-height:1.06;'
      + 'font-size:clamp(25px,4.4vw,32px);margin:0 0 6px;color:#fff;}'
      + '#pciAnx .pci-anx-date{font-family:\'Archivo\',system-ui,sans-serif;font-weight:800;letter-spacing:-.01em;'
      + 'font-size:clamp(15px,2.6vw,17px);color:#E7CE92;margin:0 0 22px;}'
      + '#pciAnx .pci-anx-rule{width:54px;height:2px;background:linear-gradient(90deg,#C9A24B,rgba(201,162,75,0));margin:0 0 22px;}'
      + '#pciAnx .pci-anx-body{font-family:\'Inter\',system-ui,sans-serif;font-size:15.5px;line-height:1.66;color:#C7D2E4;margin:0 0 16px;}'
      + '#pciAnx .pci-anx-body strong{color:#fff;font-weight:600;}'
      + '#pciAnx .pci-anx-list{list-style:none;margin:0 0 20px;padding:0;display:flex;flex-direction:column;gap:12px;}'
      + '#pciAnx .pci-anx-li{display:flex;gap:12px;font-family:\'Inter\',system-ui,sans-serif;font-size:14px;line-height:1.58;color:#C7D2E4;}'
      + '#pciAnx .pci-anx-li strong{color:#fff;font-weight:600;}'
      + '#pciAnx .pci-anx-mk{flex:0 0 auto;width:8px;height:8px;margin-top:7px;transform:rotate(45deg);'
      + 'background:linear-gradient(135deg,#E7CE92,#C9A24B);box-shadow:0 0 0 4px rgba(201,162,75,.12);}'
      + '#pciAnx .pci-anx-note{font-family:\'Inter\',system-ui,sans-serif;font-size:12.5px;line-height:1.6;color:#8DA0BD;'
      + 'border-left:2px solid rgba(201,162,75,.5);padding:2px 0 2px 14px;margin:0 0 30px;}'
      + '#pciAnx .pci-anx-cta{display:flex;flex-wrap:wrap;gap:12px;align-items:center;}'
      + '#pciAnx .pci-anx-primary{display:inline-flex;align-items:center;gap:10px;font-family:\'Inter\',system-ui,sans-serif;'
      + 'font-weight:600;font-size:15px;padding:15px 26px;background:linear-gradient(180deg,#2563EB,#1D4ED8);color:#fff;'
      + 'text-decoration:none;border:1px solid rgba(255,255,255,.14);transition:transform .15s,box-shadow .15s,background .15s;'
      + 'box-shadow:0 12px 30px -12px rgba(37,99,235,.7);}'
      + '#pciAnx .pci-anx-primary:hover{background:linear-gradient(180deg,#1D4ED8,#1E40AF);transform:translateY(-1px);box-shadow:0 16px 36px -12px rgba(37,99,235,.85);}'
      + '#pciAnx .pci-anx-primary svg{width:17px;height:17px;transition:transform .18s;}'
      + '#pciAnx .pci-anx-primary:hover svg{transform:translateX(3px);}'
      + '#pciAnx .pci-anx-secondary{font-family:\'Inter\',system-ui,sans-serif;font-weight:600;font-size:14.5px;color:#9FB0C9;'
      + 'background:transparent;border:0;cursor:pointer;padding:12px 6px;transition:color .15s;}'
      + '#pciAnx .pci-anx-secondary:hover{color:#fff;}'
      + '@media (max-width:560px){#pciAnx .pci-anx-card{padding:40px 26px 30px;}#pciAnx .pci-anx-cta{gap:6px;}'
      + '#pciAnx .pci-anx-primary{width:100%;justify-content:center;}#pciAnx .pci-anx-secondary{width:100%;padding:14px 6px;}}'
      + '@media (prefers-reduced-motion: reduce){#pciAnx,#pciAnx .pci-anx-card{transition:none!important;}}';

    var style = document.createElement('style');
    style.setAttribute('data-pci-anx', '1');
    style.textContent = css;
    document.head.appendChild(style);

    var xIcon = '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"><path d="M6 6l12 12M18 6L6 18"/></svg>';
    var arrow = '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.3" stroke-linecap="round" stroke-linejoin="round"><path d="M5 12h14M13 6l6 6-6 6"/></svg>';

    var wrap = document.createElement('div');
    wrap.id = 'pciAnx';
    wrap.setAttribute('role', 'dialog');
    wrap.setAttribute('aria-modal', 'true');
    wrap.setAttribute('aria-labelledby', 'pciAnxTitle');
    wrap.setAttribute('aria-describedby', 'pciAnxBody');
    wrap.innerHTML = ''
      + '<div class="pci-anx-back" data-anx-close></div>'
      + '<div class="pci-anx-card">'
      +   '<div class="pci-anx-glow"></div>'
      +   '<button class="pci-anx-x" type="button" aria-label="Dismiss announcement" data-anx-close>' + xIcon + '</button>'
      +   '<p class="pci-anx-eyebrow"><span class="pci-anx-pip"></span>Project Controls Institute · Announcement</p>'
      +   '<h2 class="pci-anx-h" id="pciAnxTitle">Certifications open for examination on 15 September 2026</h2>'
      +   '<div class="pci-anx-date">Applications are open now — Honorary Fellowship only</div>'
      +   '<div class="pci-anx-rule"></div>'
      +   '<p class="pci-anx-body" id="pciAnxBody">The <strong>PCI AI Project Leadership Certification Suite</strong> — PCI PCL-AI™, '
      +     'PFL-AI™ and PDL-AI™ — is scheduled to open for enrolment and examination on <strong>15 September 2026</strong>. '
      +     'Ahead of that date, the Institute is accepting applications for <strong>Honorary Fellow (PCI)</strong> '
      +     'recognition only.</p>'
      +   '<ul class="pci-anx-list">'
      +     '<li class="pci-anx-li"><span class="pci-anx-mk"></span><span><strong>Review times.</strong> Owing to the current '
      +       'volume of applications, a decision may take approximately <strong>15 to 30 days</strong> from the date your '
      +       'complete application is received.</span></li>'
      +     '<li class="pci-anx-li"><span class="pci-anx-mk"></span><span><strong>If your application is successful.</strong> '
      +       'Accepted applicants are issued personal access credentials to the PCI student portal, together with access to '
      +       'the Institute&rsquo;s learning resources and study materials.</span></li>'
      +     '<li class="pci-anx-li"><span class="pci-anx-mk"></span><span><strong>No fees.</strong> The Institute does not '
      +       'charge any fee to apply, and its study materials are provided at no cost.</span></li>'
      +   '</ul>'
      +   '<p class="pci-anx-note">Honorary Fellow (PCI) is a board-conferred recognition of distinguished contribution to the '
      +     'profession. It involves no examination, is awarded at the Institute&rsquo;s discretion on the merits of each '
      +     'application, and is not an examined PCI certification. Dates, eligibility and benefits may change and do not form '
      +     'a contract or guarantee.</p>'
      +   '<div class="pci-anx-cta">'
      +     '<a class="pci-anx-primary" href="honorary-application.html">Apply for Honorary Fellow (PCI) ' + arrow + '</a>'
      +     '<button class="pci-anx-secondary" type="button" data-anx-close>Continue browsing</button>'
      +   '</div>'
      + '</div>';

    var lastFocus = document.activeElement;

    function focusables() {
      return wrap.querySelectorAll('a[href],button:not([disabled])');
    }
    function onKey(e) {
      if (e.key === 'Escape') { close(); return; }
      if (e.key !== 'Tab') return;
      var f = focusables(); if (!f.length) return;
      var first = f[0], last = f[f.length - 1];
      if (e.shiftKey && document.activeElement === first) { e.preventDefault(); last.focus(); }
      else if (!e.shiftKey && document.activeElement === last) { e.preventDefault(); first.focus(); }
    }
    function close() {
      try { sessionStorage.setItem(ANX_KEY, '1'); } catch (e) {}
      document.removeEventListener('keydown', onKey, true);
      wrap.classList.remove('pci-anx-in');
      var done = function () { if (wrap.parentNode) wrap.parentNode.removeChild(wrap); document.documentElement.style.overflow = ''; };
      if (reduce) done(); else setTimeout(done, 460);
      try { if (lastFocus && lastFocus.focus) lastFocus.focus(); } catch (e) {}
    }
    function open() {
      document.body.appendChild(wrap);
      document.documentElement.style.overflow = 'hidden';
      var closers = wrap.querySelectorAll('[data-anx-close]');
      for (var i = 0; i < closers.length; i++) closers[i].addEventListener('click', close);
      document.addEventListener('keydown', onKey, true);
      var reveal = function () {
        wrap.classList.add('pci-anx-in');
        var x = wrap.querySelector('.pci-anx-x'); if (x) x.focus();
      };
      if (reduce) reveal(); else requestAnimationFrame(function () { setTimeout(reveal, 30); });
    }

    var start = function () { setTimeout(open, reduce ? 0 : 650); };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', start);
    else start();
  } catch (e) { /* no-op: an announcement must never break the page */ }
})();
/* ===== /PCI-ANNOUNCE ===== */
