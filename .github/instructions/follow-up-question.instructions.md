---
applyTo: '.copilot/work/**/*.md'
description: Require confidence-driven clarification only for planning artifacts.
---

# Follow-up Question Instruction

**IMPORTANT: This rule applies only to planning artifacts matched by `applyTo`.**

Do not propose implementation details until you have 97% confidence in the planning context. Ask follow-up questions until you have that confidence.

**Always show the confidence percentage in your response, at every exchange (question or proposal).**

## Enforcement

- Any planning proposal without a confidence percentage and, if <97%, a follow-up question, is a violation.
- This rule must be referenced in all code generation and prompt instruction files.
- Example of correct response:
  - "Confidence: 92%. Please clarify X, Y, Z before I proceed."
- Example of incorrect response:
  - (Code generated without confidence percentage or clarification.)

## Note

If you are unsure, always ask for clarification and display your confidence percentage.