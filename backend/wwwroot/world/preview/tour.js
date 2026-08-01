/* PCI World platform tour — shared behaviour. Everything here is additive: with scripting off the
   pages are complete documents, the demo buttons simply do nothing, and no content is hidden.
   Nothing is stored, sent or checked — each handler writes the sentence the REAL server answers
   with, so the tour demonstrates the product's refusals without being able to enforce them. */
(function () {
  'use strict';

  /* Sticky header hairline once the page has moved (same behaviour as the live World pages). */
  var h = document.querySelector('header.world');
  if (h) {
    var stick = function () { h.classList.toggle('is-stuck', window.scrollY > 4); };
    addEventListener('scroll', stick, { passive: true });
    stick();
  }

  /* Entrance choreography, gated exactly like assets/premium.js: only when IntersectionObserver
     exists and the visitor has not asked for reduced motion. html.tv arms the CSS; .tv-in reveals. */
  if ('IntersectionObserver' in window &&
      !matchMedia('(prefers-reduced-motion: reduce)').matches) {
    document.documentElement.classList.add('tv');
    var io = new IntersectionObserver(function (entries) {
      entries.forEach(function (e) {
        if (e.isIntersecting) { e.target.classList.add('tv-in'); io.unobserve(e.target); }
      });
    }, { rootMargin: '0px 0px -8% 0px' });
    document.querySelectorAll('.tv-r').forEach(function (el) { io.observe(el); });
  }

  /* Demo verdicts. One delegated listener; each button names its demo and its output slot.
     The sentences are the product's own words — the same copy the real endpoints answer with. */
  var out = function (id, html) {
    var el = document.getElementById(id);
    if (el) el.innerHTML = html;
  };
  var notice = function (kind, text) {
    return '<div class="notice ' + kind + '" role="status">' + text + '</div>';
  };

  var demos = {
    /* Forum composer: a new account's thread is held before publication. */
    hold: function () {
      out('holdout',
        notice('held', 'Your thread is waiting for a moderator. New accounts are reviewed before publication.') +
        '<p class="help" style="margin-top:10px">The draft is cleared because the server is now holding the text — nothing is lost, and keeping a copy here invites a duplicate.</p>');
      var t = document.getElementById('t'), b = document.getElementById('b');
      if (t) t.value = '';
      if (b) b.value = '';
    },

    /* Job application: no consent, nothing to send. */
    apply: function () {
      var c = document.getElementById('cons');
      out('applyout', c && c.checked
        ? notice('live', 'Your application has been sent. What you wrote, the CV you attached and the consent you gave are recorded as they were at this moment and will not change.')
        : notice('stop', 'Tick the consent box to apply. Without it there is nothing to send — the employer only ever receives what you have agreed to disclose.'));
    },

    /* Employer portal: an unverified organisation can draft everything and publish nothing. */
    nopub: function () {
      out('nopubout', notice('stop', 'Only a verified organisation can publish a job. Your draft is saved and stays private until verification completes.'));
    },

    /* Employer portal: closing a job removes the claim, keeps the page. */
    close: function () {
      out('closeout', notice('held', 'Closed. The page stays readable for anyone holding the link, but it no longer tells search engines the job is open and it has left the sitemap.'));
    },

    /* My applications: withdrawal is not erasure — and says so. */
    wd: function () {
      out('wdout', notice('held', 'Consent withdrawn. The employer can no longer open this application or your CV. The record that you applied, and that you consented at the time, is kept — withdrawing consent is not the same as erasure.'));
    },

    /* Contributor desk: submission goes to a person, never straight to the site. */
    sub: function () {
      out('subout', notice('held', 'Submitted for editorial review. Every contributor article is read by an editor before publication; nothing you submit appears on the site until a person accepts it.'));
    },

    /* Editor's queue: the maker-checker refusal — the reviewer here is also the author. */
    mc: function () {
      out('mcout',
        notice('stop', 'You cannot approve your own submission. This manuscript was written by the PCI World account linked to your editor account, so it needs a second editor.') +
        '<p class="help" style="margin-top:10px">In the real system this refusal spans two separate account tables. Comparing the ids directly would never match — which is exactly how a control like this ends up looking correct while doing nothing.</p>');
    }
  };

  document.addEventListener('click', function (e) {
    var b = e.target.closest('[data-demo]');
    if (!b) return;
    e.preventDefault();
    var run = demos[b.getAttribute('data-demo')];
    if (run) run();
  });
})();
