---
applyTo: '**/*Tests/**/*.cs'
description: GitHub Copilot instructions for writing .NET unit tests with builders and TestDataHelpers
---

# Unit test instructions for Copilot

## Purpose

Guide GitHub Copilot when generating C# and .NET unit tests so every test suite in this repository follows the agreed builder-first arrangement and TestDataHelper data strategy while remaining focused on true unit scope (integration, architecture, and e2e guidance lives elsewhere).

## Rules or Guidelines

### Scope & workflow
- Apply these instructions to all .NET unit test projects in this repo, regardless of module or naming; assume xUnit as default.
- Follow TDD: add a failing test before changing production code, then implement the minimal fix, and finally refactor.
- Keep each test class focused on a single feature (typically one public method or command) so its name and contents clearly reflect the behavior under test.
- Annotate every test with the Arrange/Act/Assert comments. Arrange sets up data only, Act performs a single domain operation, Assert inspects the outcome.
- Name tests using the `Should_ExpectedBehavior_When_StateUnderTest` pattern (add a suffix for variants if needed). Prefer `[Theory]` with `InlineData` for parameterized cases; the last parameter should describe the scenario for readability.

### Arrange block
- Use static TestDataHelpers to initialize specifications.
- Use builders exclusively to set up domain state.
- Use Bogus to get randomized data for primitive values. 
- Deterministic values are ONLY allowed when explicitly required for the test logic. This prevents a lot of boilerplate code and automatically detects edge cases.
- Never instantiate aggregates, entities, or specifications directly in tests.
- Keep the Arrange block concise by leveraging builders' fluent methods to express intent (e.g., `WithOpenstaandeTermijn`, `WithRandomDebtor`).

#### TestDataHelper & random data
- Use (static) TestDataHelper to initialize specifications.
- Randomize test inputs within realistic ranges (e.g., decimals between 100 and 50,000; alphanumeric references; past dates). Only use deterministic values when the assertion depends on an exact number.
- Helper methods apply default values, based on Bogus, and allow overriding defaults for edge cases (nulls, extreme amounts, boundary dates) so unit tests can cover both happy path and failure scenarios.
- Do not call the TestDataHelper outside the Arrange block.

Example:
```csharp
[Fact]
public void Should_RegisterAchterstand_When_SpecComesFromTestDataHelper()
{
	// Arrange
	var dossier = DossierBuilder.Nieuw().Build();
	var spec = TestDataHelper.SchuldRegisterSpec(
		openstaandBedrag: TestDataHelper.RandomOpenstaandBedrag(),
		referentie: TestDataHelper.RandomReferentie());

	var achterstand = BetalingsachterstandBuilder
		.Nieuw(dossier)
		.MetSpecificatie(spec)
		.Registreer();

	// Act
	
	// Assert
}
```

#### Builder-first arrangement
- Never instantiate aggregates, entities, or specifications directly inside tests when a builder exists. Builders live under `Builders/` and must be the single source for arranging domain state.
- Builders must expose fluent methods that mutate intent (e.g., `WithOpenstaandeTermijn`, `WithRandomDebtor`). If functionality is missing, extend the builder before writing the test.
- Builders must always produce valid domain objects with sensible defaults so a test can arrange minimal context.
- Do not use the Builders outside the Arrange block.

Example:
```csharp
[Fact]
public void Should_AddExtraTermijn_When_ConfiguredViaBuilder()
{
	// Arrange
	var dossier = DossierBuilder.Nieuw().Build();
	var achterstand = BetalingsachterstandBuilder
		.Nieuw(dossier)
		.MetRandomDebtor()
		.MetOpenstaandeTermijn(TestDataHelper.NieuweTermijnSpec())
		.Registreer();

	// Act
	achterstand.VoegTermijnToe(TestDataHelper.NieuweTermijnSpec());

	// Assert
}
```


### Act block
- The Act block must perform a single (domain) operation (method call) related to the feature under test.

### Assert block

#### Assertion strategy (AwesomeAssertions)
- Prefer the repository’s fluent assertion extensions (collectively referred to as AwesomeAssertions) for every verification: operation outcomes, returned values, validation feedback, domain events, boolean flags, and collection counts.
- Avoid raw `ShouldBe`, `Assert.True`, or manual inspections when a purpose-built AwesomeAssertions helper exists. Use the domain validation error constants to keep failure assertions intention-revealing.
- Exercise the domain-event helpers for presence/absence/count checks instead of touching the event collection directly; the helpers provide clearer error messages and keep tests intention focused.
- When a scenario requires a new helper, add it to the AwesomeAssertions library with descriptive failure messages (Dutch wording per repository standard) before relying on it in tests.
- Treat the following as anti-patterns whenever AwesomeAssertions is available:
	- Calling generic assertion APIs such as `Assert.True`, `Assert.Equal`, `Assert.NotNull`, or `result.IsSuccess.ShouldBe(true)` when a domain-specific AwesomeAssertions helper covers the same intent.
	- Writing manual loops or property inspections (`events.Count(e => ...)`, `entity.Errors.Any(...)`) instead of invoking the corresponding helper (`ShouldHaveEvent`, `ShouldHaveValidationError`).
	- Accessing `result.Value` before verifying success via `ShouldBeSuccess` or `ShouldHaveValue`.
	- Copying inline multi-assert logic between tests rather than extracting a reusable helper into the AwesomeAssertions library.

Example:
```csharp
[Fact]
public void Should_RaiseEvent_When_TermijnWordtErkend()
{
	// Arrange
	var dossier = DossierBuilder.Nieuw().Build();
	var achterstand = BetalingsachterstandBuilder
		.Nieuw(dossier)
		.MetOpenstaandeTermijn(TestDataHelper.NieuweTermijnSpec())
		.Registreer();

	// Act
	var result = achterstand.ErkenNieuweTermijn(TestDataHelper.NieuweTermijnSpec());

	// Assert
	result.ShouldBeSuccess();
	result.ShouldHaveValue();
	achterstand.ShouldHaveEvent<TermijnGeregistreerd>();
}
```

>#### Assertion strategy (static helpers)
>- Rely on the shared static assertion helpers/extensions supplied with the testing framework (e.g., `Assert.Equal`, `Assert.Throws`, or custom static extension classes dedicated to that project).
>- Keep the same intent-focused style: create small static extension methods (e.g., `AssertBetalingsachterstandHeeftSaldo`) that wrap multiple primitive assertions so individual tests remain concise.
>- Ensure validation checks still rely on explicit domain error instances rather than anonymous strings so failures expose the business rule being exercised.

>Example:
>```csharp
>public static class AssertionHelpers
>{
>	public static void ShouldContainEvent<TEvent>(this IReadOnlyCollection<IDomainEvent> events)
>		where TEvent : IDomainEvent
>	{
>		Assert.Contains(events, @event => @event is TEvent);
>	}
>}
>
>[Fact]
>public void Should_RaiseEvent_When_StaticHelpersAreUsed()
>{
>	// Arrange
>	var dossier = DossierBuilder.Nieuw().Build();
>	var achterstand = BetalingsachterstandBuilder
>		.Nieuw(dossier)
>		.MetOpenstaandeTermijn(TestDataHelper.NieuweTermijnSpec())
>		.Registreer();
>
>	// Act
>	achterstand.ErkenNieuweTermijn(TestDataHelper.NieuweTermijnSpec());
>
>	// Assert
>	AssertionHelpers.ShouldContainEvent<TermijnGeregistreerd>(achterstand.DomainEvents);
>}
>```

### Test completeness
- For each behavior under test, Copilot must produce at least:
  1. One happy-path test that asserts success, emitted events, and resulting state.
  2. One failure/validation test covering the most critical guardrail.
  3. One edge-case test (e.g., null specification inputs, boundary amounts, duplicated references).
- Each test should cover only one behavior yet can assert multiple facets of that behavior (state, events, flags) using helper assertions.

### Definition of done for unit tests
- All modified tests pass locally via `dotnet test` for the affected projects.
- No lingering TODO/FIXME markers unless part of the existing guideline backlog and explicitly justified.
- All assertions leverage the domain helpers; avoid manual boolean or count checks when a fluent helper exists.
- Arrange logic uses builders/TestDataHelper only, with no private helper methods defined inside test classes.
- Any new builder or helper methods include XML docs describing intent and default ranges.

### Anti-patterns to avoid
- Expecting validation to rely on thrown exceptions rather than helper-based assertions.
- Repeating the aggregate name in method calls (`Betalingsachterstand.RegistreerBetalingsachterstand`). Keep methods intention-revealing but concise.
- Accessing or mutating aggregate internals directly in tests; interact solely through public domain methods.
- Hard-coded literal data for references, money, or dates when helpers/randomizers exist.
- Branching, loops, or additional act steps inside a single test method.

## Best Practices
- Keep tests independent and deterministic by isolating dependencies; mock only external services (clock, repositories) when absolutely necessary.
- Prefer expressive helper names for expected validation errors so assertions read like business rules.
- Capture complex arrangements in the builder (e.g., “with arrears history”) rather than repeating inline steps across tests.
- When adding new assertion helpers, ensure they provide informative failure messages in Dutch as per repository style.
- Document any new TestDataHelper methods with parameter explanations and default ranges so future Copilot runs can rely on them.

## References
- Unit testing best practices for .NET – Microsoft Learn (search: `dotnet/core/testing/unit-testing-best-practices`)