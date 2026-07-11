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
        }
      } catch (e) { /* static page remains fully usable */ }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', start);
    else start();
  } catch (e) { /* no-op */ }
})();
