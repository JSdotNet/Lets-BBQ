---
applyTo: '**/*.cs'
description: Enforces Clean Architecture layering, dependency rules, and testing expectations for C# services.
---

# Clean Architecture

When implementing backend services, follow these Clean Architecture principles to ensure maintainability, scalability, and separation of concerns. This rule is tailored for .NET solutions with a multi-project structure.

## 1. Solution Structure

- Every bounded context, module, or component **must** expose the four Clean Architecture layers (Domain, Application, Infrastructure, Api). For single-module solutions this can be `[project].Domain`, `[project].Application`, `[project].Infrastructure`, `[project].Api`. For multi-module solutions, prefer names such as `[module].Domain` or `[project].[module].Domain` so each module has its own quartet.
- Layers may be grouped by module (e.g., `Ordering.Domain`, `Ordering.Application`, …), by layer folders containing module subfolders (e.g., `Domain/Ordering`, `Domain/Billing`), or a hybrid approach, as long as each module still honours the four-layer separation.
- Optional shared libraries (e.g., `SharedKernel`, `BuildingBlocks`) must remain infrastructure-agnostic and contain only cross-cutting abstractions/value objects used by multiple modules.
- Each project must contain a marker/reference file (e.g., `AssemblyReference.cs`) for test discovery and architecture validation.
- Tests must be in separate projects **per module or consolidated by test type**, for example:
  - `tests/Ordering.UnitTests`, `tests/Billing.UnitTests` (Domain/Application)
  - `tests/Ordering.IntegrationTests`, `tests/Billing.IntegrationTests` (Infrastructure/Api + architecture validation)
  - A shared `tests/ArchitectureTests` project is acceptable when it validates every module's layering rules.

## 2. Dependencies Between Layers

- Within every module the dependency rule stays the same: **Domain** has no dependencies, **Application** depends only on Domain, **Infrastructure** depends on Application + Domain, and **Api** depends only on Infrastructure.
- Cross-module communication must occur through allowed seams (e.g., Application contracts, published domain events, HTTP/grpc APIs) rather than direct references that break modular boundaries.
- Shared libraries may depend on Domain value objects but must not introduce dependencies back to consuming modules.
- These dependencies **must** be enforced by automated architecture tests (e.g., NetArchTest in `ArchitectureTests.cs`).
- Forbidden dependencies (e.g., EntityFrameworkCore in Api/Domain) must be checked by tests.

## 3. Folder and File Structure

- Use a **feature-oriented** (domain-driven) folder structure in each layer (e.g., `Order/`, `Customer/`).
>- Do **not** use technical root folders (Entities, ValueObjects, Services, etc.).
>- Example structure with multiple modules:
>
>```
>src/
>  Ordering.Domain/
>    AssemblyReference.cs
>    Order/
>      Order.cs
>      OrderCreatedEvent.cs
>  Ordering.Application/
>    AssemblyReference.cs
>    Order/
>      PlaceOrderCommand.cs
>  Ordering.Infrastructure/
>    AssemblyReference.cs
>    Persistence/
>      OrderRepository.cs
>  Ordering.Api/
>    Program.cs
>    Orders/OrdersEndpoints.cs
>  Billing.Domain/
>    AssemblyReference.cs
>    Invoice/
>      Invoice.cs
>  Billing.Application/
>    AssemblyReference.cs
>  Billing.Infrastructure/
>    AssemblyReference.cs
>  Billing.Api/
>    Program.cs
>  SharedKernel/
>    Money/Money.cs
>tests/
>  Ordering.UnitTests/
>  Billing.UnitTests/
>  Ordering.IntegrationTests/
>  Billing.IntegrationTests/
>  ArchitectureTests/
>    ArchitectureTests.cs
>```

## 4. Coding Style and Conventions

- Use file-scoped namespaces.
- One type per file.
- Follow Microsoft .NET C# coding conventions.
- Organize files by feature/domain.

## 5. Implementation Guidelines

- **Domain Layer**: All business logic, entities, value objects, and domain events. No dependencies on other layers.
- **Application Layer**: Use cases, commands, queries, interfaces for repositories/services. No business logic.
- **Infrastructure Layer**: Implementations for interfaces, database access, external integrations. No business logic.
- **Api Layer**: Minimal API endpoints, request/response mapping. No business logic.
- Use dependency injection for all cross-layer dependencies.
- Avoid circular dependencies.
>- Do not use a mediator library; call service methods directly from the Api layer.

## 6. Testing and Architecture Validation

- **Unit Tests**: In `tests/[module].UnitTests/` (or consolidated by layer), covering Domain and Application logic per module. Use xUnit v3 and FakeItEasy for mocks.
- **Integration Tests**: In `tests/[module].IntegrationTests/`, covering Infrastructure and Api behaviours per module. 
- **Architecture Tests**: Must be present in `ArchitectureTests.cs` (per module or shared) and:
  - Enforce allowed/forbidden dependencies between layers
  - Check for forbidden dependencies (e.g., EF Core in Api/Domain)
  - Optionally, check for immutability in Domain
- Always write tests before implementation (TDD).

## 8. Multi-Module Coordination

1. Document module boundaries and allowed cross-module dependencies in `ArchitectureTests.cs` to avoid accidental references.
2. When modules share infrastructure (databases, queues, etc.), encapsulate access through Infrastructure abstractions so each module still respects its clean layering.
3. Register module-specific services via DI modules (e.g., `Ordering.Api` calling `OrderingApplicationInstaller.AddOrderingApplication(builder)`) to keep wiring localized.
4. Shared types should live in dedicated libraries (e.g., `SharedKernel`) and remain persistence-agnostic; never let a shared library depend back on a concrete module.
5. Prefer asynchronous communication (events, messaging) between modules to reduce tight coupling; if synchronous calls are needed, go through well-defined APIs rather than direct Infrastructure references.

## 7. Architecture Testing Example

To enforce and validate architecture rules, add automated tests in `tests/[project].IntegrationTests/ArchitectureTests.cs` using NetArchTest (https://github.com/BenMorris/NetArchTest). Example:

```csharp
using System.Reflection;
using NetArchTest.Rules;
using Xunit;
using Xunit.Abstractions;

using Order.Application;
using Order.Domain;
using Order.Infrastructure;

namespace Order.IntegrationTests;

public class ArchitectureTests
{
    private static string EntityFrameworkCore = "Microsoft.EntityFrameworkCore";
    private const string ApiNamespace = "Api";
    private const string ApplicationNamespace = "Application";
    private const string DomainNamespace = "Domain";
    private const string InfrastructureNamespace = "Infrastructure";

    private static readonly Assembly ApiAssembly = typeof(Program).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(ApplicationReference).Assembly;
    private static readonly Assembly DomainAssembly = typeof(DomainReference).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(InfrastructureReference).Assembly;

    public ITestOutputHelper TestOutputHelper { get; }

    public ArchitectureTests(ITestOutputHelper testOutputHelper)
    {
        this.TestOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Api_ShouldOnlyDependOn_Application()
    {
        var result = Types.InAssembly(ApiAssembly)
            .That().ResideInNamespace(ApiNamespace)
            .Should().HaveDependencyOn(ApplicationNamespace)
            .And()
            .NotHaveDependencyOn(DomainNamespace)
            .And()
            .NotHaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, $"{ApiNamespace} should only depend on {ApplicationNamespace}");
    }

    // ...other architecture tests for Application, Infrastructure, Domain, and forbidden dependencies...
}
```

- Adapt namespaces, assemblies, and rules to your solution.
- Add tests to check for forbidden dependencies (e.g., EntityFrameworkCore in Api/Domain) and for immutability in Domain types if relevant.
- Run these tests with `dotnet test` to ensure architecture rules are enforced after every change.

## Additional Guidelines

1. Use dependency injection to manage dependencies across layers.
2. Avoid circular dependencies between layers.
3. Write unit tests for **Domain** and **Application** layers.
4. Use integration tests for **Infrastructure** and **Api** layers.
5. Follow SOLID principles within each layer.
6. Avoid using a mediator library; instead, directly call service methods from the **Api** layer.

# References
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/TheCleanArchitecture.html)
