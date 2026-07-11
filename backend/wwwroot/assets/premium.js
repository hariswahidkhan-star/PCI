/* PCI premium motion runtime — progressive enhancement only.
   Adds html.pv (motion welcome), reveals elements on scroll, elevates the
   header, and drives the scroll-progress bar. Without this file (or with
   reduced motion), the site is fully visible and static. */
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
            /* anything already in view (above the fold) shows immediately */
            if (r.top < window.innerHeight && r.bottom > 0) targets[t].classList.add('pv-in');
            else io.observe(targets[t]);
          }
        }

        /* header elevation + scroll progress, one rAF-throttled listener */
        var hdr = document.querySelector('.hdr');
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
            ticking = false;
          });
        };
        window.addEventListener('scroll', onScroll, { passive: true });
        onScroll();
      } catch (e) { /* static page remains fully usable */ }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', start);
    else start();
  } catch (e) { /* no-op */ }
})();
