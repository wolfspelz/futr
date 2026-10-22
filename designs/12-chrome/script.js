/* FUTR — Liquid Chrome: tiny vanilla helpers (no dependencies) */
(function () {
  "use strict";

  /* Seamless marquee: duplicate the track content once so translateX(-50%) loops. */
  document.querySelectorAll(".ticker .track, .wanted .track").forEach(function (track) {
    track.innerHTML += track.innerHTML;
  });

  /* Live filter for card grids and lists: input[data-filter="#target"] filters children with data-name. */
  document.querySelectorAll("input[data-filter]").forEach(function (input) {
    var target = document.querySelector(input.getAttribute("data-filter"));
    if (!target) return;
    var items = Array.prototype.slice.call(target.querySelectorAll("[data-name]"));
    var counter = document.querySelector(input.getAttribute("data-count") || "");
    var total = items.length;
    function apply() {
      var q = input.value.trim().toLowerCase();
      var shown = 0;
      items.forEach(function (el) {
        var hit = !q || el.getAttribute("data-name").toLowerCase().indexOf(q) !== -1;
        el.style.display = hit ? "" : "none";
        if (hit) shown++;
      });
      target.setAttribute("data-empty", shown === 0 ? "1" : "0");
      if (counter) counter.textContent = q ? shown + " of " + total : total + "";
    }
    input.addEventListener("input", apply);
    apply();
  });

  /* Whole table row acts as a link when it carries data-href. */
  document.querySelectorAll("tr[data-href]").forEach(function (row) {
    row.addEventListener("click", function (e) {
      if (e.target.closest("a")) return;
      window.location.href = row.getAttribute("data-href");
    });
  });

  /* "/" focuses the first search box. */
  document.addEventListener("keydown", function (e) {
    if (e.key === "/" && !/input|textarea/i.test(document.activeElement.tagName)) {
      var box = document.querySelector("input[data-filter]");
      if (box) { e.preventDefault(); box.focus(); }
    }
  });
})();
