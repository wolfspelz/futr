/* FUTR — Solarpunk Observatory: small vanilla helpers (no dependencies) */
(function () {
  "use strict";
  document.documentElement.classList.add("js");
  const NS = "http://www.w3.org/2000/svg";
  const el = (tag, attrs, parent) => {
    const n = document.createElementNS(NS, tag);
    for (const k in attrs) n.setAttribute(k, attrs[k]);
    if (parent) parent.appendChild(n);
    return n;
  };
  let gid = 0;

  /* ---------- growth rings: data-value, data-max ---------- */
  function growthRings(host) {
    const value = +host.dataset.value, max = +host.dataset.max;
    const other = host.dataset.other ? +host.dataset.other : null;
    const size = 120, c = size / 2, rMin = 9, rMax = 55;
    const svg = el("svg", { viewBox: `0 0 ${size} ${size}`, role: "img", "aria-label": `${value} of ${max}` }, host);
    const id = "rg" + (++gid);
    const defs = el("defs", {}, svg);
    const grad = el("radialGradient", { id, cx: "50%", cy: "50%", r: "50%" }, defs);
    el("stop", { offset: "0%", "stop-color": "#f2c45c" }, grad);
    el("stop", { offset: "100%", "stop-color": "#f8e2a6", "stop-opacity": "0" }, grad);
    if (!host.closest("table")) el("circle", { cx: c, cy: c, r: rMax + 4, fill: `url(#${id})` }, svg);
    const step = (rMax - rMin) / Math.max(1, max - 1);
    const sw = Math.max(1.2, step * 0.55);
    for (let i = 1; i <= max; i++) {
      const r = rMin + (i - 1) * step;
      const filled = i <= value;
      const t = i / max;
      // slightly wobbly rings so they read as organic growth rings
      const wob = 0.9 + 0.08 * Math.sin(i * 2.3);
      const path = ringPath(c, c, r, wob, i);
      el("path", {
        d: path, fill: "none",
        stroke: filled ? mix("#4a7a58", "#1f3d2b", t) : "rgba(31,61,43,0.12)",
        "stroke-width": filled ? sw : sw * 0.7,
        "stroke-linecap": "round",
        opacity: filled ? 0.95 : 1,
        "stroke-dasharray": filled ? "" : "2 3"
      }, svg);
      if (other !== null && i === other) {
        el("path", { d: ringPath(c, c, r, wob, i), fill: "none", stroke: "#b8552f", "stroke-width": sw * 0.9, "stroke-dasharray": "5 4" }, svg);
      }
    }
    // seed core
    el("circle", { cx: c, cy: c, r: 5.5, fill: "#b8552f" }, svg);
    el("circle", { cx: c - 1.5, cy: c - 1.5, r: 1.6, fill: "#f8e2a6" }, svg);
  }
  function ringPath(cx, cy, r, wob, seed) {
    // closed path of 24 points with mild sinusoidal radius variation
    const pts = [];
    for (let k = 0; k < 24; k++) {
      const a = (k / 24) * Math.PI * 2;
      const rr = r * (1 + 0.025 * Math.sin(3 * a + seed) + 0.015 * Math.cos(5 * a - seed));
      pts.push([cx + rr * Math.cos(a), cy + rr * Math.sin(a)]);
    }
    let d = "";
    for (let k = 0; k < pts.length; k++) {
      const p0 = pts[(k - 1 + 24) % 24], p1 = pts[k], p2 = pts[(k + 1) % 24], p3 = pts[(k + 2) % 24];
      if (k === 0) d += `M${p1[0].toFixed(2)} ${p1[1].toFixed(2)}`;
      const c1x = p1[0] + (p2[0] - p0[0]) / 6, c1y = p1[1] + (p2[1] - p0[1]) / 6;
      const c2x = p2[0] - (p3[0] - p1[0]) / 6, c2y = p2[1] - (p3[1] - p1[1]) / 6;
      d += `C${c1x.toFixed(2)} ${c1y.toFixed(2)} ${c2x.toFixed(2)} ${c2y.toFixed(2)} ${p2[0].toFixed(2)} ${p2[1].toFixed(2)}`;
    }
    return d + "Z";
  }
  function mix(a, b, t) {
    const pa = hex(a), pb = hex(b);
    return `rgb(${pa.map((v, i) => Math.round(v + (pb[i] - v) * t)).join(",")})`;
  }
  function hex(h) { return [1, 3, 5].map(i => parseInt(h.slice(i, i + 2), 16)); }

  /* ---------- sun arc gauge: data-value, data-max, optional data-min/data-hi (range), data-other, data-log ---------- */
  function sunArc(host) {
    const value = +host.dataset.value, scaleMax = +host.dataset.max, scaleMin = host.dataset.scaleMin ? +host.dataset.scaleMin : 0;
    const lo = host.dataset.min !== undefined ? +host.dataset.min : null;
    const hi = host.dataset.hi !== undefined ? +host.dataset.hi : null;
    const other = host.dataset.other !== undefined ? +host.dataset.other : null;
    const log = host.dataset.log === "true";
    const label = host.dataset.label || "";
    const W = 180, H = 104, cx = 90, cy = 92, R = 74, R2 = 56;
    const norm = v => {
      if (log) { const a = Math.log10(Math.max(1, scaleMin || 1)), b = Math.log10(scaleMax); return Math.min(1, Math.max(0, (Math.log10(Math.max(1, v)) - a) / (b - a))); }
      return Math.min(1, Math.max(0, (v - scaleMin) / (scaleMax - scaleMin)));
    };
    const pt = (r, f) => { const a = Math.PI * (1 - f); return [cx + r * Math.cos(a), cy - r * Math.sin(a)]; };
    const arcD = (r, f0, f1) => { const [x0, y0] = pt(r, f0), [x1, y1] = pt(r, f1); return `M${x0} ${y0} A${r} ${r} 0 0 1 ${x1} ${y1}`; };
    const svg = el("svg", { viewBox: `0 0 ${W} ${H}`, role: "img", "aria-label": `${label} ${value}` }, host);
    const id = "sa" + (++gid);
    const defs = el("defs", {}, svg);
    const g = el("linearGradient", { id, x1: "0", y1: "0", x2: "1", y2: "0" }, defs);
    el("stop", { offset: "0%", "stop-color": "#f2c45c" }, g);
    el("stop", { offset: "100%", "stop-color": "#d9774f" }, g);
    const glow = el("radialGradient", { id: id + "g", cx: "50%", cy: "100%", r: "60%" }, defs);
    el("stop", { offset: "0%", "stop-color": "#f8e2a6", "stop-opacity": "0.9" }, glow);
    el("stop", { offset: "100%", "stop-color": "#f8e2a6", "stop-opacity": "0" }, glow);
    el("path", { d: arcD(R - 10, 0, 1) + ` L${cx + R - 10} ${cy} Z`, fill: `url(#${id}g)` }, svg);
    // horizon line
    el("line", { x1: 4, y1: cy, x2: W - 4, y2: cy, stroke: "rgba(31,61,43,0.25)", "stroke-width": 1 }, svg);
    // track
    el("path", { d: arcD(R, 0, 1), fill: "none", stroke: "rgba(31,61,43,0.1)", "stroke-width": 10, "stroke-linecap": "round" }, svg);
    // rays (ticks)
    for (let i = 0; i <= 8; i++) {
      const f = i / 8; const [x0, y0] = pt(R + 9, f), [x1, y1] = pt(R + (i % 4 === 0 ? 15 : 12), f);
      el("line", { x1: x0, y1: y0, x2: x1, y2: y1, stroke: "#e3a72f", "stroke-width": 1.5, "stroke-linecap": "round", opacity: 0.8 }, svg);
    }
    // 1-sigma band
    if (lo !== null && hi !== null) {
      el("path", { d: arcD(R, norm(lo), norm(hi)), fill: "none", stroke: "rgba(184,85,47,0.22)", "stroke-width": 16, "stroke-linecap": "butt" }, svg);
    }
    // value arc
    const fv = norm(value);
    if (fv > 0.004) el("path", { d: arcD(R, 0, fv), fill: "none", stroke: `url(#${id})`, "stroke-width": 10, "stroke-linecap": "round" }, svg);
    const [vx, vy] = pt(R, fv);
    el("circle", { cx: vx, cy: vy, r: 7, fill: "#f8f3e8", stroke: "#b8552f", "stroke-width": 3 }, svg);
    // comparison arc (sibling)
    if (other !== null) {
      el("path", { d: arcD(R2, 0, 1), fill: "none", stroke: "rgba(31,61,43,0.08)", "stroke-width": 5, "stroke-linecap": "round" }, svg);
      const fo = norm(other);
      if (fo > 0.004) el("path", { d: arcD(R2, 0, fo), fill: "none", stroke: "#2f5a3e", "stroke-width": 5, "stroke-linecap": "round", opacity: 0.8 }, svg);
      const [ox, oy] = pt(R2, fo);
      el("circle", { cx: ox, cy: oy, r: 4, fill: "#2f5a3e" }, svg);
    }
    // value text
    const t = el("text", { x: cx, y: cy - 8, "text-anchor": "middle", "font-family": "Fraunces, Georgia, serif", "font-size": 24, "font-weight": 500, fill: "#1f3d2b" }, svg);
    t.textContent = host.dataset.display || value;
    const t2 = el("text", { x: 8, y: cy + 14, "font-family": "Nunito Sans, sans-serif", "font-size": 9, "font-weight": 700, fill: "#5c6a5e" }, svg);
    t2.textContent = host.dataset.lowLabel || (log ? fmt(scaleMin || 1) : fmt(scaleMin));
    const t3 = el("text", { x: W - 8, y: cy + 14, "text-anchor": "end", "font-family": "Nunito Sans, sans-serif", "font-size": 9, "font-weight": 700, fill: "#5c6a5e" }, svg);
    t3.textContent = host.dataset.highLabel || fmt(scaleMax);
  }
  function fmt(v) {
    if (v >= 1e9) return (v / 1e9).toFixed(v % 1e9 ? 1 : 0).replace(/\.0$/, "") + " bn";
    if (v >= 1e6) return (v / 1e6).toFixed(v % 1e6 ? 1 : 0).replace(/\.0$/, "") + " M";
    if (v >= 1e3) return (v / 1e3).toFixed(v % 1e3 ? 1 : 0).replace(/\.0$/, "") + " k";
    return String(v);
  }

  /* ---------- range bar: data-min, data-value, data-max, data-lo, data-hi (scale bounds) ---------- */
  function rangeBar(host) {
    const min = +host.dataset.min, max = +host.dataset.max, value = +host.dataset.value;
    const span = max - min || 1;
    const lo = host.dataset.lo !== undefined ? +host.dataset.lo : min - span * 0.6;
    const hi = host.dataset.hi !== undefined ? +host.dataset.hi : max + span * 0.6;
    const W = 320, H = 34, pad = 10, y = 18;
    const X = v => pad + ((v - lo) / (hi - lo)) * (W - pad * 2);
    const svg = el("svg", { viewBox: `0 0 ${W} ${H}`, role: "img", "aria-label": `${value}, 1-sigma range ${min} to ${max}` }, host);
    // stem (track)
    el("line", { x1: pad, y1: y, x2: W - pad, y2: y, stroke: "rgba(31,61,43,0.14)", "stroke-width": 2, "stroke-linecap": "round" }, svg);
    // leaf-shaped band
    const a = X(min), b = X(max), m = X(value);
    el("path", { d: `M${a} ${y} Q${(a + m) / 2} ${y - 9} ${m} ${y - 9} Q${(m + b) / 2} ${y - 9} ${b} ${y} Q${(m + b) / 2} ${y + 9} ${m} ${y + 9} Q${(a + m) / 2} ${y + 9} ${a} ${y}Z`, fill: "rgba(157,184,90,0.45)", stroke: "#4a7a58", "stroke-width": 1 }, svg);
    el("line", { x1: a, y1: y, x2: b, y2: y, stroke: "#4a7a58", "stroke-width": 1, "stroke-dasharray": "2 2" }, svg);
    el("circle", { cx: m, cy: y, r: 6, fill: "#b8552f", stroke: "#f8f3e8", "stroke-width": 2 }, svg);
    const tl = el("text", { x: a, y: y + 15, "text-anchor": "middle", "font-family": "Nunito Sans, sans-serif", "font-size": 9, "font-weight": 700, fill: "#5c6a5e" }, svg);
    tl.textContent = host.dataset.minLabel || fmt(min);
    const th = el("text", { x: b, y: y + 15, "text-anchor": "middle", "font-family": "Nunito Sans, sans-serif", "font-size": 9, "font-weight": 700, fill: "#5c6a5e" }, svg);
    th.textContent = host.dataset.maxLabel || fmt(max);
  }

  /* ---------- reveal on scroll ---------- */
  function reveal() {
    const items = document.querySelectorAll(".rise");
    if (!("IntersectionObserver" in window)) { items.forEach(i => i.classList.add("in")); return; }
    const io = new IntersectionObserver(es => es.forEach(e => { if (e.isIntersecting) { e.target.classList.add("in"); io.unobserve(e.target); } }), { rootMargin: "0px 0px -8% 0px" });
    items.forEach(i => io.observe(i));
    setTimeout(() => items.forEach(i => i.classList.add("in")), 1200);
  }

  /* ---------- "and N more" toggles ---------- */
  function moreToggles() {
    document.querySelectorAll("[data-more]").forEach(btn => {
      const list = document.getElementById(btn.dataset.more);
      if (!list) return;
      const hidden = list.querySelectorAll(".hidden");
      const n = hidden.length;
      if (!n) { btn.remove(); return; }
      const open = btn.dataset.openLabel || `… and ${n} more`;
      const close = btn.dataset.closeLabel || "Show fewer";
      btn.textContent = open;
      btn.addEventListener("click", () => {
        const expanded = btn.getAttribute("aria-expanded") === "true";
        hidden.forEach(h => h.classList.toggle("hidden", expanded));
        btn.setAttribute("aria-expanded", String(!expanded));
        btn.textContent = expanded ? open : close;
      });
    });
  }

  /* ---------- text filters: input[data-filter="#id"] filters children with [data-name] ---------- */
  function filters() {
    document.querySelectorAll("input[data-filter]").forEach(inp => {
      const target = document.querySelector(inp.dataset.filter);
      const counter = inp.dataset.count ? document.querySelector(inp.dataset.count) : null;
      if (!target) return;
      const rows = target.querySelectorAll("[data-name]");
      const total = rows.length;
      const apply = () => {
        const q = inp.value.trim().toLowerCase();
        let shown = 0;
        rows.forEach(r => { const ok = !q || r.dataset.name.toLowerCase().includes(q); r.style.display = ok ? "" : "none"; if (ok) shown++; });
        if (counter) counter.textContent = q ? `${shown} of ${total}` : `${total}`;
      };
      inp.addEventListener("input", apply);
    });
  }

  document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll("[data-rings]").forEach(growthRings);
    document.querySelectorAll("[data-arc]").forEach(sunArc);
    document.querySelectorAll("[data-range]").forEach(rangeBar);
    reveal(); moreToggles(); filters();
  });
})();
