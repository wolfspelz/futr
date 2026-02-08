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
5. **Ask the user which language** all texts (readme fields, descriptions, image captions) should be written in (e.g., English or German)
6. Confirm the identified civilization and language with the user

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
4. **Cross-check values against existing data** (e.g., compare Kardashev to Earth 2023 = 0.73 and other civiliztions, but also show other comparisons of other metrics) — these comparisons are for validation only, do not include them in readme fields
5. **In-universe comparisons**: When sources compare to places that share real-world names (e.g., "Sol System", "Earth", "Mars"), assume they refer to the in-universe version at the same time period—not real-world data. Research the in-universe reference to properly contextualize the comparison.
6. Gather reference URLs for all values

### Step 4: Review
1. Present all proposed data in tables:
   - New metrics (if any)
   - Polity info
   - Civilization info
   - **Datapoints table with comparisons** (see format below)
2. Ask user to approve or adjust values
3. Iterate until user is satisfied

**Datapoints Comparison Table Format:**
For EVERY datapoint, show comparable values from existing civilizations to help maintain consistency, for example (apply this to all shown metrics):

| Metric | Value | Confidence | Comparisons (same universe) | Comparisons (other universes) |
|--------|-------|------------|----------------------------|-------------------------------|
| Population | 8e9 | canon | Empire 2040: 25e9 | Earth 2023: 8e9, Federation 2373: 1e12 |
| Kardashev | 0.85 | calculated | Empire 2040: 1.2 | Earth 2023: 0.73, Federation 2373: 2.1 |
| Planets | 12 | canon | Empire 2040: 150 | Federation 2373: 150 |

- Query existing civilizations in `data/universes/` for comparison data
- Prioritize comparisons from the same universe first
- Include Earth 2023 as baseline reference where applicable
- These comparisons are for validation only—do NOT include them in readme fields or files

**Kardashev Calculation Rationale (REQUIRED):**
For every Kardashev value, show the calculation rationale including per-person power consumption:

1. **Calculate total power**: Estimate civilization's total energy consumption in Watts
2. **Calculate per-person power**: Total power ÷ Population = Watts per person
3. **Compare to Earth 2023 baseline**: Earth uses ~18 TW for 8.1e9 people = ~2,200 W/person
4. **Apply Kardashev formula**: K = log₁₀(Power) / 10 - 0.6 (where Power is in Watts)
   - Type I = 10^16 W (~1.2e6 W/person for 8e9 people)
   - Type II = 10^26 W (full stellar output)
5. **Show the math**: Present the calculation transparently so it can be verified

Example rationale:
```
Population: 50e12, Estimated power: 10^24 W
Per-person: 10^24 / 50e12 = 2e10 W/person (vs Earth 2023: 2,200 W/person → 9 million× more)
Kardashev: log₁₀(10^24) / 10 - 0.6 = 24/10 - 0.6 = 1.8
```

### Step 5: Image Research & Attribution (REQUIRED)
1. Find images (from Wikimedia Commons, Wikipedia, official sources)
2. **For EVERY image, you MUST research and document all attribution fields:**
   - `src`: Direct image URL (or `/proxy/...` path for downloaded images)
   - `link`: Source page URL for attribution (e.g., Wikimedia Commons file page, publisher page)
   - `author`: Creator/artist name (search for the artist - e.g., book cover artists, map creators)
   - `license`: Short identifier (e.g., "CC BY-SA 4.0", "Public Domain", "© Publisher" for copyrighted works)
   - `legal`: Full license text or URL (e.g., "https://creativecommons.org/licenses/by-sa/4.0/" or "Copyrighted by [Publisher]. Used for commentary under Fair Use.")
3. **Research strategy for attribution:**
   - Wikimedia Commons: Fetch the file page to find author and license info
   - Book covers: Search for "[book title] cover artist" - publishers like Baen often use known artists
   - Fan wiki images: Check the file page for upload info and licensing
   - Official art: Note the copyright holder (studio, publisher) and use Fair Use rationale
4. **If attribution is incomplete**, ask the user whether to: (a) add the image anyway with "Unknown" for missing fields, (b) skip the image, or (c) search for a different image
5. Write descriptions for polity and civilization readme fields
6. Gather links to official/wiki pages
7. **Proxy images from Wikimedia/Wikipedia** if they block external embedding (see Image Proxy section below)

### Step 6: Implementation
1. Create directories and info.yaml files:
   - `data/universes/{Universe}/info.yaml` (if new universe — include `tags: [index, new]`)
   - `data/metrics/{NewMetric}/info.yaml` (if new metrics)
   - `data/universes/{Universe}/_polities/{Polity}/info.yaml` (if new polity)
   - `data/universes/{Universe}/{Civilization}/info.yaml` (include `tags: [index, new]`)
   - `data/universes/{Universe}/{Civilization}/{Metric}/info.yaml` (datapoints)
2. **Update `showcaseMetrics` in Universe info.yaml** (REQUIRED):
   - Check which metrics have datapoints across multiple civilizations in this universe
   - Update `showcaseMetrics` list to include all metrics that enable comparisons
   - This ensures the comparison table shows maximum data
   - Example: `showcaseMetrics: [Population, Kardashev, Planets, Star Systems, Warships]`
3. Verify all files created
4. Check if `.claude/settings.local.json` was modified (new permissions added during research)
5. Commit all changes together (data files + settings if modified) with message: "add {Civilization} civilization"

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
images:
  - src: https://...           # Direct image URL
    text: "Caption"
    link: https://source-page  # Source page for attribution
    author: "Artist Name"      # REQUIRED: Research the actual artist
    license: "CC BY-SA 4.0"    # REQUIRED: License identifier
    legal: "https://creativecommons.org/licenses/by-sa/4.0/"  # REQUIRED: License URL or Fair Use text
    tags: [main]               # "main" = primary/tile image
links:
  - src: https://...
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
  - src: https://...           # Direct image URL
    text: "Caption"
    link: https://source-page  # Source page for attribution
    author: "Artist Name"      # REQUIRED: Research the actual artist
    license: "CC BY-SA 4.0"    # REQUIRED: License identifier
    legal: "https://creativecommons.org/licenses/by-sa/4.0/"  # REQUIRED: License URL or Fair Use text
    tags: [main]               # "main" = primary/tile image
readme: |
  Description of this civilization snapshot.
```

## Writing Style
- **All descriptions and readme texts MUST be written in present tense** (historisches Präsens / historical present), regardless of language. Example: "Das Imperium umfasst 50.000 Welten" (not "umfasste"), "The Empire controls 150 worlds" (not "controlled").
- **Never use em dashes** (—). Use commas, periods, or restructure sentences instead.
- **In-universe perspective**: Write all descriptions (universe, polity, civilization readmes) from an in-universe point of view, as if recorded by an accomplished historian within that universe. No publishing details, no meta-references to books, episodes, or authors. Focus on events, peoples, politics, and technology as historical facts.
- Write in the language confirmed with the user in Step 1.

## Quality Checks
- All datapoints MUST have references
- Cross-check Kardashev against Earth 2023 (K=0.73) for sanity
- Use existing metrics when possible before creating new ones
- **Always update `showcaseMetrics`** in universe info.yaml to include all metrics with datapoints — this enables comparison tables
- Images should be from canonical/official sources when available
- **All images MUST have complete attribution** — `src`, `link`, `author`, `license`, `legal` are ALL required fields
  - `author`: Research the actual creator (artist name, photographer, etc.) — do not skip this step
  - `license`: Use standard identifiers like "CC BY-SA 4.0", "Public Domain", "© [Publisher]"
  - `legal`: Provide license URL or Fair Use rationale for copyrighted works
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
3. Reference with `/proxy/...` URL in the `src` field (served by ProxyController)
4. Keep the original Wikimedia Commons page in the `link` field for attribution

**Example:**
```yaml
images:
  - src: /proxy/upload.wikimedia.org/wikipedia/commons/d/d5/The_Honor_Harrington_Universe.PNG
    text: Map of the Honorverse
    link: https://commons.wikimedia.org/wiki/File:The_Honor_Harrington_Universe.PNG
    author: "Michał Świerczek"  # Research the actual author from Wikimedia Commons page
    license: "CC BY-SA 3.0"
    legal: "https://creativecommons.org/licenses/by-sa/3.0/"
```
