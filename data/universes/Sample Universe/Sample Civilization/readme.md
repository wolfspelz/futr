# Sample Civilization

A civilization is a fraction of a Universe. It may be the entire galaxy or just a single species in a certain year.

A civilization contains datapoints each with their own description.

Each civilization is a folder with:
- descriptions:
    - info.yaml
    - readme.md
- datapoints folders


The name of the folder becomes the name of the universe.

Write something about the civilization.

## info.yaml

Minimlistic:
```
date: {Year}
```

Full:
```
date: {Year in CE}
polity: {Polity name}
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
      text: {Image caption incl. copyright}
      page: {Image source page}
    - link: {Image URL}
      text: {Image caption incl. copyright}
      page: {Image source page}
readme: |
    Readme text in case there is no readme.md file.
    More readme text. This is a YAML multiline text.
```

See other civilizations for examples.
