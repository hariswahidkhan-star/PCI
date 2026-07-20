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
      // Visibility is driven by the `.open` class (the stylesheet reveals the menu with
      // `.pci-langmenu.open{display:block}`); keep the `hidden` attribute in sync for a11y.
      function setOpen(open) {
        menu.classList.toggle('open', open);
        menu.hidden = !open;
        btn.setAttribute('aria-expanded', open ? 'true' : 'false');
      }
      btn.addEventListener('click', function (e) {
        e.stopPropagation();
        setOpen(!menu.classList.contains('open'));
      });
      document.addEventListener('click', function () {
        if (menu.classList.contains('open')) setOpen(false);
      });
      document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && menu.classList.contains('open')) { setOpen(false); btn.focus(); }
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

/* ===== PCI-SOCIAL =====
   The footer social-media icons are now rendered SERVER-SIDE from the social_accounts table into the
   <!--PCI-SOCIAL--> marker (Core/SocialLinks.cs), so they are SEO-visible, need no JavaScript, and
   support every platform configured in Admin Console -> Social media. The old client-side injector
   (which fetched /api/social and hardcoded six icons) has been removed to avoid duplication and to
   keep the database the single source of truth. /api/social remains available for other consumers. */

/* ===== PCI-CHAT-INJECT: load the site chat widget on public pages =====
   Injects <script src="assets/chat.js" defer> exactly once, on every public page EXCEPT the
   admin console, exam UI and student portal (which have their own tooling and shouldn't show a
   visitor chat bubble). Guarded against double-injection so it is safe to include more than once. */
(function () {
  'use strict';
  try {
    // Exact-match the app surfaces (same rule as the announcement below): a substring test would
    // wrongly hide the visitor chat on public pages like membership-student.html.
    var path = (location.pathname || '').toLowerCase();
    var file = path.split('/').pop() || '';
    var APP_SHELLS = { 'admin.html': 1, 'admin-chat.html': 1, 'exam-ui.html': 1, 'index-launcher.html': 1,
      'student.html': 1, 'student-login.html': 1, 'student-registration.html': 1,
      'student-dashboard.html': 1, 'student-welcome.html': 1 };
    if (path.indexOf('/app') === 0 || path === '/admin' || path.indexOf('/admin/') === 0 || APP_SHELLS[file]) return;
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
    // Skip ONLY true application surfaces (portal, console, exam client, legacy portal shells).
    // Deliberately exact-match — a substring test like indexOf('student') would wrongly hide the
    // announcement on PUBLIC pages such as membership-student.html or examination-administration.html.
    var path = (location.pathname || '').toLowerCase();
    var file = path.split('/').pop() || '';
    var APP_SHELLS = { 'admin.html': 1, 'admin-chat.html': 1, 'exam-ui.html': 1, 'index-launcher.html': 1,
      'student.html': 1, 'student-login.html': 1, 'student-registration.html': 1,
      'student-dashboard.html': 1, 'student-welcome.html': 1 };
    if (path.indexOf('/app') === 0 || path === '/admin' || path.indexOf('/admin/') === 0 || APP_SHELLS[file]) return;
    if (window.PCI_NO_ANNOUNCE) return;   // pages can opt out (e.g. a focused, personal-link flow)
    if (window.__pciAnnounceLoaded || document.getElementById('pciAnx')) return;
    window.__pciAnnounceLoaded = true;

    var reduce = false;
    try { reduce = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches; } catch (e) {}

    // The whole announcement — whether it shows, the date and every line of copy — is admin-controlled.
    // Fetch the resolved config; render nothing if disabled or on any error. The dismissal key includes
    // the version + date so any admin change re-shows the notice to visitors who dismissed the old one.
    var base = (window.PCI_API_BASE || '').replace(/\/$/, '');
    fetch(base + '/api/announcement').then(function (r) { return r.json(); }).then(function (cfg) {
      if (!cfg || !cfg.enabled) return;
      // Prefer the server's language-stable key (English version+date) so dismissing the notice in one
      // language dismisses it in all; fall back to version+date for an older backend.
      var ANX_KEY = 'pci.anx.' + (cfg.key || ((cfg.version || '') + '.' + (cfg.date || '')));
      try { if (sessionStorage.getItem(ANX_KEY) === '1') return; } catch (e) { /* private mode: still show */ }
      buildAndShow(cfg, ANX_KEY);
    }).catch(function () { /* backend unreachable: no announcement, page unaffected */ });

    function buildAndShow(cfg, ANX_KEY) {
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

    var arrow = '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.3" stroke-linecap="round" stroke-linejoin="round"><path d="M5 12h14M13 6l6 6-6 6"/></svg>';

    // Build the card from the admin config. All copy is set via textContent (never innerHTML), so an
    // admin-entered string can never inject markup into the page.
    function el(tag, cls, text) { var e = document.createElement(tag); if (cls) e.className = cls; if (text != null) e.textContent = text; return e; }

    var wrap = document.createElement('div');
    wrap.id = 'pciAnx';
    wrap.setAttribute('role', 'dialog');
    wrap.setAttribute('aria-modal', 'true');
    wrap.setAttribute('aria-labelledby', 'pciAnxTitle');

    var back = el('div', 'pci-anx-back'); back.setAttribute('data-anx-close', '');
    var card = el('div', 'pci-anx-card');
    card.appendChild(el('div', 'pci-anx-glow'));

    var x = el('button', 'pci-anx-x'); x.type = 'button'; x.setAttribute('aria-label', cfg.dismiss || 'Dismiss announcement');
    x.setAttribute('data-anx-close', '');
    x.innerHTML = '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"><path d="M6 6l12 12M18 6L6 18"/></svg>';
    card.appendChild(x);

    if (cfg.eyebrow) {
      var eb = el('p', 'pci-anx-eyebrow'); eb.appendChild(el('span', 'pci-anx-pip'));
      eb.appendChild(document.createTextNode(cfg.eyebrow)); card.appendChild(eb);
    }
    if (cfg.title) { var h = el('h2', 'pci-anx-h', cfg.title); h.id = 'pciAnxTitle'; card.appendChild(h); }
    if (cfg.subtitle) card.appendChild(el('div', 'pci-anx-date', cfg.subtitle));
    card.appendChild(el('div', 'pci-anx-rule'));
    if (cfg.intro) card.appendChild(el('p', 'pci-anx-body', cfg.intro));

    if (cfg.points && cfg.points.length) {
      var ul = el('ul', 'pci-anx-list');
      for (var i = 0; i < cfg.points.length; i++) {
        var p = cfg.points[i] || {};
        var li = el('li', 'pci-anx-li'); li.appendChild(el('span', 'pci-anx-mk'));
        var span = el('span');
        if (p.title) { span.appendChild(el('strong', null, p.title)); span.appendChild(document.createTextNode(' ')); }
        if (p.text) span.appendChild(document.createTextNode(p.text));
        li.appendChild(span); ul.appendChild(li);
      }
      card.appendChild(ul);
    }
    if (cfg.note) card.appendChild(el('p', 'pci-anx-note', cfg.note));

    var cta = el('div', 'pci-anx-cta');
    if (cfg.cta && cfg.cta.href) {
      var a = el('a', 'pci-anx-primary'); a.href = cfg.cta.href;
      a.appendChild(document.createTextNode((cfg.cta.label || 'Learn more') + ' '));
      var sv = document.createElement('span'); sv.innerHTML = arrow; a.appendChild(sv);
      cta.appendChild(a);
    }
    var sec = el('button', 'pci-anx-secondary', cfg.dismiss || 'Continue browsing'); sec.type = 'button';
    sec.setAttribute('data-anx-close', ''); cta.appendChild(sec);
    card.appendChild(cta);

    wrap.appendChild(back); wrap.appendChild(card);

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
    }  /* end buildAndShow */
  } catch (e) { /* no-op: an announcement must never break the page */ }
})();
/* ===== /PCI-ANNOUNCE ===== */
