# Copilot Model Selection

## Purpose

Define the repository preference for Copilot model configuration using Microsoft Foundry model catalog
entries and deployments. This guidance is Azure Foundry catalog-specific, not generic model-selection
guidance.

## Microsoft Foundry Configuration

Use Azure Foundry model catalog entries by default. In the UI, the `Wire model` value must match the
Azure Foundry deployment or model ID exactly.

Prefer the Azure Foundry models below because Claude 5 models currently fail when the Anthropic provider
injects the deprecated `temperature` parameter.

## Recommended Foundry Catalog Entries

| Foundry display name | Wire model | Max prompt tokens | Max output tokens | Intended use |
|---|---|---:|---:|---|
| `gpt-5.4` | `gpt-5.4` | `922000` | `128000` | Default configured model |
| `gpt-5.5` | `gpt-5.5` | `922000` | `128000` | Premium fallback |
| `gpt-5.6-luna` | `gpt-5.6-luna` | `922000` | `128000` | Fast and cheaper routine work |
| `gpt-5.6-sol` | `gpt-5.6-sol` | `922000` | `128000` | Optional balanced alternative |

## Claude Compatibility Fallbacks

Use older Claude fallback models only as Azure Foundry catalog entries, and only when the Foundry
catalog/provider accepts requests without the deprecated `temperature` parameter.

| Foundry display name | Wire model | Max prompt tokens | Max output tokens | Intended use |
|---|---|---:|---:|---|
| `claude-sonnet-4-6` | `claude-sonnet-4-6` | `1000000` | `128000` | Conditional premium fallback |
| `claude-sonnet-4-5` | `claude-sonnet-4-5` | `200000` | `64000` | Conditional general fallback |
| `claude-haiku-4-5` | `claude-haiku-4-5` | `200000` | `64000` | Conditional fast fallback |

## Models To Avoid

Avoid Claude 5 Azure Foundry catalog entries until the provider stops sending `temperature`:

- `claude-opus-5`
- `claude-sonnet-5`
- `claude-fable-5`

## Reasoning Model Parameter Safety

Do not configure request-level sampling or generation parameters for reasoning models. Avoid:

- `temperature`
- `top_p`
- penalties
- `logprobs`
- `logit_bias`
- `max_tokens`

Configure model identity, wire model, and supported token limits only. Let the reasoning provider apply its
own defaults for sampling and generation behavior.
