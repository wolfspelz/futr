---
paths:
  - "data/**/*"
---

# Content Structure & Organization

## Data Directory Structure
```
data/
├── metrics/{MetricName}/info.yaml
├── proxy/{host}/{path}/{filename}          # Proxied images (see Image Proxy)
└── universes/{UniverseName}/
    ├── info.yaml
    ├── _polities/{PolityName}/info.yaml
    └── {CivilizationName}/
        ├── info.yaml
        └── {MetricName}/info.yaml  (datapoint)
```

## Naming Conventions
- **Universe folder**: Universe name (e.g., `Star Trek`, `Orions Arm`)
- **Civilization folder**: `{PolityOrCivName} {Year}` (e.g., `Federation 2373`)
- **Polity folder**: `_polities/{PolityName}` (e.g., `_polities/Federation of Planets`)
- **Metric datapoint folder**: Must match an existing metric name in `data/metrics/`
- **Civilization `date` field**: Must be parseable as a year

## Timestamps
All info.yaml files MUST have `created` and `changed` date fields (format: `YYYY-MM-DD`):
- **New files**: Set both `created` and `changed` to today's date
- **Modified files**: Update only `changed` to today's date (never change `created`)

## YAML Schemas

### Metric (`data/metrics/{MetricName}/info.yaml`)
```yaml
title: Population              # Display name
created: 2026-02-04
changed: 2026-02-04
type: number                   # Data type
unit: People                   # Unit of measurement
range: "0-1e12"                # Optional range
tags: [index, fav]             # index=show in index, fav=featured
order: 0.0                     # Sort order (lower = earlier)
icons: [https://...]           # Icon URLs
images:
  - src: https://...           # Direct image URL
    text: "Caption"            # Image description
    link: https://source       # Source page URL (required)
    author: "Artist Name"      # Creator/artist (required if known)
    license: "CC BY-SA 4.0"    # Short license identifier
    legal: "https://creativecommons.org/licenses/by-sa/4.0/"
    tags: [main]               # "main" = primary/tile image for lists
links:
  - src: https://...
    text: "Link text"
references:
  - src: https://...
    text: "Reference text"
editors: [github_username]
readme: |
  Markdown description
```

### Universe (`data/universes/{UniverseName}/info.yaml`)
```yaml
title: Star Trek
created: 2026-02-04
changed: 2026-02-04
tags: [index, fav]
order: -1000                   # Negative = sort earlier
showcaseMetrics: [Population, Planets, Kardashev]  # Metrics shown in tables
icons: [https://...]
images:
  - src: https://...           # Direct image URL
    text: "Caption"            # Image description
    link: https://source       # Source page URL (required)
    author: "Artist Name"      # Creator/artist (required if known)
    license: "CC BY-SA 4.0"    # Short license identifier
    legal: "Full license text or URL"
    tags: [main]               # "main" = primary/tile image for lists
links:
  - link: https://official-site.com
    text: "Official Website"
editors: [github_username]
readme: |
  Markdown description
```

### Polity (`data/universes/{Universe}/_polities/{PolityName}/info.yaml`)
```yaml
title: Federation of Planets
created: 2026-02-04
changed: 2026-02-04
tags: [index]
order: 0.0
images:
  - src: https://...           # Direct image URL
    text: "Caption"            # Image description
    link: https://source       # Source page URL (required)
    author: "Artist Name"      # Creator/artist (required if known)
    license: "CC BY-SA 4.0"    # Short license identifier
    legal: "Full license text or URL"
    tags: [main]               # "main" = primary/tile image for lists
links:
  - src: https://...
    text: "Wiki link"
editors: [github_username]
readme: |
  Markdown description
```

### Civilization (`data/universes/{Universe}/{CivilizationName}/info.yaml`)
```yaml
title: Federation 2373         # Display name (often includes year)
created: 2026-02-04
changed: 2026-02-04
date: 2373                     # Year or time period (required)
polity: Federation of Planets  # Reference to polity folder name (optional)
tags: [index, fav]
order: 100.0
images:
  - src: https://...           # Direct image URL
    text: "Caption"            # Image description
    link: https://source       # Source page URL (required)
    author: "Artist Name"      # Creator/artist (required if known)
    license: "CC BY-SA 4.0"    # Short license identifier
    legal: "Full license text or URL"
    tags: [main]               # "main" = primary/tile image for lists
links:
  - src: https://...
    text: "Link text"
editors: [github_username]
readme: |
  Markdown description
```

### Datapoint (`data/universes/{Universe}/{Civilization}/{Metric}/info.yaml`)
```yaml
created: 2026-02-04
changed: 2026-02-04
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
  - src: https://...           # Direct image URL
    text: "Supporting image"   # Image description
    link: https://source       # Source page URL (required)
    author: "Artist Name"      # Creator/artist (required if known)
    license: "CC BY-SA 4.0"    # Short license identifier
    legal: "Full license text or URL"
    tags: [main]               # "main" = primary/tile image for lists
editors: [github_username]
readme: |
  Explain HOW the value was derived.
  Include calculations if applicable.
```

### Confidence Levels

Ordered from most to least reliable:

- `canon` - Directly stated in official primary canon sources (movies, TV shows, published novels in the main series). The value is explicitly given, not interpreted.
- `semiCanon` - From secondary canon sources that are not fully authoritative: RPG sourcebooks, technical manuals, creator interviews, companion guides, decanonized material (e.g., Star Wars Legends), or spinoff series with ambiguous canon status.
- `calculated` - Mathematically derived from canon or semiCanon values using straightforward formulas. The inputs are reliable, only the derivation step is added.
- `informedGuess` - Educated estimate based on substantial research. No direct source states the value, but multiple contextual clues point toward it.
- `calculatedGuess` - A calculation where one or more inputs are themselves guesses. The math is sound but the assumptions are uncertain.
- `wildGuess` - Rough order-of-magnitude estimation with little supporting evidence. Used when a value is needed but sources are scarce or contradictory.

## Image Sources

**Preferred sources:**
1. **Wikimedia Commons** - Free, embeddable, well-licensed images
2. **Wikipedia** - Direct image links (upload.wikimedia.org)
3. **Official promotional images** - Press kits, official sites with permissive policies
4. **Internet Archive** - Historical images, screenshots

**Never use:**
- Cosplay or fan costume images
- Fandom wiki image URLs for embedding (they block external embedding via CSP)

**Fandom wikis are fine for:**
- `links` and `references` fields (text links, not embedded images)
- Research and fact-checking

**Image requirements:**
- Verify the image can be embedded (test from different origin)
- All images MUST have complete attribution: `src`, `link`, `author`, `license`, `legal`

## Image Proxy

Wikimedia/Wikipedia images often block external embedding. Download these to a local proxy folder.

**Pattern:**
- Original URL: `https://upload.wikimedia.org/wikipedia/commons/d/d5/Example_Image.PNG`
- Local path: `data/proxy/upload.wikimedia.org/wikipedia/commons/d/d5/Example_Image.PNG`
- Reference in YAML: `/proxy/upload.wikimedia.org/wikipedia/commons/d/d5/Example_Image.PNG`

The local path mirrors the original URL structure (preserving slashes).

**Steps:**
1. Create directory: `mkdir -p "data/proxy/{host}/{path}"`
2. Download the image: `curl -L -o "data/proxy/{host}/{path}/{filename}" "{url}"`
3. Reference with `/proxy/...` URL in the `src` field (served by ProxyController)
4. Keep the original Wikimedia Commons page in the `link` field for attribution

**Example:**
```yaml
images:
  - src: /proxy/upload.wikimedia.org/wikipedia/commons/d/d5/The_Honor_Harrington_Universe.PNG
    text: Map of the Honorverse
    link: https://commons.wikimedia.org/wiki/File:The_Honor_Harrington_Universe.PNG
    author: "Michał Świerczek"
    license: "CC BY-SA 3.0"
    legal: "https://creativecommons.org/licenses/by-sa/3.0/"
```
