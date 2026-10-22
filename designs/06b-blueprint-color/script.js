/* FUTR Blueprint — minimal vanilla JS */
(function () {
  var root = document.documentElement;

  /* Paper mode toggle (white paper, blue ink) */
  try {
    var saved = localStorage.getItem('futr-bp-theme');
    if (saved === 'paper') root.setAttribute('data-theme', 'paper');
  } catch (e) {}

  function syncToggle() {
    var on = root.getAttribute('data-theme') === 'paper';
    document.querySelectorAll('[data-toggle-theme]').forEach(function (b) {
      b.textContent = on ? 'Blueprint mode' : 'Paper mode';
      b.setAttribute('aria-pressed', on ? 'true' : 'false');
    });
  }
  document.addEventListener('click', function (ev) {
    var b = ev.target.closest('[data-toggle-theme]');
    if (!b) return;
    var on = root.getAttribute('data-theme') === 'paper';
    if (on) root.removeAttribute('data-theme'); else root.setAttribute('data-theme', 'paper');
    try { localStorage.setItem('futr-bp-theme', on ? 'blueprint' : 'paper'); } catch (e) {}
    syncToggle();
  });

  /* Image fallback: swap to generated placeholder on error (capture phase catches img errors) */
  document.addEventListener('error', function (ev) {
    var img = ev.target;
    if (!(img instanceof HTMLImageElement) || img.classList.contains('fallback')) return;
    img.classList.add('fallback');
    img.src = 'placeholder.svg';
  }, true);

  /* Expand / collapse long lists */
  document.addEventListener('click', function (ev) {
    var b = ev.target.closest('[data-expand]');
    if (!b) return;
    var list = document.getElementById(b.getAttribute('data-expand'));
    if (!list) return;
    var open = list.classList.toggle('expanded');
    b.textContent = open ? b.getAttribute('data-less') : b.getAttribute('data-more');
  });

  /* Text filter: data-filter="#container" filters children with data-name */
  document.addEventListener('input', function (ev) {
    var inp = ev.target.closest('[data-filter]');
    if (!inp) return;
    var q = inp.value.trim().toLowerCase();
    var box = document.querySelector(inp.getAttribute('data-filter'));
    if (!box) return;
    var n = 0;
    box.querySelectorAll('[data-name]').forEach(function (el) {
      var hit = !q || el.getAttribute('data-name').toLowerCase().indexOf(q) !== -1;
      el.style.display = hit ? '' : 'none';
      if (hit) n++;
    });
    var out = document.querySelector('[data-filter-count="' + inp.getAttribute('data-filter') + '"]');
    if (out) out.textContent = n;
  });

  /* Sort parts table by clicking a header (numeric aware, uses data-v) */
  document.addEventListener('click', function (ev) {
    var th = ev.target.closest('th[data-sort]');
    if (!th) return;
    var table = th.closest('table'), tbody = table.tBodies[0];
    var idx = Array.prototype.indexOf.call(th.parentNode.children, th);
    var dir = th.getAttribute('aria-sort') === 'ascending' ? -1 : 1;
    table.querySelectorAll('th').forEach(function (h) { h.removeAttribute('aria-sort'); });
    th.setAttribute('aria-sort', dir === 1 ? 'ascending' : 'descending');
    var rows = Array.prototype.slice.call(tbody.rows);
    rows.sort(function (a, b) {
      var ca = a.cells[idx], cb = b.cells[idx];
      var va = ca.getAttribute('data-v'), vb = cb.getAttribute('data-v');
      if (va !== null && vb !== null) return (parseFloat(va) - parseFloat(vb)) * dir;
      return ca.textContent.localeCompare(cb.textContent) * dir;
    });
    rows.forEach(function (r) { tbody.appendChild(r); });
  });

  document.addEventListener('DOMContentLoaded', syncToggle);
})();
