# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

FUTR (Fictional Universe Taxonomy Research) collects and analyzes statistical data about civilizations from fictional universes (Star Trek, Orion's Arm, etc.) and real-world history. Website: https://www.futr.space/

## Commits

- Do NOT add "Co-Authored-By: Claude" to commit messages
- Keep commit messages short: max 2 lines

## Tools

Do not ask for these commands in this folder: ls, grep, find, tree

## Purpose-Specific Guidelines

Additional context is loaded automatically based on what you're working on:

- **Web development** (`src/`, `*.csproj`, `Dockerfile`): See `.claude/rules/development.md`
- **Content/research** (`data/`): See `.claude/rules/content.md`
