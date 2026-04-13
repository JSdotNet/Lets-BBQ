---
name: create-technical-debt-record
description: "Create a Technical Debt Record (TDR) document for tracking debt, impact, and remediation planning."
---

# Create Technical Debt Record

Create a TDR document for ${input:DebtTitle} using a structured format optimized for AI consumption and human readability.

## Inputs

- Context: ${input:Context}
- Debt Description: ${input:DebtDescription}
- Impact: ${input:Impact}
- Possible Solutions: ${input:PossibleSolutions}
- Owner: ${input:Owner}
- Status: ${input:Status}

## Input Validation

If any required inputs are missing or cannot be determined from the conversation history, ask the user for the missing information before proceeding with TDR generation.

## Requirements

- Use precise, unambiguous language.
- Keep the debt item traceable to architectural risks and remediation goals.
- Use status values from the repository convention: Open, Planned, In Progress, Resolved, Won't Fix.
- Include realistic remediation options and expected follow-up.
- Structure content for both machine parsing and human reference.
- Use coded bullet points (3-4 letter codes + 3-digit numbers) for multi-item sections.

The TDR must be saved in the doc/tdrs directory using the naming convention tdr-NNNN-[debt-name].md, where NNNN is the next sequential 4-digit number (for example, tdr-0001-legacy-auth-flow.md).

## Required Documentation Structure

The documentation file must follow the template below and align with the repository TDR convention.

```md
### TDR NNNN: [Debt Title]

> Date: YYYY-MM-DD
> Status: **Open** (Open, Planned, In Progress, Resolved, Won't Fix)

#### Description

[Clear statement of the technical debt, origin, and current constraints.]

#### Impact

- IMP-001: [Engineering impact]
- IMP-002: [Operational or reliability impact]
- IMP-003: [Product or delivery impact]

#### Possible Solutions

- SOL-001: [Option 1 and rationale]
- SOL-002: [Option 2 and rationale]
- SOL-003: [Recommended path and trade-offs]

#### Owner

[Team or individual accountable for follow-up and status updates.]

#### Traceability

- REF-001: [Related ADRs]
- REF-002: [Related arc42 sections]
- REF-003: [Related risks, incidents, or backlog items]
```
