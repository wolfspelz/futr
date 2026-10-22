/* FUTR Mission Control — vanilla JS: mission clock, rotary counters, filters, image fallback */
(function () {
  'use strict';

  /* ---- Mission clock (UTC + day-of-year + mission elapsed since 2023) ---- */
  function pad(n, w) { n = String(n); while (n.length < (w || 2)) n = '0' + n; return n; }
  function tickClock() {
    var el = document.getElementById('clock-time');
    if (!el) return;
    var now = new Date();
    el.textContent = pad(now.getUTCHours()) + ':' + pad(now.getUTCMinutes()) + ':' + pad(now.getUTCSeconds());
    var doy = document.getElementById('clock-doy');
    if (doy) {
      var start = Date.UTC(now.getUTCFullYear(), 0, 0);
      var d = Math.floor((now.getTime() - start) / 86400000);
      doy.textContent = 'DAY ' + pad(d, 3);
    }
    var met = document.getElementById('clock-met');
    if (met) {
      var t0 = Date.UTC(2023, 0, 1);
      var secs = Math.floor((now.getTime() - t0) / 1000);
      var days = Math.floor(secs / 86400);
      met.textContent = 'MET ' + days + 'd';
    }
  }
  tickClock();
  setInterval(tickClock, 1000);

  /* ---- Rotary counters: <span class="counter" data-value="222"></span> ---- */
  function buildCounter(el) {
    if (el.getAttribute('data-built')) return;
    el.setAttribute('data-built', '1');
    var raw = el.getAttribute('data-value') || '0';
    var digits = raw.split('');
    el.innerHTML = '';
    el.setAttribute('aria-label', raw);
    var reels = [];
    digits.forEach(function (ch) {
      if (ch === ',' || ch === '.') {
        var s = document.createElement('span');
        s.className = 'sepc';
        s.textContent = ch;
        el.appendChild(s);
        return;
      }
      var d = document.createElement('span');
      d.className = 'digit';
      d.setAttribute('aria-hidden', 'true');
      var reel = document.createElement('span');
      reel.className = 'reel';
      for (var i = 0; i <= 9; i++) {
        var n = document.createElement('span');
        n.textContent = String(i);
        reel.appendChild(n);
      }
      d.appendChild(reel);
      el.appendChild(d);
      reels.push({ reel: reel, target: parseInt(ch, 10) || 0 });
    });
    // animate after layout
    setTimeout(function () {
      reels.forEach(function (r, i) {
        r.reel.style.transitionDelay = (i * 0.08) + 's';
        r.reel.style.transform = 'translateY(' + (-r.target * 1.1) + 'em)';
      });
    }, 60);
  }
  var counters = document.querySelectorAll('.counter[data-value]');
  if ('IntersectionObserver' in window) {
    var io = new IntersectionObserver(function (entries) {
      entries.forEach(function (e) {
        if (e.isIntersecting) { buildCounter(e.target); io.unobserve(e.target); }
      });
    }, { threshold: 0.2 });
    counters.forEach(function (c) { io.observe(c); });
  } else {
    counters.forEach(buildCounter);
  }
  // Safety net: build everything after a short delay even if the observer never fires
  setTimeout(function () { counters.forEach(buildCounter); }, 2500);

  /* ---- Image fallback: swap to local placeholder on error ---- */
  document.querySelectorAll('img[data-fallback]').forEach(function (img) {
    img.addEventListener('error', function () {
      if (img.getAttribute('data-failed')) return;
      img.setAttribute('data-failed', '1');
      img.src = img.getAttribute('data-fallback');
      img.classList.add('contain');
    });
    // cached failures
    if (img.complete && img.naturalWidth === 0 && img.src && !img.getAttribute('data-failed')) {
      img.dispatchEvent(new Event('error'));
    }
  });

  /* ---- Flight plan table filter ---- */
  var filter = document.getElementById('civ-filter');
  if (filter) {
    var rows = document.querySelectorAll('table.flight tbody tr');
    var count = document.getElementById('civ-count');
    filter.addEventListener('input', function () {
      var q = filter.value.trim().toLowerCase();
      var shown = 0;
      rows.forEach(function (r) {
        var hit = !q || r.textContent.toLowerCase().indexOf(q) !== -1;
        r.classList.toggle('hidden', !hit);
        if (hit) shown++;
      });
      if (count) count.textContent = shown + ' OF ' + rows.length + ' CIVILIZATIONS';
    });
  }

  /* ---- Readout confidence filter + expand all ---- */
  var tools = document.querySelector('.readout-tools');
  if (tools) {
    var readouts = document.querySelectorAll('.readout[data-conf]');
    tools.addEventListener('click', function (ev) {
      var b = ev.target.closest('button');
      if (!b) return;
      if (b.hasAttribute('data-expand')) {
        var open = b.getAttribute('data-expand') === 'open';
        document.querySelectorAll('.readout details').forEach(function (d) { d.open = open; });
        return;
      }
      var f = b.getAttribute('data-filter');
      if (f === null) return;
      tools.querySelectorAll('button[data-filter]').forEach(function (x) { x.classList.toggle('is-on', x === b); });
      readouts.forEach(function (r) {
        var show = f === 'all' || r.getAttribute('data-conf') === f ||
          (f === 'solid' && /^(Canon|SemiCanon|Calculated)$/.test(r.getAttribute('data-conf'))) ||
          (f === 'guess' && /Guess$/.test(r.getAttribute('data-conf')));
        r.classList.toggle('hidden', !show);
      });
    });
  }
})();
