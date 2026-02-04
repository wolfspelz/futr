# Sample Universe

A fictional universe, the entire franchise across space and time where all stories happen.

A universe contains civilizations each with their own description.

Each universe is a folder with:
- descriptions:
  - info.yaml
  - readme.md
- civilization folders
- optional: a folder with polities

The name of the folder becomes the name of the universe.

Write something about the universe.
(This line checks that the markdown supports simple line breaks)

## info.yaml

Minimalistic:
```
images:
    - link: {Image URL}
      tags: [main]
```

Full:
```
showcaseMetrics: {list of names of metrics that will be shown on the universe page as a table for all civilizations, e.g. [Population, Planets, Your Metric]
title: {Name to overrride the folder name}
tags: {List of tags (new: appear in what's new, index: appear in index, e.g. [new, index]}
order: {Numerical order controlling the position lists, float, 0 is default, -1000.0 mean show early in lists, 1000.0 means show late}
editors: {list of github handles, e.g. [wolfspelz]}
links:
    - link: {Page URL}
      text: {Link text}
    - link: {Page URL}
      text: {Link text}
images:
    - link: {Image URL}
      text: {Image caption}
      page: {Image source page}
      author: {Creator/artist name}
      license: {Short license identifier, e.g. CC BY-SA 4.0}
      legal: {Full license text or URL}
      tags: [main]  # "main" = primary/tile image for lists
    - link: {Image URL}
      text: {Image caption}
      page: {Image source page}
readme: |
    Readme text in case there is no readme.md file.
    More readme text. This is a YAML multiline text.
```

See other universes for examples.
