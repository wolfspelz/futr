/* FUTR — Dataverse · charts in plain SVG + JS, no libraries */
(function () {
  'use strict';

  /* ---------------- data (from the FUTR repository) ---------------- */
  const UNIVERSES = [
    { id: 'firefly',    name: 'Firefly',         color: 'var(--c-firefly)',    civs: 1 },
    { id: 'honorverse', name: 'Honorverse',      color: 'var(--c-honorverse)', civs: 2 },
    { id: 'orion',      name: "Orion's Arm",     color: 'var(--c-orion)',      civs: 3 },
    { id: 'perry',      name: 'Perry Rhodan',    color: 'var(--c-perry)',      civs: 3 },
    { id: 'reality',    name: 'Reality',         color: 'var(--c-reality)',    civs: 10 },
    { id: 'startrek',   name: 'Star Trek',       color: 'var(--c-startrek)',   civs: 4 },
    { id: 'culture',    name: 'The Culture',     color: 'var(--c-culture)',    civs: 1 },
    { id: 'expanse',    name: 'The Expanse',     color: 'var(--c-expanse)',    civs: 4 },
    { id: 'warhammer',  name: 'Warhammer 40,000',color: 'var(--c-warhammer)',  civs: 2 },
  ];
  const UMAP = Object.fromEntries(UNIVERSES.map(u => [u.id, u]));

  // Civilizations with Population and Kardashev values. planets may be null.
  const CIVS = [
    { u: 'firefly',    name: 'Alliance 2517',        pop: 303e9,  k: 0.95, planets: 215 },
    { u: 'honorverse', name: 'Haven 1903 PD',        pop: 2e11,   k: 1.0,  planets: 100 },
    { u: 'honorverse', name: 'Manticore 1903 PD',    pop: 3.2e9,  k: 1.1,  planets: 3, href: 'civilization.html' },
    { u: 'perry',      name: 'Großes Imperium 2040', pop: 50e12,  k: 1.6,  planets: 50000 },
    { u: 'perry',      name: 'Solares Imperium 2040',pop: 9e9,    k: 0.9,  planets: 6 },
    { u: 'reality',    name: 'Earth 2023',           pop: 8.1e9,  k: 0.73, planets: 1 },
    { u: 'reality',    name: '20th Century',         pop: 1.15e9, k: 0.65, planets: 1 },
    { u: 'reality',    name: '80s',                  pop: 4e9,    k: 0.70, planets: 1 },
    { u: 'reality',    name: 'Age of Sail',          pop: 800e6,  k: 0.54, planets: 1 },
    { u: 'reality',    name: 'Bronze Age',           pop: 20e6,   k: 0.36, planets: 1 },
    { u: 'reality',    name: 'Classic Age',          pop: 100e6,  k: 0.46, planets: 1 },
    { u: 'reality',    name: 'Early Pleistocene',    pop: 18500,  k: 0.057,planets: 1 },
    { u: 'reality',    name: 'Industrialization',    pop: 1e9,    k: 0.56, planets: 1 },
    { u: 'reality',    name: 'Middle Ages',          pop: 200e6,  k: 0.48, planets: 1 },
    { u: 'reality',    name: 'Stone Age',            pop: 50000,  k: 0.10, planets: 1 },
    { u: 'startrek',   name: 'Andorian Empire 2161', pop: 8e9,    k: 0.9,  planets: 14 },
    { u: 'startrek',   name: 'Federation 2373',      pop: 100e9,  k: 1.0,  planets: 150 },
    { u: 'startrek',   name: 'Klingon Empire 2161',  pop: 9e9,    k: 0.9,  planets: 20 },
    { u: 'culture',    name: 'Culture (pre-war)',    pop: 30e12,  k: 2.3,  planets: null },
    { u: 'expanse',    name: 'Mars 2350',            pop: 4e9,    k: 0.85, planets: 1 },
    { u: 'expanse',    name: 'Outer Planets Alliance 2350', pop: 100e6, k: 0.67, planets: null },
    { u: 'expanse',    name: 'Sol System 2350',      pop: 35e9,   k: 0.9,  planets: 15 },
    { u: 'expanse',    name: 'United Nations 2350',  pop: 30e9,   k: 0.89, planets: 2 },
    { u: 'warhammer',  name: 'Imperium of Man M41',  pop: 1e16,   k: 1.6,  planets: 1e6 },
  ];

  /* ---------------- helpers ---------------- */
  const SVGNS = 'http://www.w3.org/2000/svg';
  function el(tag, attrs, parent) {
    const n = document.createElementNS(SVGNS, tag);
    for (const k in attrs) n.setAttribute(k, attrs[k]);
    if (parent) parent.appendChild(n);
    return n;
  }
  function txt(parent, x, y, s, attrs) {
    const t = el('text', Object.assign({ x, y }, attrs || {}), parent);
    t.textContent = s;
    return t;
  }
  const NAMES = ['', 'thousand', 'million', 'billion', 'trillion', 'quadrillion', 'quintillion'];
  function human(n) {
    if (n === null || n === undefined || isNaN(n)) return '—';
    if (Math.abs(n) < 1000) return (Math.round(n * 100) / 100).toString();
    let e = Math.floor(Math.log10(Math.abs(n)) / 3);
    if (e >= NAMES.length) return n.toExponential(1);
    const m = n / Math.pow(10, e * 3);
    const s = m >= 100 ? Math.round(m) : (Math.round(m * 10) / 10);
    return s + ' ' + NAMES[e];
  }
  function short(n) {
    if (n === null || n === undefined || isNaN(n)) return '—';
    if (Math.abs(n) < 1000) return (Math.round(n * 100) / 100).toString();
    const S = ['', 'k', 'M', 'B', 'T', 'Q'];
    let e = Math.floor(Math.log10(Math.abs(n)) / 3);
    if (e >= S.length) return '10^' + Math.round(Math.log10(n));
    const m = n / Math.pow(10, e * 3);
    return (m >= 100 ? Math.round(m) : Math.round(m * 10) / 10) + S[e];
  }
  function pow10Label(e) {
    const sup = '⁰¹²³⁴⁵⁶⁷⁸⁹';
    return '10' + String(e).split('').map(d => sup[+d]).join('');
  }
  function scaleFn(kind, min, max, out0, out1) {
    if (kind === 'log') {
      const a = Math.log10(min), b = Math.log10(max);
      return v => out0 + (Math.log10(v) - a) / (b - a) * (out1 - out0);
    }
    return v => out0 + (v - min) / (max - min) * (out1 - out0);
  }
  function clamp(v, a, b) { return Math.max(a, Math.min(b, v)); }

  window.FUTR = { UNIVERSES, CIVS, human, short };

  /* ---------------- bubble chart (home hero) ---------------- */
  function bubbleChart(root) {
    const W = 760, H = 480, m = { t: 20, r: 24, b: 56, l: 64 };
    const svg = el('svg', { viewBox: `0 0 ${W} ${H}`, role: 'img', 'aria-label': 'Population versus Kardashev level of all civilizations, bubble size is number of planets' });
    root.querySelector('.chart-host').appendChild(svg);

    const x = scaleFn('log', 1e4, 1e17, m.l, W - m.r);
    const y = scaleFn('lin', 0, 2.5, H - m.b, m.t);
    const r = v => v == null ? 7 : clamp(6 + Math.log10(v) * 4.2, 6, 34);

    // grid + axes
    for (let e = 4; e <= 17; e++) {
      if (e % 2) continue;
      el('line', { x1: x(Math.pow(10, e)), x2: x(Math.pow(10, e)), y1: m.t, y2: H - m.b, class: 'grid-line' }, svg);
      txt(svg, x(Math.pow(10, e)), H - m.b + 18, pow10Label(e), { class: 'tick', 'text-anchor': 'middle' });
    }
    for (let k = 0; k <= 2.5; k += 0.5) {
      el('line', { x1: m.l, x2: W - m.r, y1: y(k), y2: y(k), class: 'grid-line' }, svg);
      txt(svg, m.l - 10, y(k) + 4, k.toFixed(1), { class: 'tick', 'text-anchor': 'end' });
    }
    el('line', { x1: m.l, x2: W - m.r, y1: H - m.b, y2: H - m.b, class: 'axis-line' }, svg);
    el('line', { x1: m.l, x2: m.l, y1: m.t, y2: H - m.b, class: 'axis-line' }, svg);
    txt(svg, W - m.r, H - 14, 'Population  →', { class: 'axis-label', 'text-anchor': 'end' });
    txt(svg, 0, 0, '↑  Kardashev', { class: 'axis-label', transform: `translate(16 ${m.t}) rotate(-90)`, 'text-anchor': 'end' });

    // bubbles, biggest first so small ones are clickable
    const sorted = CIVS.slice().sort((a, b) => (b.planets || 0) - (a.planets || 0));
    const groups = {};
    const tip = root.querySelector('.chart-tip');
    sorted.forEach(c => {
      const u = UMAP[c.u];
      const g = groups[c.u] || (groups[c.u] = el('g', { 'data-u': c.u }, svg));
      const circ = el('circle', { cx: x(c.pop), cy: y(c.k), r: r(c.planets), class: 'bubble', fill: u.color, stroke: u.color, tabindex: 0 }, g);
      const title = el('title', {}, circ);
      title.textContent = `${c.name} · ${u.name}`;
      if (c.href) circ.style.cursor = 'pointer';
      const show = () => {
        tip.innerHTML = `<b>${c.name}</b><span class="u">${u.name}</span><br>Population ${human(c.pop)}<br>Kardashev ${c.k}<br>Planets ${c.planets == null ? 'n/a' : human(c.planets)}`;
        const bb = circ.getBoundingClientRect(), rb = root.getBoundingClientRect();
        tip.style.left = (bb.left - rb.left + bb.width / 2) + 'px';
        tip.style.top = (bb.top - rb.top) + 'px';
        tip.classList.add('show');
      };
      circ.addEventListener('mouseenter', show);
      circ.addEventListener('focus', show);
      circ.addEventListener('mouseleave', () => tip.classList.remove('show'));
      circ.addEventListener('blur', () => tip.classList.remove('show'));
      circ.addEventListener('click', () => { if (c.href) location.href = c.href; });
    });
    // a few labels for orientation
    const label = (name, dx, dy, anchor) => {
      const c = CIVS.find(v => v.name === name); if (!c) return;
      txt(svg, x(c.pop) + dx, y(c.k) + dy, name, { class: 'bubble-label', 'text-anchor': anchor || 'start' });
    };
    label('Culture (pre-war)', 0, -16, 'middle');
    label('Imperium of Man M41', 0, -40, 'middle');
    label('Großes Imperium 2040', 0, -32, 'middle');
    label('Manticore 1903 PD', 10, 24);
    label('Earth 2023', -14, -10, 'end');
    label('Stone Age', 0, -14, 'middle');

    // legend with overflow
    const legend = root.querySelector('.legend');
    const MAXVIS = 8;
    const present = UNIVERSES.filter(u => groups[u.id]);
    const active = new Set(present.map(u => u.id));
    function apply() {
      present.forEach(u => groups[u.id].classList.toggle('series-dim', !active.has(u.id)));
    }
    function makeBtn(u) {
      const b = document.createElement('button');
      b.type = 'button'; b.setAttribute('aria-pressed', 'true'); b.style.setProperty('--c', u.color);
      b.innerHTML = `<i></i>${u.name}`;
      b.addEventListener('click', () => {
        const on = active.has(u.id);
        if (on && active.size === present.length) { active.clear(); active.add(u.id); }
        else if (on) active.delete(u.id); else active.add(u.id);
        if (active.size === 0) present.forEach(p => active.add(p.id));
        legend.querySelectorAll('button[data-u]').forEach(x => x.setAttribute('aria-pressed', String(active.has(x.dataset.u))));
        apply();
      });
      b.dataset.u = u.id;
      return b;
    }
    present.slice(0, MAXVIS).forEach(u => legend.appendChild(makeBtn(u)));
    if (present.length > MAXVIS) {
      const more = document.createElement('button');
      more.type = 'button'; more.className = 'legend-more'; more.textContent = `+${present.length - MAXVIS} more`;
      more.addEventListener('click', () => {
        present.slice(MAXVIS).forEach(u => legend.insertBefore(makeBtn(u), more));
        more.remove();
      });
      legend.appendChild(more);
    }
  }

  /* ---------------- universe card mini bars ---------------- */
  function miniBars() {
    document.querySelectorAll('.uni-card .minibar[data-values]').forEach(box => {
      const vals = box.dataset.values.split(',').map(Number);
      const max = Math.max.apply(null, vals) || 1;
      box.innerHTML = vals.map(v => `<i style="height:${Math.max(3, Math.round(v / max * 22))}px"></i>`).join('');
    });
  }

  /* ---------------- show-more lists ---------------- */
  function showMore() {
    document.querySelectorAll('[data-showmore]').forEach(wrap => {
      const n = +wrap.dataset.showmore;
      const items = Array.from(wrap.querySelectorAll('[data-item]'));
      const btn = document.querySelector(wrap.dataset.button);
      if (!btn) return;
      let open = false;
      const render = () => {
        items.forEach((it, i) => it.classList.toggle('hidden', !open && i >= n));
        const hidden = Math.max(0, items.length - n);
        btn.textContent = open ? 'Show fewer' : `Show ${hidden} more`;
        btn.hidden = hidden === 0;
      };
      btn.addEventListener('click', () => { open = !open; render(); });
      render();
    });
  }

  /* ---------------- metric bars (home) ---------------- */
  function metricBars() {
    const bars = document.querySelectorAll('.metric-bar');
    if (!bars.length) return;
    const max = Math.max.apply(null, Array.from(bars).map(b => +b.dataset.n));
    const io = new IntersectionObserver(entries => {
      entries.forEach(e => { if (e.isIntersecting) {
        bars.forEach(b => { b.querySelector('.m-fill').style.width = Math.max(1.5, (+b.dataset.n) / max * 100) + '%'; });
        io.disconnect();
      } });
    }, { threshold: 0.2 });
    io.observe(bars[0]);
  }

  /* ---------------- data table: inline bars, sort, filter ---------------- */
  function dataTable(table) {
    const ths = Array.from(table.querySelectorAll('thead th'));
    const rows = Array.from(table.querySelectorAll('tbody tr[data-href]'));
    // inline bars
    ths.forEach((th, ci) => {
      const kind = th.dataset.scale; if (!kind) return;
      const min = +th.dataset.min, max = +th.dataset.max;
      const sc = scaleFn(kind, min, max, 0, 100);
      rows.forEach(tr => {
        const td = tr.children[ci]; const v = +td.dataset.v; if (isNaN(v)) return;
        const pct = clamp(sc(v), 1.5, 100);
        const raw = td.dataset.v;
        const display = th.dataset.fmt === 'human' ? human(v) : (th.dataset.fmt === 'int' ? v.toLocaleString('en-US') : v);
        td.innerHTML = `<div class="cell-bar" title="${raw} ${th.dataset.unit || ''}"><div class="track"><div class="fill" style="width:${pct}%"></div></div><div class="v">${display}${th.dataset.fmt === 'human' && v >= 1000 ? `<small>${raw}</small>` : ''}</div></div>`;
      });
    });
    // row click
    rows.forEach(tr => {
      tr.addEventListener('click', e => { if (e.target.closest('a')) return; location.href = tr.dataset.href; });
    });
    // sort
    ths.forEach((th, ci) => {
      const btn = th.querySelector('button'); if (!btn) return;
      btn.addEventListener('click', () => {
        const cur = th.getAttribute('aria-sort');
        const dir = cur === 'descending' ? 'ascending' : 'descending';
        ths.forEach(t => t.removeAttribute('aria-sort'));
        th.setAttribute('aria-sort', dir);
        const tbody = table.tBodies[0];
        rows.sort((a, b) => {
          const av = a.children[ci].dataset.v, bv = b.children[ci].dataset.v;
          const an = parseFloat(av), bn = parseFloat(bv);
          let c = (!isNaN(an) && !isNaN(bn)) ? an - bn : String(av ?? a.children[ci].textContent).localeCompare(String(bv ?? b.children[ci].textContent));
          return dir === 'ascending' ? c : -c;
        }).forEach(r => tbody.appendChild(r));
      });
    });
    // filter
    const input = document.querySelector(table.dataset.filter);
    const hint = document.querySelector(table.dataset.hint);
    const empty = table.querySelector('.empty');
    const update = () => {
      const q = (input ? input.value : '').trim().toLowerCase();
      let n = 0;
      rows.forEach(r => { const hit = !q || r.textContent.toLowerCase().includes(q); r.classList.toggle('hidden', !hit); if (hit) n++; });
      if (hint) hint.textContent = `Showing ${n} of ${rows.length} civilizations`;
      if (empty) empty.hidden = n > 0;
    };
    if (input) input.addEventListener('input', update);
    update();
  }

  /* ---------------- dumbbell comparison (universe page) ---------------- */
  function dumbbell(root) {
    const rowsData = JSON.parse(root.dataset.rows);
    const a = root.dataset.a, b = root.dataset.b;
    const W = 720, rowH = 44, m = { l: 150, r: 70, t: 10 };
    const H = m.t + rowsData.length * rowH + 10;
    const svg = el('svg', { viewBox: `0 0 ${W} ${H}`, role: 'img', 'aria-label': `${a} versus ${b} on each showcase metric` });
    root.appendChild(svg);
    rowsData.forEach((row, i) => {
      const cy = m.t + i * rowH + rowH / 2;
      const sc = scaleFn(row.scale, row.min, row.max, m.l, W - m.r);
      txt(svg, m.l - 16, cy + 4, row.metric, { class: 'lbl', 'text-anchor': 'end' });
      el('line', { x1: m.l, x2: W - m.r, y1: cy, y2: cy, class: 'rail' }, svg);
      txt(svg, m.l, cy + 18, row.scale === 'log' ? short(row.min) : row.min, { class: 'sub' });
      txt(svg, W - m.r, cy + 18, row.scale === 'log' ? short(row.max) : row.max, { class: 'sub', 'text-anchor': 'end' });
      const xa = sc(row.a), xb = sc(row.b);
      el('line', { x1: xa, x2: xb, y1: cy, y2: cy, class: 'seg' }, svg);
      el('circle', { cx: xb, cy, r: 7, class: 'pt-b' }, svg).appendChild(Object.assign(document.createElementNS(SVGNS, 'title'), { textContent: `${b}: ${row.b}` }));
      el('circle', { cx: xa, cy, r: 8, class: 'pt-a' }, svg).appendChild(Object.assign(document.createElementNS(SVGNS, 'title'), { textContent: `${a}: ${row.a}` }));
      const left = Math.min(xa, xb), right = Math.max(xa, xb);
      const la = xa <= xb, va = row.fmt === 'human' ? short(row.a) : row.a, vb = row.fmt === 'human' ? short(row.b) : row.b;
      if (right - left > 60) {
        txt(svg, xa + (la ? -14 : 14), cy + 4, va, { class: 'val', 'text-anchor': la ? 'end' : 'start' });
        txt(svg, xb + (la ? 14 : -14), cy + 4, vb, { class: 'val', 'text-anchor': la ? 'start' : 'end' });
      } else {
        txt(svg, left - 14, cy + 4, la ? va : vb, { class: 'val', 'text-anchor': 'end' });
        txt(svg, right + 14, cy + 4, la ? vb : va, { class: 'val', 'text-anchor': 'start' });
      }
    });
  }

  /* ---------------- datapoint axis cards (civilization page) ---------------- */
  function dpAxes() {
    document.querySelectorAll('.dp-card[data-scale]').forEach(card => {
      const d = card.dataset;
      const min = +d.min, max = +d.max, v = +d.v;
      const W = 320, H = 62, l = 8, rgt = 8, cy = 30;
      const sc = scaleFn(d.scale, min, max, l, W - rgt);
      const svg = el('svg', { viewBox: `0 0 ${W} ${H}`, class: 'dp-axis', preserveAspectRatio: 'none', role: 'img', 'aria-label': `${d.metric} on a ${d.scale} scale from ${min} to ${max}; value ${v}` });
      svg.setAttribute('preserveAspectRatio', 'xMidYMid meet');
      card.querySelector('.dp-axis-host').appendChild(svg);
      el('line', { x1: l, x2: W - rgt, y1: cy, y2: cy, class: 'rail' }, svg);
      // ticks
      if (d.scale === 'log') {
        for (let e = Math.ceil(Math.log10(min)); e <= Math.floor(Math.log10(max)); e++) {
          const xx = sc(Math.pow(10, e));
          el('line', { x1: xx, x2: xx, y1: cy - 4, y2: cy + 4, class: 'tick-l' }, svg);
        }
      } else {
        const step = d.step ? +d.step : (max - min) / 5;
        for (let t = min; t <= max + 1e-9; t += step) { const xx = sc(t); el('line', { x1: xx, x2: xx, y1: cy - 4, y2: cy + 4, class: 'tick-l' }, svg); }
      }
      const fmt = n => d.scale === 'log' ? pow10Label(Math.round(Math.log10(n))) : n;
      txt(svg, l, 56, fmt(min), {});
      txt(svg, W - rgt, 56, fmt(max), { 'text-anchor': 'end' });
      txt(svg, W / 2, 56, d.scale === 'log' ? 'log scale · all civilizations' : 'all civilizations', { 'text-anchor': 'middle' });
      // comparison markers
      const xe = d.earth ? sc(+d.earth) : null, xh = d.haven ? sc(+d.haven) : null;
      const near = xe !== null && xh !== null && Math.abs(xh - xe) < 70;
      if (xe !== null) {
        el('rect', { x: xe - 5, y: cy - 5, width: 10, height: 10, class: 'cmp-e', transform: `rotate(45 ${xe} ${cy})` }, svg);
        const a = near ? (xe <= xh ? 'end' : 'start') : 'middle';
        txt(svg, xe + (near ? (xe <= xh ? -9 : 9) : 0), 12, 'Earth 2023', { class: 'cmp-lbl', 'text-anchor': a });
      }
      if (xh !== null) {
        el('circle', { cx: xh, cy, r: 5.5, class: 'cmp' }, svg);
        const a = near ? (xh < xe ? 'end' : 'start') : 'middle';
        txt(svg, xh + (near ? (xh < xe ? -9 : 9) : 0), 12, 'Haven 1903 PD', { class: 'cmp-lbl', 'text-anchor': a });
      }
      // range + value
      if (d.lo && d.hi) el('line', { x1: sc(+d.lo), x2: sc(+d.hi), y1: cy, y2: cy, class: 'range' }, svg);
      el('circle', { cx: sc(v), cy, r: 7, class: 'val' }, svg);
    });
  }

  /* ---------------- dp filter (numeric / text / all) ---------------- */
  function dpFilter() {
    const tools = document.querySelector('.dp-tools'); if (!tools) return;
    const cards = document.querySelectorAll('.dp-card');
    tools.querySelectorAll('.seg-btns button').forEach(btn => btn.addEventListener('click', () => {
      tools.querySelectorAll('.seg-btns button').forEach(b => b.setAttribute('aria-pressed', 'false'));
      btn.setAttribute('aria-pressed', 'true');
      const f = btn.dataset.filter;
      cards.forEach(c => c.classList.toggle('hidden', f !== 'all' && c.dataset.kind !== f));
    }));
    const open = document.querySelector('[data-open-all]');
    if (open) open.addEventListener('click', () => {
      const all = document.querySelectorAll('.dp-body details');
      const anyClosed = Array.from(all).some(d => !d.open);
      all.forEach(d => d.open = anyClosed);
      open.textContent = anyClosed ? 'Collapse all' : 'Expand all';
    });
  }

  /* ---------------- misc ---------------- */
  function nav() {
    const t = document.querySelector('.nav-toggle'), n = document.querySelector('.nav');
    if (t && n) t.addEventListener('click', () => { const o = n.classList.toggle('open'); t.setAttribute('aria-expanded', String(o)); });
  }
  function images() {
    document.querySelectorAll('img[data-fallback]').forEach(img => {
      const fail = () => { img.classList.add('broken'); const ph = img.parentElement.querySelector('.ph'); if (ph) ph.hidden = false; };
      img.addEventListener('error', fail);
      if (img.complete && img.naturalWidth === 0) fail();
    });
  }
  function reveal() {
    const items = document.querySelectorAll('.reveal');
    if (!('IntersectionObserver' in window)) { items.forEach(i => i.classList.add('in')); return; }
    const io = new IntersectionObserver(es => es.forEach(e => { if (e.isIntersecting) { e.target.classList.add('in'); io.unobserve(e.target); } }), { threshold: 0.08 });
    items.forEach(i => io.observe(i));
  }

  document.addEventListener('DOMContentLoaded', () => {
    nav(); images(); reveal(); miniBars(); showMore(); metricBars(); dpAxes(); dpFilter();
    const bc = document.querySelector('[data-bubble-chart]'); if (bc) bubbleChart(bc);
    document.querySelectorAll('table.data-table').forEach(dataTable);
    document.querySelectorAll('.dumbbell[data-rows]').forEach(dumbbell);
  });
})();
