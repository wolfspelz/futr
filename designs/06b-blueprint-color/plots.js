/* FUTR Blueprint 06b — hero plots, plain SVG + JS, no libraries.
   Several plots over the same civilization set, switched by a tab bar. Engine shared with design 03b. */
(function () {
  'use strict';

  /* ---------------- data (scanned from data/universes, 2026-10-22) ---------------- */
  const UNIVERSES = [
    { id: 'firefly',    name: 'Firefly' },
    { id: 'honorverse', name: 'Honorverse' },
    { id: 'orion',      name: "Orion's Arm" },
    { id: 'perry',      name: 'Perry Rhodan' },
    { id: 'reality',    name: 'Reality', acid: true },
    { id: 'startrek',   name: 'Star Trek' },
    { id: 'culture',    name: 'The Culture' },
    { id: 'expanse',    name: 'The Expanse' },
    { id: 'warhammer',  name: 'Warhammer 40,000' },
  ];
  const UMAP = Object.fromEntries(UNIVERSES.map(u => [u.id, u]));

  // pop = Population, k = Kardashev, tl = Techlevel, dom = Domain, reach = Reach, pl = Planets, ships = Warships
  const CIVS = [
    { u: 'firefly',    name: 'Alliance 2517',        pop: 303e9,   k: 0.95, tl: 7,  dom: 9,  reach: 9,  pl: 215,   ships: null },
    { u: 'honorverse', name: 'Haven 1903 PD',        pop: 2e11,    k: 1.0,  tl: 9,  dom: 11, reach: 11, pl: 100,   ships: 2106 },
    { u: 'honorverse', name: 'Manticore 1903 PD',    pop: 3.2e9,   k: 1.1,  tl: 9,  dom: 10, reach: 11, pl: 3,     ships: 1621, href: 'civilization.html' },
    { u: 'orion',      name: 'Diamond Network',      pop: 30e12,   k: null, tl: 14, dom: 12, reach: 13, pl: null,  ships: null },
    { u: 'orion',      name: 'Solar System (OA)',    pop: 6.97e12, k: null, tl: 13, dom: 9,  reach: 9,  pl: null,  ships: null },
    { u: 'orion',      name: 'Terragen Sphere',      pop: 390e15,  k: null, tl: 14, dom: 13, reach: 13, pl: null,  ships: null },
    { u: 'perry',      name: 'Großes Imperium 2040', pop: 50e12,   k: 1.6,  tl: 10, dom: 13, reach: 13, pl: 50000, ships: 100000 },
    { u: 'perry',      name: 'Posbis 2114',          pop: null,    k: 1.3,  tl: 10, dom: 11, reach: 16, pl: 5,     ships: 10000 },
    { u: 'perry',      name: 'Solares Imperium 2040',pop: 9e9,     k: 0.9,  tl: 9,  dom: 10, reach: 11, pl: 6,     ships: 3500 },
    { u: 'reality',    name: 'Earth 2023',           pop: 8.1e9,   k: 0.73, tl: 6,  dom: 7,  reach: 8,  pl: 1,     ships: null },
    { u: 'reality',    name: '20th Century',         pop: 1.15e9,  k: 0.65, tl: 6,  dom: 6,  reach: 6,  pl: 1,     ships: null },
    { u: 'reality',    name: '80s',                  pop: 4e9,     k: 0.70, tl: 6,  dom: 7,  reach: 8,  pl: 1,     ships: null },
    { u: 'reality',    name: 'Age of Sail',          pop: 800e6,   k: 0.54, tl: 4,  dom: 6,  reach: 6,  pl: 1,     ships: null },
    { u: 'reality',    name: 'Bronze Age',           pop: 20e6,    k: 0.36, tl: 3,  dom: 4,  reach: 5,  pl: 1,     ships: null },
    { u: 'reality',    name: 'Classic Age',          pop: 100e6,   k: 0.46, tl: 3,  dom: 5,  reach: 6,  pl: 1,     ships: null },
    { u: 'reality',    name: 'Early Pleistocene',    pop: 18500,   k: 0.057,tl: 1,  dom: 2,  reach: 2,  pl: 1,     ships: null },
    { u: 'reality',    name: 'Industrialization',    pop: 1e9,     k: 0.56, tl: 5,  dom: 6,  reach: 6,  pl: 1,     ships: null },
    { u: 'reality',    name: 'Middle Ages',          pop: 200e6,   k: 0.48, tl: 4,  dom: 5,  reach: 6,  pl: 1,     ships: null },
    { u: 'reality',    name: 'Stone Age',            pop: 50000,   k: 0.10, tl: 2,  dom: 2,  reach: 4,  pl: 1,     ships: null },
    { u: 'startrek',   name: 'Andorian Empire 2161', pop: 8e9,     k: 0.9,  tl: 9,  dom: 10, reach: 10, pl: 14,    ships: null },
    { u: 'startrek',   name: 'Federation 2373',      pop: 100e9,   k: 1.0,  tl: 10, dom: 11, reach: 12, pl: 150,   ships: null },
    { u: 'startrek',   name: 'Klingon Empire 2161',  pop: 9e9,     k: 0.9,  tl: 9,  dom: 10, reach: 11, pl: 20,    ships: null },
    { u: 'startrek',   name: 'Vulcans 2063',         pop: null,    k: null, tl: 9,  dom: 10, reach: 11, pl: 1,     ships: null },
    { u: 'culture',    name: 'Culture (pre-war)',    pop: 30e12,   k: 2.3,  tl: 13, dom: 13, reach: 15, pl: null,  ships: 1e6 },
    { u: 'expanse',    name: 'Mars 2350',            pop: 4e9,     k: 0.85, tl: 7,  dom: 8,  reach: 9,  pl: 1,     ships: 60 },
    { u: 'expanse',    name: 'Outer Planets Alliance 2350', pop: 100e6, k: 0.67, tl: 7, dom: 8, reach: 9, pl: null, ships: 10 },
    { u: 'expanse',    name: 'Sol System 2350',      pop: 35e9,    k: 0.9,  tl: 7,  dom: 9,  reach: 9,  pl: 15,    ships: 350 },
    { u: 'expanse',    name: 'United Nations 2350',  pop: 30e9,    k: 0.89, tl: 7,  dom: 8,  reach: 9,  pl: 2,     ships: 300 },
    { u: 'warhammer',  name: 'Imperium of Man M41',  pop: 1e16,    k: 1.6,  tl: 10, dom: 15, reach: 15, pl: 1e6,   ships: 5e5 },
    { u: 'warhammer',  name: 'Orks M41',             pop: 5e16,    k: null, tl: 10, dom: 12, reach: 15, pl: 2e6,   ships: null },
  ];

  /* ---------------- metric definitions ---------------- */
  const METRICS = {
    pop:   { label: 'Population', scale: 'log', min: 1e4,  max: 1e18, ticks: [1e4, 1e6, 1e8, 1e10, 1e12, 1e14, 1e16, 1e18], fmt: pow10 },
    k:     { label: 'Kardashev',  scale: 'lin', min: 0,    max: 2.5,  ticks: [0, 0.5, 1, 1.5, 2, 2.5], fmt: v => v.toFixed(1) },
    tl:    { label: 'Techlevel',  scale: 'lin', min: 0,    max: 15,   ticks: [0, 3, 6, 9, 12, 15], fmt: v => 'TL ' + v },
    dom:   { label: 'Domain',     scale: 'lin', min: 0,    max: 20,   ticks: [0, 5, 10, 15, 20], fmt: v => String(v) },
    reach: { label: 'Reach',      scale: 'lin', min: 0,    max: 20,   ticks: [0, 5, 10, 15, 20], fmt: v => String(v) },
    pl:    { label: 'Planets',    scale: 'log', min: 0.5,  max: 1e7,  ticks: [1, 10, 100, 1e3, 1e4, 1e5, 1e6], fmt: v => v < 1000 ? String(v) : pow10(v) },
    ships: { label: 'Warships',   scale: 'log', min: 5,    max: 2e6,  ticks: [10, 100, 1e3, 1e4, 1e5, 1e6], fmt: v => v < 1000 ? String(v) : pow10(v) },
  };

  /* ---------------- plots: the tab bar ---------------- */
  const PLOTS = [
    { id: 'tech',     tab: 'Tech',     title: 'Size against technology',   sub: 'Population × Techlevel · size = Reach', note: 'Every universe is on this one: Techlevel exists for all civilizations.',
      x: 'pop', y: 'tl', size: 'reach', labels: ['Terragen Sphere', 'Culture (pre-war)', 'Orks M41', 'Earth 2023', 'Stone Age'] },
    { id: 'kardashev', tab: 'Energy', title: 'Size against energy',        sub: 'Population × Kardashev · size = Planets', note: 'Energy use grows with population, but not for everybody at the same rate.',
      x: 'pop', y: 'k', size: 'pl', labels: ['Culture (pre-war)', 'Imperium of Man M41', 'Großes Imperium 2040', 'Manticore 1903 PD', 'Earth 2023', 'Stone Age'] },
    { id: 'k-tl',     tab: 'Energy vs Tech', title: 'Energy against technology', sub: 'Kardashev × Techlevel · size = Population', note: 'High tech on little energy is possible: a Matrioshka brain running on fusion is still TL 7.',
      x: 'k', y: 'tl', size: 'pop', labels: ['Culture (pre-war)', 'Imperium of Man M41', 'Manticore 1903 PD', 'Earth 2023', 'Early Pleistocene'] },
    { id: 'reach',    tab: 'Reach',    title: 'Where they rule against where they are', sub: 'Domain × Reach · size = Population', note: 'Above the line a civilization is present beyond what it governs.',
      x: 'dom', y: 'reach', size: 'pop', diag: true, labels: ['Posbis 2114', 'Culture (pre-war)', 'Imperium of Man M41', 'Earth 2023', 'Early Pleistocene'] },
    { id: 'planets',  tab: 'Planets',  title: 'People per world',           sub: 'Planets × Population · size = Techlevel', note: 'Above the guide line worlds are crowded, below it they are thinly settled.',
      x: 'pl', y: 'pop', size: 'tl', labels: ['Orks M41', 'Imperium of Man M41', 'Großes Imperium 2040', 'Federation 2373', 'Manticore 1903 PD', 'Earth 2023'] },
    { id: 'fleet',    tab: 'Fleets',   title: 'Militarization',             sub: 'Population × Warships · size = Techlevel', note: 'Ships per head: the Culture arms a tiny population, the Imperium a huge one.',
      x: 'pop', y: 'ships', size: 'tl', labels: ['Culture (pre-war)', 'Imperium of Man M41', 'Großes Imperium 2040', 'Manticore 1903 PD', 'Mars 2350'] },
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
    if (n === null || n === undefined || isNaN(n)) return 'n/a';
    if (Math.abs(n) < 1000) return (Math.round(n * 100) / 100).toString();
    let e = Math.floor(Math.log10(Math.abs(n)) / 3);
    if (e >= NAMES.length) return n.toExponential(1);
    const m = n / Math.pow(10, e * 3);
    const s = m >= 100 ? Math.round(m) : (Math.round(m * 10) / 10);
    return s + ' ' + NAMES[e];
  }
  function pow10(v) {
    const sup = '⁰¹²³⁴⁵⁶⁷⁸⁹';
    const e = Math.round(Math.log10(v));
    return '10' + String(e).split('').map(d => sup[+d]).join('');
  }
  function scaleFn(m, out0, out1) {
    if (m.scale === 'log') {
      const a = Math.log10(m.min), b = Math.log10(m.max);
      return v => out0 + (Math.log10(v) - a) / (b - a) * (out1 - out0);
    }
    return v => out0 + (v - m.min) / (m.max - m.min) * (out1 - out0);
  }
  function sizeFn(key) {
    const m = METRICS[key];
    if (m.scale === 'log') return v => v == null ? 6 : Math.max(6, Math.min(34, 4 + Math.log10(v) * 2.2));
    return v => v == null ? 6 : Math.max(6, Math.min(34, 5 + (v - m.min) / (m.max - m.min) * 26));
  }
  function has(c, key) { return c[key] !== null && c[key] !== undefined && !isNaN(c[key]); }
  const UNITS = { pop: 'Population', k: 'Kardashev', tl: 'Techlevel', dom: 'Domain', reach: 'Reach', pl: 'Planets', ships: 'Warships' };
  function fmtVal(key, v) {
    if (v == null) return 'n/a';
    if (key === 'k') return String(v);
    if (key === 'tl') return 'TL ' + v;
    return human(v);
  }

  /* ---------------- one plot ---------------- */
  function drawPlot(root, plot, state) {
    const host = root.querySelector('.chart-host');
    host.innerHTML = '';
    const W = 640, H = 470, m = { t: 34, r: 28, b: 54, l: 58 };
    const MX = METRICS[plot.x], MY = METRICS[plot.y];
    const svg = el('svg', { viewBox: `0 0 ${W} ${H}`, role: 'img', 'aria-label': `${MX.label} versus ${MY.label} of all civilizations` });
    host.appendChild(svg);

    const x = scaleFn(MX, m.l, W - m.r);
    const y = scaleFn(MY, H - m.b, m.t);
    const r = sizeFn(plot.size);

    // grid + axes
    MX.ticks.forEach(v => {
      el('line', { x1: x(v), x2: x(v), y1: m.t, y2: H - m.b, class: 'grid-line' }, svg);
      txt(svg, x(v), H - m.b + 18, MX.fmt(v), { class: 'tick', 'text-anchor': 'middle' });
    });
    MY.ticks.forEach(v => {
      el('line', { x1: m.l, x2: W - m.r, y1: y(v), y2: y(v), class: 'grid-line' }, svg);
      txt(svg, m.l - 10, y(v) + 4, MY.fmt(v), { class: 'tick', 'text-anchor': 'end' });
    });
    if (plot.diag) {
      const lo = Math.max(MX.min, MY.min), hi = Math.min(MX.max, MY.max);
      el('line', { x1: x(lo), y1: y(lo), x2: x(hi), y2: y(hi), class: 'guide-line' }, svg);
      txt(svg, x(hi) - 6, y(hi) + 14, 'DOMAIN = REACH', { class: 'guide-label', 'text-anchor': 'end' });
    }
    if (plot.id === 'planets') {
      // guide: 10 billion people per planet
      const a = MX.min * 2, b = MX.max / 2;
      el('line', { x1: x(a), y1: y(Math.min(MY.max, a * 1e10)), x2: x(b), y2: y(Math.min(MY.max, b * 1e10)), class: 'guide-line' }, svg);
      txt(svg, x(b) - 6, y(b * 1e10) - 8, '10 BILLION PER WORLD', { class: 'guide-label', 'text-anchor': 'end' });
    }
    el('line', { x1: m.l, x2: W - m.r, y1: H - m.b, y2: H - m.b, class: 'axis-line' }, svg);
    el('line', { x1: m.l, x2: m.l, y1: m.t, y2: H - m.b, class: 'axis-line' }, svg);
    txt(svg, W - m.r, H - 12, MX.label.toUpperCase() + ' →', { class: 'axis-label', 'text-anchor': 'end' });
    txt(svg, m.l, m.t - 12, '↑ ' + MY.label.toUpperCase(), { class: 'axis-label', 'text-anchor': 'start' });

    // bubbles, biggest first so small ones stay clickable
    const shown = CIVS.filter(c => has(c, plot.x) && has(c, plot.y));
    const sorted = shown.slice().sort((a, b) => (b[plot.size] || 0) - (a[plot.size] || 0));
    const groups = {};
    const tip = root.querySelector('.chart-tip');
    sorted.forEach(c => {
      const u = UMAP[c.u];
      const g = groups[c.u] || (groups[c.u] = el('g', { 'data-u': c.u, class: u.acid ? 'series acid' : 'series' }, svg));
      const circ = el('circle', { cx: x(c[plot.x]), cy: y(c[plot.y]), r: r(c[plot.size]), class: 'bubble', tabindex: 0 }, g);
      const title = el('title', {}, circ);
      title.textContent = `${c.name} · ${u.name}`;
      if (c.href) circ.style.cursor = 'pointer';
      const show = () => {
        tip.innerHTML = `<b>${c.name}</b><span class="u">${u.name}</span>` +
          [plot.x, plot.y, plot.size].map(k => `${UNITS[k]} ${fmtVal(k, c[k])}`).join('<br>');
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

    // orientation labels, pushed up or down so they do not sit on the bubble
    (plot.labels || []).forEach((name, i) => {
      const c = shown.find(v => v.name === name); if (!c) return;
      const above = i % 2 === 0;
      const dy = above ? -(r(c[plot.size]) + 6) : (r(c[plot.size]) + 14);
      txt(svg, x(c[plot.x]), y(c[plot.y]) + dy, name.toUpperCase(), { class: 'bubble-label', 'text-anchor': 'middle' });
    });

    // footer: how many civilizations this plot can show
    root.querySelector('.chart-count').textContent = `${shown.length} of ${CIVS.length} civilizations have both values. ${plot.note || ''}`;

    state.groups = groups;
    state.present = UNIVERSES.filter(u => groups[u.id]);
    applyLegend(root, state);
  }

  /* ---------------- legend: click to isolate a universe ---------------- */
  function applyLegend(root, state) {
    const legend = root.querySelector('.legend');
    legend.innerHTML = '';
    const present = state.present;
    // keep the user's selection across plots, but never end up with nothing visible
    if (![...state.active].some(id => present.find(u => u.id === id))) present.forEach(u => state.active.add(u.id));
    const all = present.every(u => state.active.has(u.id));
    function paint() {
      present.forEach(u => state.groups[u.id].classList.toggle('series-dim', !state.active.has(u.id)));
      legend.querySelectorAll('button[data-u]').forEach(b => b.setAttribute('aria-pressed', String(state.active.has(b.dataset.u))));
    }
    const MAXVIS = 8;
    function makeBtn(u) {
      const b = document.createElement('button');
      b.type = 'button'; b.dataset.u = u.id;
      if (u.acid) b.classList.add('acid');
      b.innerHTML = `<i></i>${u.name}`;
      b.addEventListener('click', () => {
        const on = state.active.has(u.id);
        const allOn = present.every(p => state.active.has(p.id));
        if (on && allOn) { present.forEach(p => state.active.delete(p.id)); state.active.add(u.id); }
        else if (on) state.active.delete(u.id); else state.active.add(u.id);
        if (!present.some(p => state.active.has(p.id))) present.forEach(p => state.active.add(p.id));
        paint();
      });
      return b;
    }
    present.slice(0, MAXVIS).forEach(u => legend.appendChild(makeBtn(u)));
    if (present.length > MAXVIS) {
      const more = document.createElement('button');
      more.type = 'button'; more.className = 'legend-more'; more.textContent = `+${present.length - MAXVIS} more`;
      more.addEventListener('click', () => {
        present.slice(MAXVIS).forEach(u => legend.insertBefore(makeBtn(u), more));
        more.remove(); paint();
      });
      legend.appendChild(more);
    }
    if (all) present.forEach(u => state.active.add(u.id));
    paint();
  }

  /* ---------------- tab bar ---------------- */
  function chartPanel(root) {
    const tabs = root.querySelector('.chart-tabs');
    const state = { active: new Set(UNIVERSES.map(u => u.id)), groups: {}, present: [] };
    let current = PLOTS[0];
    function select(plot) {
      current = plot;
      tabs.querySelectorAll('button').forEach(b => b.setAttribute('aria-selected', String(b.dataset.plot === plot.id)));
      root.querySelector('.chart-title h2').textContent = plot.title;
      root.querySelector('.chart-title span').textContent = plot.sub;
      drawPlot(root, plot, state);
    }
    PLOTS.forEach(p => {
      const b = document.createElement('button');
      b.type = 'button'; b.role = 'tab'; b.dataset.plot = p.id; b.textContent = p.tab;
      b.addEventListener('click', () => select(p));
      tabs.appendChild(b);
    });
    const fromHash = PLOTS.find(p => '#plot-' + p.id === location.hash);
    select(fromHash || PLOTS[0]);
  }

  document.addEventListener('DOMContentLoaded', () => {
    const bc = document.querySelector('[data-plots]'); if (bc) chartPanel(bc);
  });
})();
