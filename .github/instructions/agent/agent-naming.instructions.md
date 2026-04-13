---
applyTo: '.github/agents/copilot.agent.md'
description: Defines naming and grouping conventions for agent-related instruction assets.
---

# Agent Naming Instructions

## Purpose

- Define a clear naming convention for instruction files that steer common agent behavior.
- Keep these files easy to recognize and maintain.
- Primary use is alongside the Copilot agent.

## Naming Convention

- Use the pattern: `agent-<topic>.instructions.md`.
- Use lowercase letters and hyphens in `<topic>`.
- Keep `<topic>` short and specific.

## Grouping Convention

- Agent-specific instruction files must be grouped in `.github/instructions/agent/`.
- Documentation-specific instruction files must be grouped in `.github/instructions/documentation/`.
- Idea-specific instruction files must be grouped in `.github/instructions/idea/`.
- Profile-specific instruction files must be grouped in `.github/instructions/profile/`.
- Work instruction files (stories, epics, bugs) must be grouped in `.github/instructions/work/`.
- Agent files must be stored directly under `.github/agents/` and use prefixes in filenames when grouping is needed.
- Do not repeat a prefix in the remaining name segment (for example, `profile.agent.md`, not `profile-profile.agent.md`).

## Examples

- Agent group:
  - `agent/agent-language-and-tone.instructions.md`
  - `agent/agent-handoff.instructions.md`
  - `agent/agent-model-recommendation.instructions.md`
  - `agent/agent-naming.instructions.md`
- Documentation group:
  - `documentation/howto.instructions.md`
- Profile group:
  - `profile/github.instructions.md`
  - `profile/projects.instructions.md`
  - `profile/linkedin.instructions.md`
- Idea group:
  - `idea/ideas.instructions.md`
- Work group:
  - `work/stories.instructions.md`
  - `work/epics.instructions.md`
  - `work/bugs.instructions.md`

- Agent prefixes:
  - `documentation.agent.md`
  - `product-owner.agent.md`
  - `profile.agent.md`

## Scope Note

- This convention applies to instruction files intended to steer common agent behavior.
- It does not force renaming of non-agent or artifact-specific instruction files.
- Grouping with subfolders is required for documentation, profile, work, and agent instruction files.

## IMPORTANT: English Enforcement for Agent and Instruction Files

- Files under `.github/agents/**/*.md` must always be written in English.
- Files under `.github/instructions/*.md` must always be written in English.
- Files under `.github/instructions/**/*.md` must always be written in English.
- This rule overrides project-specific language preferences for those folders.
- The Copilot agent must enforce this rule when creating or editing these files.

## Quick Compliance Check

- [ ] Common agent behavior instruction files follow `agent-<topic>.instructions.md`.
- [ ] Agent-specific instruction files are stored in `.github/instructions/agent/`.
- [ ] Documentation instruction files are stored in `.github/instructions/documentation/`.
- [ ] Idea instruction files are stored in `.github/instructions/idea/`.
- [ ] Profile instruction files are stored in `.github/instructions/profile/`.
- [ ] Work instruction files are stored in `.github/instructions/work/`.
- [ ] Agent files are stored directly under `.github/agents/` with prefixed names when grouping is needed.
- [ ] Agent filenames do not repeat their own grouping prefix.
- [ ] Topic names are specific and readable.
- [ ] The file purpose is clearly agent-behavior related.
- [ ] `.github/agents/**/*.md` and `.github/instructions/**/*.md` content is in English.