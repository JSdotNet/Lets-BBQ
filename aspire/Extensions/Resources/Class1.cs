//using System.Data.Common;

//using Aspire.Hosting;
//using Aspire.Hosting.ApplicationModel;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Diagnostics.HealthChecks;

//namespace AspireDemo.Extensions.Neo4j;

//public class Neo4jResource(string name, ParameterResource? password = null)
//    : ContainerResource(name), IResourceWithConnectionString
//{
//    public const string DefaultRegistry = "docker.io";
//    public const string DefaultImage = "neo4j";
//    public const string DefaultTag = "2025.07.1-community-bullseye";
//    public const string BoltEndpointName = "neo4j";
//    public const string AdminEndpointName = "http";
//    public const string Username = "neo4j";

//    public EndpointReference BoltEndpoint => new(this, BoltEndpointName);
//    public ParameterResource PasswordParameter { get; }
//        = password ?? new ParameterResource("neo4j-password", x => "P@ssw0rd123!", true);

//    public ReferenceExpression ConnectionStringExpression =>
//        ReferenceExpression.Create(
//            $"Endpoint={BoltEndpoint};Username={Username};Password={PasswordParameter}"
//        );
//}



//public static class Neo4jResourceExtensions
//{
//    public static IResourceBuilder<Neo4jResource> WithVolumeStorage(
//        this IResourceBuilder<Neo4jResource> builder,
//        string? name = null)
//    {
//        return builder.WithVolume(name, "/data");
//    }

//    public static IResourceBuilder<Neo4jResource> WithSeedDatabaseCommand(
//        this IResourceBuilder<Neo4jResource> builder)
//    {
//        return builder.WithCommand(
//            name: "seed",
//            displayName: "Seed Database",
//            commandOptions: new()
//            {
//                UpdateState = ctx =>
//                    ctx.ResourceSnapshot.HealthStatus is HealthStatus
//                        .Healthy
//                        ? ResourceCommandState.Enabled
//                        : ResourceCommandState.Disabled,
//                IconName = "ArchiveArrowBack",
//                IconVariant = IconVariant.Filled,
//                ConfirmationMessage = "Are you sure you want to seed the database?"
//            },
//            executeCommand: async context =>
//            {
//                var connstring = new DbConnectionStringBuilder
//                {
//                    ConnectionString =
//                        await builder.Resource.ConnectionStringExpression.GetValueAsync(CancellationToken.None)
//                };

//                await using var driver = GraphDatabase.Driver(
//                    (string)connstring["Endpoint"],
//                    AuthTokens.Basic(
//                        (string)connstring["Username"],
//                        (string)connstring["Password"]
//                    )
//                );

//                await driver.ExecutableQuery("MATCH (n) DETACH DELETE n").ExecuteAsync();
//                await driver.ExecutableQuery("CREATE (:User {name:'Chris'})").ExecuteAsync();
//                await driver.ExecutableQuery("CREATE (:User {name:'Erica'})").ExecuteAsync();
//                await driver.ExecutableQuery("CREATE (:User {name:'John'})").ExecuteAsync();
//                await driver.ExecutableQuery("CREATE (:User {name:'Lisa'})").ExecuteAsync();

//                var interactionService = context.ServiceProvider.GetRequiredService<IInteractionService>();
//                if (interactionService.IsAvailable)
//                {
//                    _ = interactionService.PromptNotificationAsync(
//                        title: "Database Seed Complete",
//                        message: "Database has been seeded with 4 users…",
//                        options: new NotificationInteractionOptions
//                        {
//                            Intent = MessageIntent.Information
//                        });
//                }

//                return new ExecuteCommandResult { Success = true };
//            }
//        );
//    }
//}