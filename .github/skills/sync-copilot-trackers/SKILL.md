---
name: sync-copilot-trackers
description: Keep .github/copilot-plugins.md and .github/copilot-skills.md synchronized with the actual installed Copilot plugins and available skills.
license: MIT
---

# Sync Copilot Plugin and Skill Trackers

## Purpose

Use this skill when you need to refresh the repository tracker documents from the real environment state:
- `.github/copilot-plugins.md`
- `.github/copilot-skills.md`

This skill prevents drift between documented plugin/skill inventory and what is actually installed or present.

## Inputs

- Repository root path.
- Installed plugin metadata from local Copilot plugin cache.

## Data Sources

1. Installed plugins:
   - `~/.copilot/installed-plugins/**/plugin.json`
2. Local repository skills:
   - `.github/skills/*/SKILL.md`
3. Existing tracker files:
   - `.github/copilot-plugins.md`
   - `.github/copilot-skills.md`

## Update Rules

1. Read installed plugin manifests and extract:
   - Plugin id/name
   - Source/marketplace (if available)
   - Declared skills list (if available)
2. Build a deduplicated skill inventory with one row per skill.
3. Determine skill source values:
   - `Plugin: <plugin-name>` for plugin-provided skills
   - `Local: .github/skills` for repository-owned skills
   - Multiple origins must be combined in one source cell, comma-separated.
4. Descriptions:
   - For local skills, read frontmatter `description` from `.github/skills/<skill>/SKILL.md`.
   - For plugin skills, use description from plugin-provided skill metadata when available.
   - If no description can be found, use: `Description not available from metadata.`
5. Sort rows alphabetically by skill name.
6. Keep prose sections in tracker files, but fully regenerate the inventory tables.

## Required Output Shapes

### `.github/copilot-plugins.md`

- Refresh `## Installed Plugins` table from actual plugin manifests.
- Keep each row as:
  - Plugin
  - Source
  - Notes
- Notes should be concise and factual (for example: `Detected in local Copilot plugin cache.`)

### `.github/copilot-skills.md`

- Refresh `## Skills List and Provenance` table with columns:
  - Skill
  - Source
  - Description
- Ensure one row per skill.
- Merge duplicate skills from multiple sources into a single row.

## PowerShell Reference Workflow

Run from repository root.

```powershell
$pluginFiles = Get-ChildItem "$HOME/.copilot/installed-plugins" -Recurse -Filter plugin.json -ErrorAction SilentlyContinue
$plugins = foreach ($file in $pluginFiles) {
  $json = Get-Content $file.FullName -Raw | ConvertFrom-Json
  [PSCustomObject]@{
    Name = $json.name
    Source = if ($json.marketplace) { $json.marketplace } else { "unknown" }
    Skills = @($json.skills)
    File = $file.FullName
  }
}

$localSkillDirs = Get-ChildItem ".github/skills" -Directory -ErrorAction SilentlyContinue
$localSkills = foreach ($dir in $localSkillDirs) {
  $skillFile = Join-Path $dir.FullName "SKILL.md"
  if (-not (Test-Path $skillFile)) { continue }

  $content = Get-Content $skillFile -Raw
  $nameMatch = [regex]::Match($content, "(?ms)^---.*?^name:\s*([^\r\n]+).*$")
  $descMatch = [regex]::Match($content, "(?ms)^---.*?^description:\s*['\"]?([^\r\n'\"]+)['\"]?.*$")

  [PSCustomObject]@{
    Name = if ($nameMatch.Success) { $nameMatch.Groups[1].Value.Trim() } else { $dir.Name }
    Description = if ($descMatch.Success) { $descMatch.Groups[1].Value.Trim() } else { "Description not available from metadata." }
    Source = "Local: .github/skills"
  }
}
```

## Execution Checklist

1. Read and parse plugin manifests.
2. Read and parse local skill frontmatter.
3. Build normalized plugin table.
4. Build normalized skill table.
5. Update `.github/copilot-plugins.md` table.
6. Update `.github/copilot-skills.md` table.
7. Verify markdown table rendering and sorting.
8. Report summary:
   - Plugin count
   - Total skill count
   - Skills with multiple sources
   - Skills missing metadata descriptions

## Guardrails

- Do not invent plugin names or skills.
- Do not remove prose guidance sections outside the inventory tables.
- Do not use stale cached outputs from previous sessions.
- Always recalculate from current filesystem state.
- Keep all instructions and generated content in English.
