# Copilot Plugins

## Purpose

Track GitHub Copilot plugins used by this repository so team members can install, update, and audit them consistently.

## Marketplace

- Marketplace: `awesome-copilot`
- Repository config: `.github/copilot-settings.json`

## Installed Plugins

| Plugin | Source | Notes |
|---|---|---|
| `dotnet` | `awesome-copilot` | Installed in local environment (`copilot plugin install dotnet@awesome-copilot`). |
| `awesome-copilot` | `awesome-copilot` | Meta discovery plugin for finding and generating curated Copilot resources. |
| `csharp-dotnet-development` | `awesome-copilot` | C#/.NET development guidance plugin. |
| `testing-automation` | `awesome-copilot` | Testing workflows and automation guidance plugin. |
| `azure` | `awesome-copilot` | Azure skills and MCP workflows plugin. |
| `azure-cloud-development` | `awesome-copilot` | Azure architecture and IaC development plugin. |
| `project-planning` | `awesome-copilot` | Planning support for epics, feature breakdown, and implementation planning workflows. |
| `software-engineering-team` | `awesome-copilot` | Multi-role engineering plugin covering architecture, implementation, QA, and DevOps workflows. |
| `technical-spike` | `awesome-copilot` | Research and assumption-validation workflows before committing to implementation. |
| `security-best-practices` | `awesome-copilot` | Security, accessibility, performance, and code-quality guardrails. |

## Skills

- See [Copilot Skills](./copilot-skills.md) for skill inventory and provenance.

## Suggested C#/.NET Plugins

| Plugin | Why use it |
|---|---|
| `csharp-dotnet-development` | Broad C#/.NET coding guidance and workflows. |
| `dotnet-diag` | Performance and diagnostics workflows for incidents and tuning. |
| `testing-automation` | Test generation and automation guidance, including unit-test workflows (xUnit-friendly). |
| `security-best-practices` | Security-focused guardrails and checks. |

## Requested Skill Focus

| Focus | Recommended plugin(s) | Notes |
|---|---|---|
| Aspire | `dotnet` | No clearly named standalone `aspire` plugin was found in the current Awesome Copilot plugin directory; use `dotnet` plus local repo instructions/skills for Aspire conventions. |
| C# | `csharp-dotnet-development` | Best match for day-to-day C# and ASP.NET development guidance. |
| xUnit | `testing-automation` | Covers unit/integration testing workflows; use with your existing xUnit test conventions. |
| Azure | `azure`, `azure-cloud-development` | Use `azure` for Azure MCP/server-centric workflows and `azure-cloud-development` for cloud architecture and IaC patterns. |
| Project planning | `project-planning` | Adds planning-focused guidance for epics, task decomposition, and delivery planning. |

## Team Commands

```bash
# Install
copilot plugin install <plugin-name>@awesome-copilot

# Install requested focus plugins
copilot plugin install awesome-copilot@awesome-copilot
copilot plugin install csharp-dotnet-development@awesome-copilot
copilot plugin install testing-automation@awesome-copilot
copilot plugin install azure@awesome-copilot
copilot plugin install azure-cloud-development@awesome-copilot
copilot plugin install project-planning@awesome-copilot
copilot plugin install software-engineering-team@awesome-copilot
copilot plugin install technical-spike@awesome-copilot
copilot plugin install security-best-practices@awesome-copilot

# List installed plugins
copilot plugin list

# Update plugin
copilot plugin update <plugin-name>

# Uninstall plugin
copilot plugin uninstall <plugin-name>
```

## Update Process

1. Add or remove plugin entries in the table above.
2. Record a short note in the `Notes` column when changes are made.
3. Keep this file aligned with team onboarding docs.
