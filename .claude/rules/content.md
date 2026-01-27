---
paths:
  - "data/**/*"
---

# Content & Research

## Conceptual Model

**Universes** contain **factions** (named peoples, planets, or empires). Factions can range from galaxy-spanning civilizations to a single individual—it depends on the universe, your interests, and available data.

A **civilization** is a faction at a specific point in time. Since factions change over time, we capture snapshots as civilizations (e.g., "Federation 2373" is the Federation of Planets in the year 2373).

**Metrics** describe civilizations with measurable data.

**Example:** Star Trek (universe) → Federation of Planets (faction) → Federation 2373 (civilization) → 150 member worlds (metric: Planets)

## Data Directory Structure
```
data/
├── metrics/{MetricName}/info.yaml
└── universes/{UniverseName}/
    ├── info.yaml
    ├── _factions/{FactionName}/info.yaml
    └── {CivilizationName}/
        ├── info.yaml
        └── {MetricName}/info.yaml  (datapoint)
```

## YAML Schemas

### Metric (`data/metrics/{MetricName}/info.yaml`)
```yaml
title: Population              # Display name
type: number                   # Data type
unit: People                   # Unit of measurement
range: "0-1e12"                # Optional range
tags: [index, fav]             # index=show in index, fav=featured
order: 0.0                     # Sort order (lower = earlier)
tile: https://example.com/img  # Main image URL
icons: [https://...]           # Icon URLs
images:
  - link: https://...
    text: "Caption"
    page: https://source
links:
  - link: https://...
    text: "Link text"
references:
  - link: https://...
    text: "Reference text"
editors: [github_username]
readme: |
  Markdown description
```

### Universe (`data/universes/{UniverseName}/info.yaml`)
```yaml
title: Star Trek
tags: [index, fav]
order: -1000                   # Negative = sort earlier
tile: https://...
showcaseMetrics: [Population, Planets, Kardashev]  # Metrics shown in tables
icons: [https://...]
images:
  - link: https://...
    text: "Caption"
    page: https://source
links:
  - link: https://official-site.com
    text: "Official Website"
editors: [github_username]
readme: |
  Markdown description
```

### Faction (`data/universes/{Universe}/_factions/{FactionName}/info.yaml`)
```yaml
title: Federation of Planets
tags: [index]
order: 0.0
images:
  - link: https://...
    text: "Caption"
links:
  - link: https://...
    text: "Wiki link"
editors: [github_username]
readme: |
  Markdown description
```

### Civilization (`data/universes/{Universe}/{CivilizationName}/info.yaml`)
```yaml
title: Federation 2373         # Display name (often includes year)
date: 2373                     # Year or time period (required)
faction: Federation of Planets # Reference to faction folder name (optional)
tags: [index, fav]
order: 100.0
images:
  - link: https://...
    text: "Caption"
links:
  - link: https://...
    text: "Link text"
editors: [github_username]
readme: |
  Markdown description
```

### Datapoint (`data/universes/{Universe}/{Civilization}/{Metric}/info.yaml`)
```yaml
value: 150                     # Main measured value (required)
min: 100                       # Lower bound (1 std deviation)
max: 200                       # Upper bound (1 std deviation)
confidence: canon              # See confidence levels below
tags: [index]
order: 0.0
references:                    # REQUIRED: must cite sources
  - link: https://en.wikipedia.org/...
    text: "Wikipedia - Source"
  - link: https://memory-alpha.fandom.com/...
    text: "Memory Alpha"
images:
  - link: https://...
    text: "Supporting image"
editors: [github_username]
readme: |
  Explain HOW the value was derived.
  Include calculations if applicable.
```

### Confidence Levels
- `canon` - From official canon sources (movies, shows, books)
- `calculated` - Mathematically derived from other known values
- `informedGuess` - Educated estimate based on research
- `calculatedGuess` - Calculation with uncertain assumptions
- `wildGuess` - Rough estimation with limited basis

## Guidelines for Agentic Research

### Adding New Universes
1. Research the universe thoroughly (official wikis, fan wikis, source material)
2. Create folder: `data/universes/{UniverseName}/`
3. Create `info.yaml` with title, images, links to official sources
4. Create `_factions/` subfolder for major political/cultural groups
5. Create civilization snapshots at key historical moments

### Adding Civilizations
1. Choose meaningful time points (major events, peak eras, transitions)
2. Folder name convention: `{FactionOrCivName} {Year}` (e.g., "Federation 2373")
3. Must have `date` field that can be parsed as a year
4. Reference faction if applicable

### Adding Datapoints
1. **Always cite references** - every value needs sources
2. Prefer canon > calculated > informedGuess > wildGuess
3. Include min/max bounds when uncertain
4. Explain reasoning in readme field
5. Metric folder name must match existing metric in `data/metrics/`

### Research Sources (by priority)
1. **Official sources** - Creator-sanctioned content (Memory Alpha, Wookieepedia verified)
2. **Fan wikis** - Memory Alpha, Wookieepedia, various fandom wikis
3. **Original works** - Books, shows, movies, games
4. **Scholarly analysis** - Academic papers on fictional universes
5. **Fan calculations** - Well-reasoned community estimates

### Quality Standards
- All values must have at least one reference
- Calculations should be shown in readme
- Uncertainty should be reflected in min/max and confidence level
- Use consistent units across the project
- Images should have proper attribution (text + page fields)

### Image Sources
Fandom wikis (Memory Alpha, Wookieepedia, etc.) block external image embedding via CSP headers. **Do NOT use Fandom image URLs** for `tile`, `icons`, or `images` fields.

**Preferred image sources:**
1. **Wikimedia Commons** - Free, embeddable, well-licensed images
2. **Wikipedia** - Direct image links (upload.wikimedia.org)
3. **Official promotional images** - Press kits, official sites with permissive policies
4. **Internet Archive** - Historical images, screenshots

**Never use:**
- Cosplay or fan costume images

**Always:**
- Verify the image can be embedded (test in browser from different origin)
- Prefer PNG/JPG over WebP for compatibility
- Include `page` field linking to the source page for attribution

**Fandom wikis are fine for:**
- `links` and `references` fields (text links, not embedded images)
- Research and fact-checking

### Common Metrics to Research
For any civilization, try to find:
1. Population (total, and breakdown by type if available)
2. Kardashev level (energy usage, technological capability)
3. Number of inhabited planets/worlds
4. Territory size (if measurable)
5. Age of civilization
6. Key dates (founding, major events)
