# Copilot Customization Reference

## Purpose

This is a compact reference for working with custom instructions, custom agents, skills, and prompt files in this repository.

## Official Documentation

- Inspiration and examples:
  - https://github.com/github/awesome-copilot
- GitHub repository instructions:
  - https://docs.github.com/en/copilot/how-tos/configure-custom-instructions/add-repository-instructions
- VS Code customization overview:
  - https://code.visualstudio.com/docs/copilot/customization/overview
- VS Code custom instructions:
  - https://code.visualstudio.com/docs/copilot/customization/custom-instructions
- VS Code custom agents:
  - https://code.visualstudio.com/docs/copilot/customization/custom-agents
- VS Code agent skills:
  - https://code.visualstudio.com/docs/copilot/customization/agent-skills
- VS Code prompt files:
  - https://code.visualstudio.com/docs/copilot/customization/prompt-files

## Plugins And Workflow Resources

- VS Code agent plugins (official):
  - https://code.visualstudio.com/docs/copilot/customization/agent-plugins
- VS Code handoffs between custom agents (official):
  - https://code.visualstudio.com/docs/copilot/customization/custom-agents#_handoffs
- VS Code planning workflow with agents (official):
  - https://code.visualstudio.com/docs/copilot/agents/planning
- GitHub custom agents for organizations (official):
  - https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents

## Priority And Scope

When multiple instruction sources apply, use this precedence model:

1. Personal instructions (highest)
2. Repository always-on instructions (`.github/copilot-instructions.md` or `AGENTS.md`)
3. Organization instructions

Notes:

- Multiple relevant instruction files can be included together.
- In conflicts, higher-priority sources win.
- For `.instructions.md`, `applyTo` controls automatic inclusion.

## Repository Conventions

- Agent files: `.github/agents/*.agent.md`
- Path-specific instructions: `.github/instructions/**/*.instructions.md`
- Skills: `.github/skills/*.md`
- Markdown baseline: `.github/instructions/markdown.instructions.md`
- Handoff policy: `.github/instructions/agent/agent-handoff.instructions.md`

## Quick Decision Guide

- Use `.github/copilot-instructions.md` for repository-wide standards.
- Use `.github/instructions/*.instructions.md` for path-specific rules.
- Use `.github/agents/*.agent.md` for personas, tool limits, and handoffs.
- Use `.github/skills/<skill>/SKILL.md` for reusable workflows with resources.
- Use `.github/prompts/*.prompt.md` for slash-command style prompts.

## Troubleshooting Checklist

- Confirm file location is valid for its type.
- Confirm extension and naming pattern are correct.
- Confirm frontmatter keys are valid (`applyTo`, `tools`, etc.).
- Confirm `applyTo` glob matches the active file.
- Check Chat diagnostics to see which files were loaded.
- Check relevant settings (`chat.useAgentsMdFile`, `chat.includeApplyingInstructions`, `chat.instructionsFilesLocations`).

## Notes

- Last curated: 2026-04-13.
- Keep this file short and practical; use official docs for deeper details.
