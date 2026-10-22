/* FUTR Starchart Atlas — small vanilla helpers */
(function () {
  'use strict';

  /* "and N more" expanders -------------------------------------------- */
  document.querySelectorAll('[data-expand]').forEach(function (btn) {
    var sel = btn.getAttribute('data-expand');
    btn.addEventListener('click', function () {
      document.querySelectorAll(sel).forEach(function (el) { el.classList.remove('hidden'); });
      var note = btn.parentElement.querySelector('.expand-note');
      if (note) note.textContent = btn.getAttribute('data-done') || '';
      btn.remove();
    });
  });

  /* text filters: <input data-filter="selector"> hides rows whose text does not match */
  document.querySelectorAll('input[data-filter]').forEach(function (input) {
    var sel = input.getAttribute('data-filter');
    var countEl = document.querySelector(input.getAttribute('data-count') || '__none');
    input.addEventListener('input', function () {
      var q = input.value.trim().toLowerCase();
      var shown = 0;
      document.querySelectorAll(sel).forEach(function (row) {
        var hit = !q || row.textContent.toLowerCase().indexOf(q) !== -1;
        row.classList.toggle('hidden', !hit);
        if (hit) shown++;
      });
      if (countEl) countEl.textContent = shown;
    });
  });

  /* confidence filter buttons on the civilization log ----------------- */
  var filterBar = document.querySelector('.filters');
  if (filterBar) {
    filterBar.addEventListener('click', function (e) {
      var b = e.target.closest('button'); if (!b) return;
      filterBar.querySelectorAll('button').forEach(function (x) { x.setAttribute('aria-pressed', x === b ? 'true' : 'false'); });
      var want = b.getAttribute('data-conf');
      document.querySelectorAll('.log .entry').forEach(function (en) {
        var lvl = +en.getAttribute('data-level');
        var show = want === 'all' || (want === 'canon' && lvl >= 5) || (want === 'calc' && (lvl === 4 || lvl === 2)) || (want === 'guess' && (lvl === 3 || lvl === 1));
        en.classList.toggle('hidden', !show);
      });
    });
  }

  /* Orbit range arcs ---------------------------------------------------
     <div class="orbit" data-value data-min data-max data-min-label data-max-label data-value-label>
     Draws a shallow orbital arc; the gold band is the 1-sigma range, the star is the value. */
  var NS = 'http://www.w3.org/2000/svg';
  function el(name, attrs) {
    var n = document.createElementNS(NS, name);
    for (var k in attrs) n.setAttribute(k, attrs[k]);
    return n;
  }
  document.querySelectorAll('.orbit[data-value]').forEach(function (box) {
    var v = parseFloat(box.getAttribute('data-value'));
    var hasRange = box.hasAttribute('data-min');
    var min = hasRange ? parseFloat(box.getAttribute('data-min')) : v;
    var max = hasRange ? parseFloat(box.getAttribute('data-max')) : v;
    var W = 320, H = 86;
    var svg = el('svg', { viewBox: '0 0 ' + W + ' ' + H, role: 'img' });
    var title = el('title', {});
    title.textContent = hasRange ? 'Range ' + min + ' to ' + max + ', value ' + v : 'Point value ' + v;
    svg.appendChild(title);
    var d = 'M12 60 Q160 10 308 60';
    var track = el('path', { d: d, 'class': 'track' });
    svg.appendChild(track);
    var L = track.getTotalLength();

    // domain with headroom so the band sits in the middle of the arc
    var span = hasRange ? (max - min) : 0;
    var lo = hasRange ? min - span * 0.9 : v - 1;
    var hi = hasRange ? max + span * 0.9 : v + 1;
    function frac(x) { return (x - lo) / (hi - lo); }
    function at(x) { return track.getPointAtLength(frac(x) * L); }

    if (hasRange) {
      var a = frac(min) * L, b = frac(max) * L;
      var band = el('path', { d: d, 'class': 'band', 'stroke-dasharray': (b - a) + ' ' + L, 'stroke-dashoffset': -a });
      svg.appendChild(band);
      var pMin = at(min), pMax = at(max);
      [[pMin, box.getAttribute('data-min-label'), 'end'], [pMax, box.getAttribute('data-max-label'), 'start']].forEach(function (t, i) {
        svg.appendChild(el('line', { x1: t[0].x, y1: t[0].y + 6, x2: t[0].x, y2: t[0].y + 14, 'class': 'tick' }));
        var tx = el('text', { x: t[0].x + (i === 0 ? -4 : 4), y: t[0].y + 28, 'class': 'lab', 'text-anchor': i === 0 ? 'end' : 'start' });
        tx.textContent = (i === 0 ? 'min ' : 'max ') + (t[1] || '');
        svg.appendChild(tx);
      });
    } else {
      var lab = el('text', { x: W / 2, y: 82, 'class': 'lab', 'text-anchor': 'middle' });
      lab.textContent = 'point fix · no range given';
      svg.appendChild(lab);
    }
    var p = at(v);
    // four-point star marker
    var r = 7, s = 2.4;
    var star = el('path', { 'class': 'fix', d:
      'M' + p.x + ' ' + (p.y - r) + ' L' + (p.x + s) + ' ' + (p.y - s) + ' L' + (p.x + r) + ' ' + p.y +
      ' L' + (p.x + s) + ' ' + (p.y + s) + ' L' + p.x + ' ' + (p.y + r) + ' L' + (p.x - s) + ' ' + (p.y + s) +
      ' L' + (p.x - r) + ' ' + p.y + ' L' + (p.x - s) + ' ' + (p.y - s) + ' Z' });
    svg.appendChild(el('circle', { cx: p.x, cy: p.y, r: 12, fill: 'rgba(212,174,92,.18)' }));
    svg.appendChild(star);
    box.appendChild(svg);
  });
})();
