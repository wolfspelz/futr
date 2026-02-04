---
name: myciv
description: Interactive research session to add a new civilization to FUTR
argument-hint: [civilization-spec]
---

# Add Civilization - Interactive Research Session

Research and add a new civilization (polity at a point in time) to the FUTR database.

## Input
The user provides a **civilization specification** - a hint at universe, polity, and time frame.
Examples: "Andorians from Star Trek at Federation founding", "Galactic Empire at fall of Coruscant"

## Workflow

### Step 1: Identifying
1. Research the specification online to verify it's unambiguous
2. Identify: Universe, Polity, Civilization name, Date
3. Check if the universe/polity already exists in `data/universes/`
4. Ask clarifying questions if the specification is ambiguous
5. Confirm the identified civilization with the user

### Step 2: General Research
1. Search for descriptive and metric data about the civilization
2. Find material on: population, territory, technology, government, military, culture
3. Check existing metrics in `data/metrics/` that could apply
4. Propose new metrics if needed (ask user)
5. Present findings and ask which metrics to include

### Step 3: Metrics Research
1. Research specific values for each selected metric
2. Determine confidence levels: canon > calculated > informedGuess > calculatedGuess > wildGuess
3. Find min/max ranges where uncertain
4. **Cross-check values against existing data** (e.g., compare Kardashev to Earth 2023 = 0.73) — these comparisons are for validation only, do not include them in readme fields
5. **In-universe comparisons**: When sources compare to places that share real-world names (e.g., "Sol System", "Earth", "Mars"), assume they refer to the in-universe version at the same time period—not real-world data. Research the in-universe reference to properly contextualize the comparison.
6. Gather reference URLs for all values

### Step 4: Review
1. Present all proposed data in tables:
   - New metrics (if any)
   - Polity info
   - Civilization info
   - Datapoints with values, ranges, confidence, sources AND comparable figures from other civilizations (for validation only—do not include comparisons in final readme fields)
2. Ask user to approve or adjust values
3. Iterate until user is satisfied

### Step 5: Additional Research
1. Find images (from Memory Alpha, wikis, official sources)
2. Write descriptions for polity and civilization readme fields
3. Gather links to official/wiki pages
4. Ensure all info.yaml fields can be populated
5. **Proxy images from Wikimedia/Wikipedia** if they block external embedding (see Image Proxy section below)

### Step 6: Implementation
1. Create directories and info.yaml files:
   - `data/universes/{Universe}/info.yaml` (if new universe — include `tags: [index, new]`)
   - `data/metrics/{NewMetric}/info.yaml` (if new metrics)
   - `data/universes/{Universe}/_polities/{Polity}/info.yaml` (if new polity)
   - `data/universes/{Universe}/{Civilization}/info.yaml` (include `tags: [index, new]`)
   - `data/universes/{Universe}/{Civilization}/{Metric}/info.yaml` (datapoints)
2. Verify all files created
3. Check if `.claude/settings.local.json` was modified (new permissions added during research)
4. Commit all changes together (data files + settings if modified) with message: "add {Civilization} civilization"

## YAML Schemas Reference

See `.claude/rules/content.md` for full schemas. Key fields:

### Timestamps
All info.yaml files MUST have `created` and `changed` date fields (format: `YYYY-MM-DD`):
- **New files**: Set both `created` and `changed` to today's date
- **Modified files**: Update only `changed` to today's date (never change `created`)

**Datapoint:**
```yaml
created: 2026-02-04
changed: 2026-02-04
value: 8e9
min: 5e9
max: 11e9
confidence: informedGuess  # canon|calculated|informedGuess|calculatedGuess|wildGuess
references:
  - link: https://...
    text: "Source name"
readme: |
  Explanation of how value was derived.
```

**Universe (if new):**
```yaml
title: Universe Name
created: 2026-02-04
changed: 2026-02-04
tags: [index, new]  # always add 'new' tag for new universes
tile: https://...  # image for universe tile on home page
images:
  - link: https://...
    text: "Caption"
    page: https://source-page
links:
  - link: https://...
    text: "Official/Wiki link"
readme: |
  Description of the universe.
```

**Civilization:**
```yaml
title: Polity Year
created: 2026-02-04
changed: 2026-02-04
date: 2161
polity: Polity Name
tags: [index, new]  # always add 'new' tag for new civilizations
images:
  - link: https://...
    text: "Caption"
    page: https://source-page
readme: |
  Description of this civilization snapshot.
```

## Quality Checks
- All datapoints MUST have references
- Cross-check Kardashev against Earth 2023 (K=0.73) for sanity
- Use existing metrics when possible before creating new ones
- Images should be from canonical/official sources when available
- **New universes and civilizations MUST have `tags: [index, new]`** — the "new" tag marks recently added content
- **All info.yaml files MUST have `created` and `changed` dates** — use today's date for new files

## Image Proxy

Wikimedia/Wikipedia images often block external embedding. Download these to a local proxy folder:

**Pattern:**
- Original URL: `https://upload.wikimedia.org/wikipedia/commons/d/d5/Example_Image.PNG`
- Local path: `data/proxy/upload.wikimedia.org/wikipedia/commons/d/d5/Example_Image.PNG`
- Reference in YAML: `/proxy/upload.wikimedia.org/wikipedia/commons/d/d5/Example_Image.PNG`

The local path mirrors the original URL structure (preserving slashes).

**Steps:**
1. Create directory: `mkdir -p "data/proxy/{host}/{path}"`
2. Download the image: `curl -L -o "data/proxy/{host}/{path}/{filename}" "{url}"`
3. Reference with `/proxy/...` URL in the `link` field (served by ProxyController)
4. Keep the original Wikimedia Commons page in the `page` field for attribution

**Example:**
```yaml
images:
  - link: /proxy/upload.wikimedia.org/wikipedia/commons/d/d5/The_Honor_Harrington_Universe.PNG
    text: Map of the Honorverse
    page: https://commons.wikimedia.org/wiki/File:The_Honor_Harrington_Universe.PNG
```
