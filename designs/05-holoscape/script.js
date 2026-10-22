/* FUTR Holoscape — tiny vanilla helpers: nav toggle, reveal, tilt, filters, show-more */
(function () {
  'use strict';

  var reduce = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;

  /* ---- mobile nav ---- */
  var toggle = document.querySelector('.nav-toggle');
  var nav = document.querySelector('.nav');
  if (toggle && nav) {
    toggle.addEventListener('click', function () {
      var open = nav.classList.toggle('open');
      toggle.setAttribute('aria-expanded', open ? 'true' : 'false');
      toggle.textContent = open ? 'Close' : 'Menu';
    });
  }

  /* ---- reveal on scroll ---- */
  var revealEls = document.querySelectorAll('.reveal, .compare');
  if ('IntersectionObserver' in window && !reduce) {
    var io = new IntersectionObserver(function (entries) {
      entries.forEach(function (e) {
        if (e.isIntersecting) { e.target.classList.add('in'); io.unobserve(e.target); }
      });
    }, { rootMargin: '0px 0px -8% 0px', threshold: 0.05 });
    revealEls.forEach(function (el) { io.observe(el); });
  } else {
    revealEls.forEach(function (el) { el.classList.add('in'); });
  }

  /* ---- parallax tilt on tiles (pointer devices only) ---- */
  var fine = window.matchMedia && window.matchMedia('(hover: hover) and (pointer: fine)').matches;
  if (fine && !reduce) {
    document.querySelectorAll('.tile').forEach(function (tile) {
      tile.addEventListener('pointermove', function (ev) {
        var r = tile.getBoundingClientRect();
        var px = (ev.clientX - r.left) / r.width;
        var py = (ev.clientY - r.top) / r.height;
        tile.classList.add('is-tilting');
        tile.style.setProperty('--ry', ((px - 0.5) * 10).toFixed(2) + 'deg');
        tile.style.setProperty('--rx', ((0.5 - py) * 8).toFixed(2) + 'deg');
        tile.style.setProperty('--mx', (px * 100).toFixed(1) + '%');
        tile.style.setProperty('--my', (py * 100).toFixed(1) + '%');
      });
      tile.addEventListener('pointerleave', function () {
        tile.classList.remove('is-tilting');
        tile.style.setProperty('--rx', '0deg');
        tile.style.setProperty('--ry', '0deg');
        tile.style.setProperty('--mx', '50%');
        tile.style.setProperty('--my', '50%');
      });
    });
  }

  /* ---- image fallback ---- */
  document.querySelectorAll('img[data-fallback]').forEach(function (img) {
    img.addEventListener('error', function () {
      if (img.dataset.failed) return;
      img.dataset.failed = '1';
      img.src = img.getAttribute('data-fallback');
    });
  });

  /* ---- generic text filter: data-filter-input targets data-filter-list ---- */
  document.querySelectorAll('[data-filter-input]').forEach(function (input) {
    var list = document.querySelector(input.getAttribute('data-filter-input'));
    var counter = document.querySelector(input.getAttribute('data-filter-count') || '__none__');
    if (!list) return;
    var items = Array.prototype.slice.call(list.querySelectorAll('[data-name]'));
    input.addEventListener('input', function () {
      var q = input.value.trim().toLowerCase();
      var shown = 0;
      items.forEach(function (it) {
        var hit = !q || it.getAttribute('data-name').toLowerCase().indexOf(q) !== -1;
        it.style.display = hit ? '' : 'none';
        if (hit) shown++;
      });
      if (counter) counter.textContent = shown + ' of ' + items.length;
    });
  });

  /* ---- show more / show less ---- */
  document.querySelectorAll('[data-more-for]').forEach(function (btn) {
    var list = document.querySelector(btn.getAttribute('data-more-for'));
    if (!list) return;
    var hidden = list.querySelectorAll('.is-hidden');
    var n = hidden.length;
    if (!n) { btn.style.display = 'none'; return; }
    var labelMore = btn.getAttribute('data-label-more') || ('Show ' + n + ' more');
    var labelLess = btn.getAttribute('data-label-less') || 'Show fewer';
    btn.querySelector('span').textContent = labelMore;
    var open = false;
    btn.addEventListener('click', function () {
      open = !open;
      hidden.forEach(function (el) { el.classList.toggle('is-hidden', !open); });
      btn.querySelector('span').textContent = open ? labelLess : labelMore;
      btn.setAttribute('aria-expanded', open ? 'true' : 'false');
    });
  });

  /* ---- confidence filter on datapoints ---- */
  var fbtns = document.querySelectorAll('.filters .fbtn');
  if (fbtns.length) {
    var dps = document.querySelectorAll('.dp');
    fbtns.forEach(function (b) {
      b.addEventListener('click', function () {
        fbtns.forEach(function (x) { x.setAttribute('aria-pressed', 'false'); });
        b.setAttribute('aria-pressed', 'true');
        var want = b.getAttribute('data-conf');
        dps.forEach(function (dp) {
          var ok = want === 'all' || dp.getAttribute('data-conf') === want;
          dp.classList.toggle('is-hidden', !ok);
        });
      });
    });
  }
})();
