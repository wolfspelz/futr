/* FUTR Flight Deck — small vanilla helpers: boot animation, counters, dial ticks, filters. */
(function () {
  var root = document.documentElement;
  root.classList.add('boot');

  function onReady(fn) {
    if (document.readyState !== 'loading') fn(); else document.addEventListener('DOMContentLoaded', fn);
  }

  onReady(function () {
    /* dial ticks: generated from data attributes so every gauge gets a consistent scale */
    document.querySelectorAll('svg.dial[data-min]').forEach(function (svg) {
      var min = parseFloat(svg.dataset.min), max = parseFloat(svg.dataset.max);
      var step = parseFloat(svg.dataset.step || '1');
      var g = svg.querySelector('.ticks');
      if (!g || !(max > min)) return;
      var ns = 'http://www.w3.org/2000/svg';
      for (var v = min; v <= max + 1e-9; v += step) {
        var major = g.dataset.major ? g.dataset.major.split(',').indexOf(String(Math.round(v * 100) / 100)) > -1 : false;
        if (major) continue; // majors are inline
        var a = -135 + 270 * (v - min) / (max - min);
        var l = document.createElementNS(ns, 'line');
        l.setAttribute('x1', 100); l.setAttribute('y1', 14); l.setAttribute('x2', 100); l.setAttribute('y2', 9);
        l.setAttribute('transform', 'rotate(' + a.toFixed(2) + ' 100 100)');
        l.setAttribute('class', 'tick minor');
        g.appendChild(l);
      }
    });

    /* telemetry counters */
    var counters = document.querySelectorAll('[data-count]');
    counters.forEach(function (el) {
      var target = parseInt(el.dataset.count, 10);
      var t0 = null, dur = 1100;
      function tick(ts) {
        if (t0 === null) t0 = ts;
        var p = Math.min(1, (ts - t0) / dur);
        var e = 1 - Math.pow(1 - p, 3);
        el.textContent = Math.round(target * e);
        if (p < 1) requestAnimationFrame(tick); else el.textContent = target;
      }
      el.textContent = '0';
      requestAnimationFrame(tick);
    });

    /* release boot state -> needles sweep, bars fill */
    requestAnimationFrame(function () { requestAnimationFrame(function () { root.classList.remove('boot'); }); });

    /* text filters: input[data-filter="#selector"] hides non-matching children */
    document.querySelectorAll('input[data-filter]').forEach(function (inp) {
      var items = document.querySelectorAll(inp.dataset.filter);
      var out = inp.dataset.count ? document.querySelector(inp.dataset.count) : null;
      inp.addEventListener('input', function () {
        var q = inp.value.trim().toLowerCase(), n = 0;
        items.forEach(function (it) {
          var hit = !q || it.textContent.toLowerCase().indexOf(q) > -1;
          it.classList.toggle('is-hidden', !hit);
          if (hit) n++;
        });
        if (out) out.textContent = n;
      });
    });

    /* show-more toggles */
    document.querySelectorAll('[data-toggle]').forEach(function (btn) {
      var target = document.querySelector(btn.dataset.toggle);
      if (!target) return;
      btn.addEventListener('click', function () {
        var open = target.classList.toggle('is-open');
        btn.textContent = open ? (btn.dataset.less || 'Show less') : (btn.dataset.more || 'Show all');
      });
    });

    /* image fallback */
    document.querySelectorAll('img[data-fallback]').forEach(function (img) {
      img.addEventListener('error', function () {
        if (img.dataset.failed) return;
        img.dataset.failed = '1';
        img.src = 'placeholder.svg';
      });
      if (img.complete && img.naturalWidth === 0 && img.src.indexOf('placeholder.svg') < 0) {
        img.dataset.failed = '1'; img.src = 'placeholder.svg';
      }
    });
  });
})();
