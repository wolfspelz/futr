---
name: myciv
description: Interactive research session to add a new civilization to FUTR
argument-hint: [civilization-spec]
---

# Add Civilization - Interactive Research Session

Research and add a new civilization (polity at a point in time) to the FUTR database.

**IMPORTANT: At the start of every session, read `.claude/rules/content.md` for YAML schemas, directory structure, naming conventions, and image policies.**

## Input
The user provides a **civilization specification** - a hint at universe, polity, and time frame.
Examples: "Andorians from Star Trek at Federation founding", "Galactic Empire at fall of Coruscant"

## Workflow

### Step 1: Identifying
1. **Read `.claude/rules/content.md`** for schemas and structural conventions
2. Research the specification online to verify it's unambiguous
3. Identify: Universe, Polity, Civilization name, Date
4. Check if the universe/polity already exists in `data/universes/`
5. Ask clarifying questions if the specification is ambiguous
6. Ask the user which language all texts (readme fields, descriptions, image captions) should be written in (e.g., English or German)
7. Confirm the identified civilization and language with the user

### Step 2: General Research
1. Search for descriptive and metric data about the civilization
2. Find material on: population, territory, technology, government, military, culture
3. Check existing metrics in `data/metrics/` that could apply
4. Propose new metrics if needed (ask user)
5. Present findings and ask which metrics to include

### Step 3: Metrics Research
1. Research specific values for each selected metric
2. Determine confidence levels
3. Find min/max ranges where uncertain
   - **Canon/semiCanon values**: Consider whether min/max are needed. If the value is exact and certain, omit min and max. If the source gives an approximate or rounded number (e.g., "about 50,000 worlds"), estimate plausible min/max ranges, since canon does not always mean precise. Use `semiCanon` for values from secondary sources (RPG sourcebooks, technical manuals, creator interviews, decanonized material).
4. **Cross-check values against existing data** (e.g., compare Kardashev to Earth 2023 = 0.73 and other civilizations, but also show other comparisons of other metrics) -- these comparisons are for validation only, do not include them in readme fields
5. **In-universe comparisons**: When sources compare to places that share real-world names (e.g., "Sol System", "Earth", "Mars"), assume they refer to the in-universe version at the same time period, not real-world data. Research the in-universe reference to properly contextualize the comparison.
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
For EVERY datapoint, show comparable values from existing civilizations to help maintain consistency:

| Metric | Value | Confidence | Comparisons (same universe) | Comparisons (other universes) |
|--------|-------|------------|----------------------------|-------------------------------|
| Population | 8e9 | canon | Empire 2040: 25e9 | Earth 2023: 8e9, Federation 2373: 1e12 |
| Kardashev | 0.85 | calculated | Empire 2040: 1.2 | Earth 2023: 0.73, Federation 2373: 2.1 |
| Planets | 12 | canon | Empire 2040: 150 | Federation 2373: 150 |

- Query existing civilizations in `data/universes/` for comparison data
- Prioritize comparisons from the same universe first
- Include Earth 2023 as baseline reference where applicable
- These comparisons are for validation only -- do NOT include them in readme fields or files

**Kardashev Calculation Rationale (REQUIRED):**
For every Kardashev value, show the calculation rationale including per-person power consumption:

1. **Calculate total power**: Estimate civilization's total energy consumption in Watts
2. **Calculate per-person power**: Total power / Population = Watts per person
3. **Compare to Earth 2023 baseline**: Earth uses ~18 TW for 8.1e9 people = ~2,200 W/person
4. **Apply Kardashev formula**: K = log10(Power) / 10 - 0.6 (where Power is in Watts)
   - Type I = 10^16 W (~1.2e6 W/person for 8e9 people)
   - Type II = 10^26 W (full stellar output)
5. **Show the math**: Present the calculation transparently so it can be verified

Example rationale:
```
Population: 50e12, Estimated power: 10^24 W
Per-person: 10^24 / 50e12 = 2e10 W/person (vs Earth 2023: 2,200 W/person = 9 million x more)
Kardashev: log10(10^24) / 10 - 0.6 = 24/10 - 0.6 = 1.8
```

### Step 5: Image Research & Attribution (REQUIRED)
1. Find images (see content.md for approved sources and proxy instructions)
2. **For EVERY image, research and document all attribution fields:**
   - `src`: Direct image URL (or `/proxy/...` path for proxied images)
   - `link`: Source page URL for attribution (e.g., Wikimedia Commons file page, publisher page)
   - `author`: Creator/artist name (search for the artist, e.g., book cover artists, map creators)
   - `license`: Short identifier (e.g., "CC BY-SA 4.0", "Public Domain", "(c) Publisher" for copyrighted works)
   - `legal`: Full license text or URL (e.g., "https://creativecommons.org/licenses/by-sa/4.0/" or "Copyrighted by [Publisher]. Used for commentary under Fair Use.")
3. **Research strategy for attribution:**
   - Wikimedia Commons: Fetch the file page to find author and license info
   - Book covers: Search for "[book title] cover artist" - publishers like Baen often use known artists
   - Fan wiki images: Check the file page for upload info and licensing
   - Official art: Note the copyright holder (studio, publisher) and use Fair Use rationale
4. **If attribution is incomplete**, ask the user whether to: (a) add the image anyway with "Unknown" for missing fields, (b) skip the image, or (c) search for a different image
5. Write descriptions for polity and civilization readme fields
6. Gather links to official/wiki pages
7. **Proxy images from Wikimedia/Wikipedia** (see content.md Image Proxy section)

### Step 6: Implementation
1. Create directories and info.yaml files following schemas in content.md:
   - `data/universes/{Universe}/info.yaml` (if new universe -- include `tags: [index, new]`)
   - `data/metrics/{NewMetric}/info.yaml` (if new metrics)
   - `data/universes/{Universe}/_polities/{Polity}/info.yaml` (if new polity)
   - `data/universes/{Universe}/{Civilization}/info.yaml` (include `tags: [index, new]`)
   - `data/universes/{Universe}/{Civilization}/{Metric}/info.yaml` (datapoints)
2. **Update `showcaseMetrics` in Universe info.yaml** (REQUIRED):
   - Check which metrics have datapoints across multiple civilizations in this universe
   - Update `showcaseMetrics` list to include all metrics that enable comparisons
   - Example: `showcaseMetrics: [Population, Kardashev, Planets, Star Systems, Warships]`
3. Verify all files created
4. Check if `.claude/settings.local.json` was modified (new permissions added during research)
5. Commit all changes together (data files + settings if modified) with message: "add {Civilization} civilization"

## Writing Style
- **All descriptions and readme texts MUST be written in present tense** (historisches Prasens / historical present), regardless of language. Example: "Das Imperium umfasst 50.000 Welten" (not "umfasste"), "The Empire controls 150 worlds" (not "controlled").
- **Never use em dashes**. Use commas, periods, or restructure sentences instead.
- **In-universe perspective**: Write all descriptions (universe, polity, civilization readmes) from an in-universe point of view, as if recorded by an accomplished historian within that universe. No publishing details, no meta-references to books, episodes, or authors. Focus on events, peoples, politics, and technology as historical facts.
- **Readme/description length**: Descriptions must be at least 1/4 page and at most 1 page long (roughly 75-300 words).
- Write in the language confirmed with the user in Step 1. (Of course keep all YAML keywords in English.)

## Research Sources (by priority)
1. **Official sources** - Creator-sanctioned content (Memory Alpha, Wookieepedia verified)
2. **Fan wikis** - Memory Alpha, Wookieepedia, various fandom wikis
3. **Original works** - Books, shows, movies, games
4. **Scholarly analysis** - Academic papers on fictional universes
5. **Fan calculations** - Well-reasoned community estimates

## Common Metrics to Research
For any civilization, try to find data on:
1. Population (total, and breakdown by type if available)
2. Kardashev level (energy usage, technological capability)
3. TechLevel (unified 1-20 scale) and Tech Summary (short text description)
4. Number of inhabited planets/worlds
5. Territory size (if measurable) and Location
6. Age of civilization
7. Key dates (founding, major events)

## Quality Checks
- All datapoints MUST have references
- Cross-check Kardashev against Earth 2023 (K=0.73) for sanity
- Use existing metrics when possible before creating new ones
- **Always update `showcaseMetrics`** in universe info.yaml to include all metrics with datapoints
- Images should be from canonical/official sources when available
- **All images MUST have complete attribution** (see content.md for required fields and approved sources)
- **New universes and civilizations MUST have `tags: [index, new]`**
- **All info.yaml files MUST have `created` and `changed` dates** (see content.md Timestamps)
