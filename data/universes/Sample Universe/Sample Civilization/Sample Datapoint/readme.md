# Sample Datapoint

A datapoint is a value for a metric.

Each datapoint is a folder with:
- descriptions: 
    - info.yaml
    - readme.md

The name of the folder is the name of the metric for which this datapoint has a value.

Explain the reasoning for the value of the datapoint.

## info.yaml

Minimalistic:
```
value: {This is the main value, e.g. the United Federation of Planets has 150 planets in 2373}
min: {A lower limit for the value, because values are never exact. One standard deviation, e.g. the United Federation of Planets might have only 120 planets in 2373}
max: {An upper limit for the value, because values are never exact. One standard deviation, e.g. the United Federation of Planets might have even 180 planets in 2373}
confidence: {Confidence label. How confident are you as a label, not a number. Allowed values: canon, calculated, informedGuess, calculatedGuess, wildGuess}
references:
    - link: {Link to the source. Values must be documented and "proven" by references}
      text: {Link text}
```

Full:
```
value: {This is the main value, e.g. the United Federation of Planets has 150 planets in 2373}
min: {A lower limit for the value, because values are never exact. One standard deviation, e.g. the United Federation of Planets might have only 120 planets in 2373}
max: {An upper limit for the value, because values are never exact. One standard deviation, e.g. the United Federation of Planets might have even 180 planets in 2373}
confidence: {Confidence label. How confident are you as a label, not a number. Allowed values: canon, calculated, informedGuess, calculatedGuess, wildGuess}
references: 
    - link: {Link to the source. Values must be documented and "proven" by references}
      text: {Link text}
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

See other datapoints for examples.
