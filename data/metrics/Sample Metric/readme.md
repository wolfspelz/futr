# Sample Metric

A metric is a way to measure a civilization.

Metrics are used by civilizations 

Each metric is a folder with:
- descriptions: 
    - info.yaml
    - readme.md

The name of the folder is the name of the metric.

Explain the metric.

## Reason

In general metrics could be universe or civilization specific. 
But having a list of known metrics for the project allows to compare different civilizations maybe even across universes.
The list is also a nice reference and idea collection for new universes. 

## info.yaml

Minimalistic:
```
type: {Data type: "number" or "text"}
unit: {The unit that is described by the metric, e.g. "Planets"}
```

Full:
```
type: {Data type: "number" or "text"}
unit: {The unit that is described by the metric, e.g. "Planets"}
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
      text: {Image caption incl. copyright}
      page: {Image source page}
    - link: {Image URL}
      text: {Image caption incl. copyright}
      page: {Image source page}
readme: |
    Readme text in case there is no readme.md file.
    More readme text. This is a YAML multiline text.
```

See other metrics for examples.
