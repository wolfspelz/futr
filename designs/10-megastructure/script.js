/* FUTR Megastructure — small vanilla helpers: filters and "show more". */
(function () {
  'use strict';

  // Text filters: <input data-filter="#selector"> filters children with [data-name]
  document.querySelectorAll('input[data-filter]').forEach(function (input) {
    var root = document.querySelector(input.getAttribute('data-filter'));
    if (!root) return;
    var items = root.querySelectorAll('[data-name]');
    var counter = document.querySelector(input.getAttribute('data-count') || '');
    var empty = document.querySelector(input.getAttribute('data-empty') || '');
    var moreBtn = document.querySelector(input.getAttribute('data-more') || '');

    function apply() {
      var q = input.value.trim().toLowerCase();
      var shown = 0;
      items.forEach(function (el) {
        var hit = !q || el.getAttribute('data-name').toLowerCase().indexOf(q) !== -1;
        el.classList.toggle('is-filtered', !hit);
        if (q) el.classList.remove('is-hidden');
        if (hit) shown++;
      });
      if (counter) counter.textContent = shown;
      if (empty) empty.classList.toggle('show', shown === 0);
      if (moreBtn) moreBtn.style.display = q ? 'none' : '';
    }
    input.addEventListener('input', apply);
  });

  // Show more: <button data-show-more="#tbody">: reveals .is-hidden rows
  document.querySelectorAll('[data-show-more]').forEach(function (btn) {
    var root = document.querySelector(btn.getAttribute('data-show-more'));
    if (!root) return;
    btn.addEventListener('click', function () {
      root.querySelectorAll('.is-hidden').forEach(function (el) { el.classList.remove('is-hidden'); });
      btn.style.display = 'none';
      var note = document.querySelector(btn.getAttribute('data-note') || '');
      if (note) note.textContent = note.getAttribute('data-full') || note.textContent;
    });
  });
})();
