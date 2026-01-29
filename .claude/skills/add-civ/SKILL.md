---
name: add-civ
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
4. **Cross-check values against existing data** (e.g., compare Kardashev to Earth 2023 = 0.73)
5. Gather reference URLs for all values

### Step 4: Review
1. Present all proposed data in tables:
   - New metrics (if any)
   - Polity info
   - Civilization info
   - Datapoints with values, ranges, confidence, sources AND comparable figures from other civilizations of the same universe or different universes if appropriate
2. Ask user to approve or adjust values
3. Iterate until user is satisfied

### Step 5: Additional Research
1. Find images (from Memory Alpha, wikis, official sources)
2. Write descriptions for polity and civilization readme fields
3. Gather links to official/wiki pages
4. Ensure all info.yaml fields can be populated

### Step 6: Implementation
1. Create directories and info.yaml files:
   - `data/metrics/{NewMetric}/info.yaml` (if new metrics)
   - `data/universes/{Universe}/_polities/{Polity}/info.yaml` (if new polity)
   - `data/universes/{Universe}/{Civilization}/info.yaml`
   - `data/universes/{Universe}/{Civilization}/{Metric}/info.yaml` (datapoints)
2. Verify all files created
3. Commit with message: "add {Civilization} civilization"

## YAML Schemas Reference

See `.claude/rules/content.md` for full schemas. Key fields:

**Datapoint:**
```yaml
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

**Civilization:**
```yaml
title: Polity Year
date: 2161
polity: Polity Name
tags: [index]
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
