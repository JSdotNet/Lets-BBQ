---
applyTo: '**/*.md'
---

# Global Markdown Rules

## Purpose
- Apply these rules to every Markdown file in this repository.
- Treat this file as the baseline for Markdown quality and lint safety.
- Goal: produced Markdown should contain no Markdown style warnings.

## Global Standard
- Write Markdown compatible with common `markdownlint` defaults.
- When multiple instruction files apply, this file is the minimum baseline.
- File-specific instruction files may add stricter requirements but should not weaken these rules.

## Required Formatting Rules
- Use ATX headings only (`#`, `##`, `###`, ...), with one space after `#`.
- Increment heading levels one level at a time (no skipped heading levels).
- Keep exactly one top-level heading (`#`) per file.
- Leave exactly one blank line before and after headings.
- Do not leave trailing whitespace.
- Do not use multiple consecutive blank lines.
- End every file with exactly one newline.

## Lists
- Use `-` for unordered lists.
- Use ordered lists as `1.`, `2.`, `3.` when sequence matters.
- Keep list marker style consistent within the same list.
- Leave one blank line before and after every list block.
- For nested lists, add a blank line before the nested list starts and after it ends.
- Indent nested list content with 2 spaces.

## Links and Images
- Use descriptive link text; avoid bare URLs in running prose.
- Keep link syntax valid and balanced: `[text](url)`.
- Use reference-style links only when reused multiple times.
- Ensure image alt text is present and meaningful.

## Code and Commands
- Wrap inline commands, paths, env vars, and identifiers in backticks.
- Use fenced code blocks with explicit language tags where possible (for example `bash`, `json`, `yaml`, `powershell`).
- Use consistent fence style with triple backticks.
- Do not use indented code blocks.

## Tables and Blockquotes
- Keep table pipes aligned enough to remain readable in raw Markdown.
- Include a valid separator row in every table.
- Keep blockquotes for callouts or quoted text only; avoid large quoted sections for normal content.

## Line Length and Readability
- Prefer short lines for readability (target around 100-120 chars).
- If a long line improves link readability or avoids awkward wrapping, keep it intentional and limited.
- Keep paragraphs concise and scannable.

## Pre-Publish Lint Checklist
- [ ] One `#` heading per file.
- [ ] No skipped heading levels.
- [ ] Blank lines around all lists (including nested lists) are correct.
- [ ] No trailing spaces or extra blank lines.
- [ ] Fenced code blocks use backticks and language tags.
- [ ] Links, lists, and tables render correctly in preview.

## Agent Behavior for Markdown Output
- Prefer minimal, valid Markdown over complex formatting.
- If uncertain between two styles, choose the style least likely to trigger lint warnings.
- Do not introduce HTML when normal Markdown can express the same content.
