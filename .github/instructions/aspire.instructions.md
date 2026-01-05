---
applyTo: '**/*'
description: Enforces the required .NET Aspire solution structure with AppHost, ServiceDefaults, and Extensions projects using resource definitions.
---

# .NET Aspire solution instructions

## Purpose
Define the mandatory project layout and implementation practices for .NET Aspire-based solutions so every repository contains a consistent AppHost, ServiceDefaults, and Extensions project, with resource definitions centralizing reusable infrastructure registrations.

## Rules or Guidelines
- Group every Aspire-specific asset inside a dedicated `Aspire/` folder, regardless of whether the repository follows a `src/` + `tests/` model or a more complex polyglot structure. The folder may live at the repository root (`Aspire/...`) or beneath another root (e.g., `src/Aspire/...`).
	- `Aspire/AppHost/AppHost.csproj` hosts the Aspire orchestrator.
	- `Aspire/ServiceDefaults/ServiceDefaults.csproj` consolidates shared service configuration (diagnostics, health checks, OpenTelemetry, resilience).
	- `Aspire/Extensions/Extensions.csproj` exposes strongly typed resource definitions reusable across services.
- The solution file must reference all three projects and ensure `AppHost` references `ServiceDefaults` and `Extensions`.
- `AppHost` must declare every project or container resource in `Program.cs` using `builder.AddProject`/`AddContainer` and apply defaults from `ServiceDefaults`.
- `ServiceDefaults` must provide extension methods (e.g., `AddServiceDefaults(this IHostApplicationBuilder builder)`) configuring logging, metrics, tracing, retry policies, and health checks.
- `Extensions` must organize resource definitions under `Aspire/Extensions/Resources/` (or the equivalent relative path if the `Aspire/` folder is nested) and expose factory methods returning `IResourceBuilder<T>` instances.
- Resource definitions must avoid inline magic strings; parameterize connection info through Aspire parameters or environment variables.
- All services consuming infrastructure must reference `Extensions` instead of duplicating `builder.AddAzureStorage` or similar calls.
- Keep infrastructure secrets in user secrets or Azure Key Vault; never hard-code credentials in any Aspire project.
- The solution file must reference all three projects and ensure `AppHost` references `ServiceDefaults` and `Extensions`.
- `AppHost` must declare every project or container resource in `Program.cs` using `builder.AddProject`/`AddContainer` and apply defaults from `ServiceDefaults`.
- `ServiceDefaults` must provide extension methods (e.g., `AddServiceDefaults(this IHostApplicationBuilder builder)`) configuring logging, metrics, tracing, retry policies, and health checks.
- `Extensions` must organize resource definitions under `src/Extensions/Resources/` and expose factory methods returning `IResourceBuilder<T>` instances.
- Resource definitions must avoid inline magic strings; parameterize connection info through Aspire parameters or environment variables.
- All services consuming infrastructure must reference `Extensions` instead of duplicating `builder.AddAzureStorage` or similar calls.
- Keep infrastructure secrets in user secrets or Azure Key Vault; never hard-code credentials in any Aspire project.

## Best Practices
- Treat `AppHost` as composition root: no business logic, only wiring resources, projects, and deployment metadata.
- Keep `ServiceDefaults` free from application-specific dependencies; only reference BCL and approved Microsoft.Extensions packages.
- Use naming aligned with the business context when defining resources (e.g., `PaymentsSqlDatabaseResource`).
- Version resource definitions so breaking changes require explicit consumer updates.
- Add XML doc comments to every public resource extension describing required environment variables and expected consumers.

## Examples
```csharp
// Aspire/Extensions/Resources/PaymentsSqlResource.cs
namespace Company.App.Extensions.Resources;

public static class PaymentsSqlResource
{
	public static IResourceBuilder<SqlServerDatabaseResource> AddPaymentsSqlDatabase(
		this IDistributedApplicationBuilder builder)
	{
		var sql = builder.AddSqlServer("payments-sql")
			.WithDataVolume("payments-sql-data")
			.WithLifetime(ContainerLifetime.Persistent);

		return sql.AddDatabase("payments-db")
			.WithParameter("PAYMENTS_SQL_PASSWORD");
	}
}

// Aspire/AppHost/Program.cs
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject("Company.App.Api", "../Company.App.Api/Company.App.Api.csproj")
	   .WithReference(builder.AddPaymentsSqlDatabase());

builder.Build().Run();
```

## References
- Microsoft Learn article “.NET Aspire documentation” (search keyword: `learn.microsoft.com/dotnet/aspire`).
- GitHub sample repository “dotnet/aspire-samples” (search keyword: `github.com/dotnet/aspire-samples`).
