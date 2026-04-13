---
name: architecture-arc42-generator
description: Interactive arc42 architecture blueprint generator for drafting, validating, and iterating sections using reusable prompt and instruction assets.
---

# Architecture arc42 Generator

## Purpose

Use this skill to run an interactive, repeatable arc42 workflow that can be reused across repositories.

This skill is designed to:
- Keep section prompts and instructions as canonical assets.
- Guide incremental drafting with targeted questions.
- Validate cross-section consistency before finalizing updates.

## Entry Point Policy

- Keep the ARC42 agent as the main entry point for interactive sessions.
- This skill is the reusable workflow backbone that the ARC42 agent can invoke.
- Direct invocation is allowed, but agent-led usage is preferred.

## Prompt Packaging Strategy

Prompts can be moved into the skill package, and for cross-repository reuse this is recommended.

Preferred structure:
- Skill workflow: `.github/skills/architecture-arc42-generator/SKILL.md`
- Reusable prompt pack: `.github/skills/architecture-arc42-generator/prompts/arc42-section-XX.prompt.md`
- Reusable instruction pack: `.github/skills/architecture-arc42-generator/instructions/arc42-section-XX.instructions.md`

Compatibility mode:
- If repository-local assets exist under `.github/prompts/arc42` and `.github/instructions/arc42`, load those first as project overrides.
- Otherwise, use the skill-owned prompt and instruction pack.

## When To Use

Use this skill when the user asks to:
- Create or improve arc42 documentation.
- Fill one or more arc42 sections with missing content.
- Validate consistency between arc42 sections.
- Prepare architecture documentation incrementally with stakeholder review checkpoints.

## Expected Inputs

- Target section(s): 1-12 or "whole document".
- Desired detail level: `LEAN`, `ESSENTIAL`, or `THOROUGH`.
- Repository paths for:
  - Global arc42 instruction file.
  - Section-specific instruction files.
  - Section-specific prompt files.
  - Target arc42 documentation files.

If paths are not provided, discover them by convention first.

## Default Repository Conventions

Prefer these paths when they exist:
- Global instructions: `.github/instructions/arc42/arc42-global-instructions.md`
- Section instructions: `.github/instructions/arc42/arc42-section-XX-instructions.md`
- Section prompts: `.github/prompts/arc42/arc42-section-XX.prompt.md`
- Documentation targets: `doc/arc42/XX_*.md`

Fallback for cross-repo reuse:
- Skill instructions: `.github/skills/architecture-arc42-generator/instructions/`
- Skill prompts: `.github/skills/architecture-arc42-generator/prompts/`

## Interactive Workflow

1. Confirm scope.
- Ask which section(s) to work on and expected output depth.

2. Load authoritative guidance.
- Read global instructions.
- Read matching section instruction and prompt assets.
- Do not duplicate or replace these sources with ad hoc rules.

3. Reuse existing facts.
- Read current section content and related sections.
- Extract confirmed facts, decisions, constraints, and unresolved placeholders.

4. Ask only missing questions.
- Use a minimal, conflict-focused questionnaire.
- Avoid requesting information already present elsewhere.

5. Draft incrementally.
- Produce partial section updates as soon as enough information is available.
- Mark assumptions and unknowns explicitly.

6. Run consistency checks.
- Verify section links and dependencies (especially 3, 5, 6, 7, 10, 11).
- Flag contradictions and propose exact edits.

7. Finalize with action.
- Offer to apply updates directly to target markdown files.
- Summarize unresolved questions and owners.

## Mandatory Quality Gates

- Section-specific checklists from the loaded instruction and prompt files are satisfied.
- Output remains concise, stakeholder-relevant, and traceable.
- Cross-section references are explicit.
- Assumptions, risks, and open questions are visible.
- Content is markdown-ready and easy to review.

## Cross-Section Consistency Rules

At minimum, validate:
- Section 1 quality goals are reflected in Section 4 strategy and Section 10 scenarios.
- Section 3 external interfaces are represented in Section 5 Level-1.
- Section 5 building blocks appear in Section 6 runtime scenarios.
- Section 5 building blocks map to Section 7 deployment units.
- Section 9 ADR references align with decisions implied in Sections 4, 5, 7, and 8.
- Section 11 risks/debt reference concrete affected sections and decisions.

## Output Contract

For each iteration, produce:
- Updated draft content for the active section.
- A short "Assumptions and Open Questions" list.
- A "Consistency Findings" list with concrete follow-up edits.
- A proposed next step (continue section, switch section, or apply changes).

## Reuse Notes For Other Repositories

To reuse in another repository:
- Keep this skill unchanged where possible.
- Map path conventions only.
- Repoint section templates/prompts if filenames differ.
- Preserve the interactive flow and quality gates.

## Example User Triggers

- "Update arc42 section 5 using existing project rules and ask only what is missing."
- "Draft sections 3 and 5, then report consistency gaps with section 7."
- "Run a lean pass for all sections and list unresolved architecture decisions."
