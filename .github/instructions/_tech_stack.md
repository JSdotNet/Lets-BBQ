## Technology Stack

### Frontend (Blazor)
- **Blazor Server + WebAssembly** hybrid approach
- **Fluent UI components** for consistent design
- **Interactive rendering modes**: Use `@rendermode InteractiveWebAssembly` for client-side components
- **CSS**: Use Fluent UI CSS variables and design tokens

### Backend
- **.NET 8** with latest C# 12 features
- **ASP.NET Core** for APIs and web hosting
- **MediatR** for CQRS pattern implementation
- **FluentValidation** for input validation
- **Entity Framework Core** for data access

### Infrastructure
- **SQL Server** for primary database
- **Aspire** for service orchestration and observability
- **Health checks** implementation required
- **OpenTelemetry** for monitoring