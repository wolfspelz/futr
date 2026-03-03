# FUTR Design Gallery — Briefing Document

This document contains everything needed to pick up design work in a new session.

## What This Is

The `/designs/` folder contains standalone HTML homepage design explorations for [futr.space](https://www.futr.space/). Each design is a single self-contained HTML file (inline CSS/JS, no external dependencies except Google Fonts and Unsplash images). The `index.html` is a gallery page linking to all designs.

## Current State

12 designs exist (#1–#12). Gallery (`index.html`) has cards with CSS-only thumbnails for all 12.

## Team Process

When creating new designs, use a 5-agent team:

| Agent | Role | Model | Mode |
|-------|------|-------|------|
| **scifi-lead** | Creative visionary — proposes concepts | Opus | bypassPermissions |
| **art-director** | Builder — implements HTML/CSS/JS | Opus | bypassPermissions |
| **genz-reviewer** | Culture/vibe critic | Opus | bypassPermissions |
| **ux-expert** | Accessibility specialist | Opus | bypassPermissions |
| **test-engineer** | Functionality checker | Sonnet | bypassPermissions |

### Workflow

1. **TeamCreate** → create 6 tasks with sequential dependencies
2. **Task 1**: scifi-lead proposes concepts (2 per batch)
3. **Task 2**: ALL agents discuss — real back-and-forth debate, not rubber-stamping. Arguments and counter-arguments required.
4. **Task 3**: art-director builds first design
5. **Task 4**: ALL agents review, art-director iterates
6. **Task 5**: art-director builds second design
7. **Task 6**: ALL agents review, art-director iterates, then updates gallery `index.html`
8. Shutdown all agents, TeamDelete

### Recurring Issues (Learnings)

These happen in EVERY session — check proactively:

- **Gallery thumbnail font sizes are ALWAYS too small.** Grep-check all `font-size` values in thumb CSS after every gallery update. Minimum is 0.8125rem.
- **Font sizes below 0.8125rem appear in nearly EVERY design build.** Art-director must be reminded.
- **Art-director sometimes puts h1 outside `<main>`.** Verify every build.
- **Color cross-contamination.** Art-director sometimes borrows exact hex values from other designs. Grep all primary hex values across all design files.
- **Gallery twin risks.** When two designs look too similar in the gallery (same darkness, same accent warmth, same layout shape), they need differentiation. Check during concept discussion.
- **ux-expert sometimes reviews against the WRONG palette.** If a palette changed during discussion, make sure reviewers have the FINAL values.
- **Opacity stacking kills contrast.** If an element has a subdued color AND `opacity < 1`, the compound contrast often fails WCAG AA. ux-expert should check for this.
- **scifi-lead can get stuck.** If no proposals arrive after a few minutes, reassign Task 1 to art-director.
- **Reviewers go idle waiting.** Send explicit nudges when it's time to review.
- **Boot/intro animations need careful accessibility handling.** Cap duration, skip on any key/click/touch, aria-live for screen readers, prefers-reduced-motion must skip entirely (not just slow down).

## Design Requirements

### Content Above the Fold
The first viewport must be rich with actual FUTR data — universe counts, civilization counts, Kardashev range, featured callouts. Not just a hero image with a tagline.

### Grand / Marketing Landing Page Energy
These should feel cinematic, immersive, awe-inspiring. Think sci-fi movie poster meets data dashboard. Not a boring admin panel.

### Accessibility Baseline (MANDATORY — bake in from the start)

- All font sizes minimum 0.8125rem (13px)
- WCAG AA contrast ratios on all text (4.5:1 normal, 3:1 large/18px+ bold)
- `focus-visible` on ALL interactive elements (NOT `:focus`)
- `aria-hidden="true"` on all decorative elements
- `prefers-reduced-motion` disabling ALL animations:
  - CSS wildcard: `*, *::before, *::after { animation-duration: 0.01ms !important; ... }`
  - JS gates: check `matchMedia('(prefers-reduced-motion: reduce)').matches` before any animation init
- `<main>` landmark (proper element, NOT `role="main"` on a div)
- Skip-to-content link (`<a href="#main-content">`)
- `<noscript>` fallback for JS-dependent content
- `fetchpriority="high"` on above-fold images, `loading="lazy"` below fold
- `font-display: swap` in Google Fonts URL (`&display=swap`)
- Explicit `width` and `height` on all `<img>` elements (CLS prevention)
- Heading hierarchy: h1 → h2 (sections) → h3 (items) → h4 (sub-items). h1 INSIDE `<main>`.
- Debounced scroll/resize handlers (rAF ticking or passive listeners)
- `IntersectionObserver` with `.unobserve()` after trigger
- WCAG 1.4.10 reflow: no horizontal scroll at 320px viewport width
- `overflow-x: hidden` on body, `overflow: hidden` on hero containers

## Already Used (DO NOT REPEAT in future designs)

### Fonts (26 used)
Syne, Space Grotesk, Instrument Serif, DM Mono, Unbounded, IBM Plex Sans, Cormorant Garamond, Nunito Sans, Playfair Display, Libre Baskerville, Kalam, Source Sans 3, Bebas Neue, Courier Prime, Crimson Pro, Outfit, Lora, Fira Code, Share Tech Mono, Archivo, Lexend, Overpass Mono, Darker Grotesque, Space Mono, Exo 2, Inconsolata, Rajdhani, Red Hat Mono

### Color Themes (12 used)
1. Dark navy + gold (#080C16 / #C9A84C) — Design #1
2. Light bone + oxidized red (#F2EDE4 / #B04030) — Design #2
3. Dark mineral green + teal (#2B3A33 / #00BFA5) — Design #3
4. Dark midnight + brass (#0B1622 / #C9955D) — Design #4
5. Light newsprint + ink (#F4ECD8 / #1C1C1C) — Design #5
6. Purple-black + bioluminescent (#08060E / #D04890 + #D4982B + #9B68E8) — Design #6
7. Manila + carbon (#D4C5A9 / #1C1C1E) — Design #7
8. Form paper + institutional sage (#F5F2EC / #A8C5B8 + #B03432 + #2E5090) — Design #8
9. Alabaster + india ink (#FAFAF8 / #1A1A1A + #2D6A4F) — Design #9
10. Phosphor + gunmetal (#1B1F23 / #33FF66 + #7B9BB5) — Design #10
11. Void + plasma cyan + thermal orange (#0A0510 / #00D4FF + #FF5722) — Design #11
12. Deep indigo + violet + amber (#08061A / #7C4DFF + #FFD54F) — Design #12

### Metaphors (12 used)
1. Cosmic librarian — #1
2. Radio listening post — #2
3. Geological survey — #3
4. Art Deco observatory — #4
5. Victorian broadsheet newspaper — #5
6. Dark-field mycological culture array — #6
7. Classified intelligence dossier — #7
8. Intergalactic immigration processing terminal — #8
9. Xenological specimen catalog — #9
10. Live orbital tracking station — #10
11. Kardashev event horizon / black hole — #11
12. Ancient autonomous archive ship — #12

### Layout Patterns (12 used)
1. Card grids with per-universe accent colors — #1
2. Frequency bands with signal lock cards — #2
3. Topographic archipelago + core sample column + hex lattice — #3
4. Art Deco gauge panel with brass-framed cards — #4
5. Newspaper multi-column grid + classified ads + market board — #5
6. Organic circular petri dish scatter + SVG mycelium — #6
7. Continuous dossier scroll + redaction bars + threat matrix + file tabs — #7
8. Stacked government form sections + permit cards + rubber stamps — #8
9. Taxonomic classification grid with specimen cards — #9
10. 3×3 sector monitoring grid with contact entries — #10
11. CSS black hole hero + proximity-tier data cards + gravity threshold bar — #11
12. Boot sequence hero + classification-tier archive records + integrity check split-screen — #12

### Design Summaries

| # | Name | Theme | Fonts | Key Feature |
|---|------|-------|-------|-------------|
| 1 | Stellar Cartography | Dark navy+gold | Syne + Space Grotesk | Glassmorphism cards, periodic table metrics |
| 2 | Signal Decay | Light bone+red | Instrument Serif + DM Mono | Garble-to-resolve animation, frequency bands |
| 3 | Tectonic Index | Dark mineral+teal | Unbounded + IBM Plex Sans | Topographic archipelago, core sample column |
| 4 | Grand Observatory | Dark midnight+brass | Cormorant Garamond + Nunito Sans | Art Deco sunburst, brass-framed cards |
| 5 | The FUTR Chronicle | Light newsprint | Playfair Display + Libre Baskerville | Newspaper layout, Kardashev market board |
| 6 | Spore Drift | Purple-black+glow | Lexend + Overpass Mono | Circular petri dishes, SVG mycelium network |
| 7 | REDACTED | Manila+carbon | Bebas Neue + Courier Prime | Redaction bars, rubber stamps, coffee rings |
| 8 | Transit Authority | Form paper+sage | Darker Grotesque + Space Mono | Bureaucratic absurdism, permit cards, stamps |
| 9 | Species | Alabaster+ink | Lora + Fira Code | Taxonomic specimen cards, catalog numbers |
| 10 | Panopticon | Phosphor+gunmetal | Share Tech Mono + Archivo | CRT scanlines, radar sweep, live MET counter |
| 11 | Singularity | Void+cyan+orange | Exo 2 + Inconsolata | CSS black hole, Doppler accretion disk, parallax |
| 12 | Archive Omega | Indigo+violet+amber | Rajdhani + Red Hat Mono | Boot sequence, integrity check, data veins |

## FUTR Data Reference (VERIFIED from YAML files)

### 9 Universes, 30 Civilizations

**Star Trek** (4 civs):
- Vulcans 2063: 1 planet, TL9, Class 10. NO pop, NO Kardashev.
- Andorian Empire 2161: ~8B pop, 14 planets, K0.9, TL9
- Klingon Empire 2161: ~9B pop, ~20 planets, K0.9, TL9
- Federation 2373: ~100B pop, ~150 planets, K1.0, TL10

**Warhammer 40,000** (2 civs):
- Imperium of Man M41: ~1×10¹⁶ pop, ~1M planets, K1.6, TL10
- Orks M41: ~5×10¹⁶ pop, ~2M planets, TL10. NO Kardashev.

**Firefly** (1 civ):
- Alliance 2517: ~303B pop, 215 worlds, K0.95, TL7

**The Culture** (1 civ):
- Culture (pre-war): ~30T pop, ~14,000 Banks Orbitals, 6 Dyson Spheres, K2.3, TL13

**Honorverse** (2 civs):
- Manticore 1903 PD: ~3.2B pop, 3 planets, K1.1, TL9
- Haven 1903 PD: ~200B pop, ~100 planets, K1.0, TL9

**Perry Rhodan** (3 civs):
- Solares Imperium 2040: ~9B pop, 6 planets, K0.9, TL9
- Großes Imperium 2040: ~50T pop, 50,000 planets, K1.6, TL10
- Posbis 2114: 100M units (robotic/AI), K1.3, TL10

**Orion's Arm** (3 civs):
- Terragen Sphere 10600 AT: ~3.9×10¹⁷ pop, TL14. NO Kardashev.
- Solar System 10600 AT: TL13. NO Kardashev.
- Diamond Network 10600 AT: TL14. NO Kardashev.

**The Expanse** (4 civs):
- United Nations 2350: ~30B pop, K0.89
- Mars 2350: ~4B pop, K0.85
- Sol System 2350: ~35B pop, K0.9
- Outer Planets Alliance 2350: ~100M pop, K0.67

**Reality** (10 civs):
- Early Pleistocene: 18,500 pop, K0.057
- Stone Age: 50,000 pop, K0.1
- Bronze Age: 20M pop, K0.36
- Classic Age: 100M pop, K0.46
- Middle Ages: 200M pop, K0.48
- Age of Sail: 800M pop, K0.54
- Industrialization: 1B pop, K0.56
- Mid 20th Century: 1.15B pop, K0.65
- 1980s: 4B pop, K0.7
- 2023: 8.1B pop, K0.73

### K-null Civilizations (5 total — no Kardashev value)
Vulcans 2063, Orks M41, Terragen Sphere 10600 AT, Solar System 10600 AT, Diamond Network 10600 AT

### Key Stats
- Max population: ~3.9×10¹⁷ (Terragen Sphere 10600 AT)
- Max Kardashev: 2.3 (Culture pre-war)
- Min Kardashev: 0.057 (Early Pleistocene)
- Total civilizations: 30
- Total universes: 9

## Unsplash Image URLs (EXACT — never modify photo IDs)

```
Star Trek:    https://images.unsplash.com/photo-1462331940025-496dfbfc7564?auto=format&fit=crop&w=800&h=400&q=80
Warhammer:    https://images.unsplash.com/photo-1534996858221-380b92700493?auto=format&fit=crop&w=800&h=400&q=80
Firefly:      https://images.unsplash.com/photo-1446776811953-b23d57bd21aa?auto=format&fit=crop&w=800&h=400&q=80
The Culture:  https://images.unsplash.com/photo-1464802686167-b939a6910659?auto=format&fit=crop&w=800&h=400&q=80
Honorverse:   https://images.unsplash.com/photo-1454789548928-9efd52dc4031?auto=format&fit=crop&w=800&h=400&q=80
Perry Rhodan: https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&w=800&h=400&q=80
Orion's Arm:  https://images.unsplash.com/photo-1419242902214-272b3f66ee7a?auto=format&fit=crop&w=800&h=400&q=80
The Expanse:  https://images.unsplash.com/photo-1457364559154-aa2644600ebb?auto=format&fit=crop&w=800&h=400&q=80
Reality:      https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&w=800&h=400&q=80
```

## Concept Proposal Template

Each new design concept needs:
1. **Metaphor** — what real-world or sci-fi artifact is this? (NOT from the "already used" list)
2. **Color palette** — 3-4 colors with hex values. Must not overlap with existing themes.
3. **Font pair** — display + data fonts from Google Fonts (NOT from the "already used" list)
4. **Layout** — hero structure, data presentation, section flow
5. **Interaction language** — 3 verbs (e.g., "Descend — Orbit — Transcend")
6. **Why it's GRAND** — what makes someone go "holy shit" when they see it
7. **Gallery twin risk assessment** — which existing designs could it be confused with

## Review Checklist (for every build)

### Data Accuracy (test-engineer)
- [ ] All civilization names spelled correctly
- [ ] All K values match YAML data
- [ ] All population figures match
- [ ] All planet counts match
- [ ] K-null civs correctly identified (5 total)
- [ ] "30 Civilizations · 9 Universes" counts correct
- [ ] Unsplash URLs have correct photo IDs
- [ ] No fabricated stats presented as real data

### Accessibility (ux-expert)
- [ ] All font sizes >= 0.8125rem
- [ ] WCAG AA contrast on all text (calculate with actual hex values)
- [ ] No opacity stacking that compounds to fail contrast
- [ ] focus-visible on all interactive elements
- [ ] aria-hidden on all decorative elements
- [ ] prefers-reduced-motion: CSS wildcard + JS gates
- [ ] h1 inside main
- [ ] Skip-to-content link
- [ ] noscript fallback
- [ ] Image optimization (fetchpriority, loading, width, height)
- [ ] font-display: swap
- [ ] Heading hierarchy (no skips)
- [ ] WCAG 1.4.10 reflow at 320px
- [ ] Print styles

### Creative (scifi-lead + genz-reviewer)
- [ ] Concept matches spec
- [ ] Feels GRAND (not a boring dashboard)
- [ ] No "AI slop" aesthetics
- [ ] Distinct from all other designs in the gallery
- [ ] Data is above the fold
- [ ] Interaction language present

### Code Quality (test-engineer)
- [ ] No fonts from the banned list
- [ ] No borrowed hex values from other designs
- [ ] Clean HTML (no unclosed tags)
- [ ] IntersectionObserver with unobserve
- [ ] Debounced scroll handlers

### Gallery Update
- [ ] Card added to index.html
- [ ] CSS-only thumbnail
- [ ] All thumbnail font sizes >= 0.8125rem
- [ ] Description and tags match the design
