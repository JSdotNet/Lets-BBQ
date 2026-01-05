//using System.Text;

//using Aspire.Hosting;
//using Aspire.Hosting.ApplicationModel;
//using Aspire.Hosting.Eventing;
//using Aspire.Hosting.Lifecycle;
//using Aspire.Hosting.Pipelines;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//namespace Lets_BBQ.Extensions.Publishers;

//public class RecipientAnnotation(string name, string email) : IResourceAnnotation
//{
//    public string Name { get; } = name;
//    public string Email { get; } = email;
//}

//public class SenderAnnotation(string name) : IResourceAnnotation
//{
//    public string Name { get; } = name;
//}


//internal class EmailContentBuilder
//{
//    private StringBuilder contents = new();
//    private bool hasAddedProjectResources;
//    private bool hasAddedContainerResources;

//    public EmailContentBuilder AddGreeting(string? name)
//    {
//        contents!.Append(@$"Dear {name ?? "IT-person"}!
//We need to deploy our solution, and because of this, we need some stuff from you...

//Here are the specifics...

//");
//        return this;
//    }

//    public EmailContentBuilder AddResource(EnterpriseProjectResource resource)
//    {
//        if (!hasAddedProjectResources)
//        {
//            contents.AppendLine("We need the following IIS web apps:");
//            hasAddedProjectResources = true;
//        }

//        contents.AppendLine($" - {resource.Name}");
//        return this;
//    }

//    public EmailContentBuilder AddResource(EnterpriseContainerResource resource)
//    {
//        if (!hasAddedContainerResources)
//        {
//            contents.AppendLine(
//                $"\r\nWe{(hasAddedProjectResources ? " also " : " ")}need the following container(s) to be hosted:");
//            hasAddedContainerResources = true;
//        }

//        ContainerImageAnnotation? image = null;
//        if (resource.TargetResource.TryGetAnnotationsOfType<ContainerImageAnnotation>(
//                out var imageAnnotations))
//        {
//            image = imageAnnotations.FirstOrDefault();
//        }

//        contents.AppendLine($" - {resource.Name} ({image!.Registry}/{image!.Image}:{image!.Tag})");
//        foreach (var port in resource.GetPorts())
//        {
//            contents.AppendLine($"   - {port.Scheme}: {port.ExposedPort}");
//        }

//        foreach (var mount in resource.GetVolumes())
//        {
//            contents.AppendLine($"   - Mount: {mount.Target}{(mount.ReadOnly ? " (as read only)" : "")}");
//        }

//        return this;
//    }

//    public EmailContentBuilder AddExternalResource(EnterpriseExternalResource[] resources)
//    {
//        if (resources.Length == 0)
//            return this;

//        contents.AppendLine(
//            $"\r\nWe also need to make sure that the firewall allows us to talk to:");

//        foreach (var resource in resources)
//            contents.AppendLine($" - {resource.Uri}");

//        return this;
//    }

//    public EmailContentBuilder AddSignature(string? name)
//    {
//        contents.Append(@$"
//Thank you for the help!
//// {name ?? "The Development Team"}
//");
//        return this;
//    }

//    public string Build() => contents.ToString();
//}


//public class EnterpriseEnvironmentEventSubscriber
//    : IDistributedApplicationEventingSubscriber
//{
//    public Task SubscribeAsync(
//        IDistributedApplicationEventing eventing,
//        DistributedApplicationExecutionContext ctx,
//        CancellationToken cancellationToken
//    )
//    {
//        eventing.Subscribe<BeforeStartEvent>(OnBeforeStartAsync);
//        return Task.CompletedTask;
//    }

//    private Task OnBeforeStartAsync(BeforeStartEvent @event, CancellationToken cancellationToken = default)
//    {
//        var environment = @event.Model.Resources.OfType<EnterpriseEnvironmentResource>().Single();

//        var resources = @event.Model.GetComputeResources().ToList();
//        resources.AddRange(@event.Model.Resources.OfType<ExternalServiceResource>());

//        foreach (var r in resources)
//        {
//            EnterpriseServiceResource serviceResource = r switch
//            {
//                ProjectResource => new EnterpriseProjectResource(r.Name, r, environment),
//                ContainerResource => new EnterpriseContainerResource(r.Name, r, environment),
//                ExternalServiceResource => new EnterpriseExternalResource(r.Name, r, environment),
//                _ => throw new InvalidOperationException("Weird...")
//            };

//            r.Annotations.Add(new DeploymentTargetAnnotation(serviceResource)
//            {
//                ComputeEnvironment = environment
//            });

//            environment.ResourceMapping[r] = serviceResource;
//        }
//        return Task.CompletedTask;
//    }
//}

//public static class EnterpriseEnvironmentExtensions
//{
//    public static IResourceBuilder<EnterpriseEnvironmentResource> WithSender(
//        this IResourceBuilder<EnterpriseEnvironmentResource> builder,
//        string name)
//    {
//        return builder.WithAnnotation(new SenderAnnotation(name));
//    }

//    public static IResourceBuilder<EnterpriseEnvironmentResource> WithRecipient(
//        this IResourceBuilder<EnterpriseEnvironmentResource> builder,
//        string name,
//        string email)
//    {
//        return builder.WithAnnotation(new RecipientAnnotation(name, email));
//    }
//}



//public class EnterpriseEnvironmentResource
//    : Resource, IComputeEnvironmentResource
//{
//    public EnterpriseEnvironmentResource(string name) : base(name)
//    {
//        Annotations.Add(new PipelineStepAnnotation(async (factoryContext) =>
//        {
//            var steps = new List<PipelineStep>();
//            steps.Add(new PipelineStep
//            {
//                Name = $"publish-{Name}",
//                Action = PublishAsync,
//                Tags = ["publish-environment"],
//                RequiredBySteps = [WellKnownPipelineSteps.Publish],
//            });

//            var resources = factoryContext.PipelineContext.Model.GetComputeResources();
//            foreach (var resource in resources)
//            {
//                var deploymentTarget = resource.GetDeploymentTargetAnnotation(this)?.DeploymentTarget;
//                if (deploymentTarget is null)
//                {
//                    continue;
//                }

//                if (deploymentTarget.TryGetAnnotationsOfType<PipelineStepAnnotation>(out var annotations))
//                {
//                    foreach (var annotation in annotations)
//                    {
//                        var childFactoryContext = new PipelineStepFactoryContext
//                        {
//                            PipelineContext = factoryContext.PipelineContext,
//                            Resource = deploymentTarget
//                        };

//                        var deploymentTargetSteps = (await annotation.CreateStepsAsync(childFactoryContext)).ToArray();
//                        foreach (var step in deploymentTargetSteps)
//                        {
//                            step.Resource ??= deploymentTarget;
//                        }

//                        steps.AddRange(deploymentTargetSteps);
//                    }
//                }
//            }

//            return steps;
//        }));

//        Annotations.Add(new PipelineConfigurationAnnotation(context =>
//        {
//            var resources = context.Model.GetComputeResources().ToList();
//            resources.AddRange(context.Model.Resources.OfType<ExternalServiceResource>());

//            foreach (var resource in resources)
//            {
//                var deploymentTarget = resource.GetDeploymentTargetAnnotation(this)?.DeploymentTarget;
//                if (deploymentTarget is null)
//                {
//                    continue;
//                }

//                var notificationSteps = context.GetSteps(deploymentTarget, "notification");
//                var publishStep = context.GetSteps(this, "publish-environment");
//                publishStep.DependsOn(notificationSteps);
//            }
//        }));
//    }

//    private Task PublishAsync(PipelineStepContext context)
//    {
//        var contentBuilder = new EmailContentBuilder();

//        var recipient = Annotations.OfType<RecipientAnnotation>().FirstOrDefault();
//        contentBuilder.AddGreeting(recipient?.Name);

//        var environment = context.Model.Resources.OfType<EnterpriseEnvironmentResource>().First();

//        var projectResources = environment.ResourceMapping.Values.OfType<EnterpriseProjectResource>().ToArray();
//        var containerResources = environment.ResourceMapping.Values.OfType<EnterpriseContainerResource>().ToArray();
//        var externalResources = environment.ResourceMapping.Values.OfType<EnterpriseExternalResource>().ToArray();

//        foreach (var resource in projectResources)
//        {
//            contentBuilder.AddResource(resource);
//        }

//        foreach (var resource in containerResources)
//        {
//            contentBuilder.AddResource(resource);
//        }

//        contentBuilder.AddExternalResource(externalResources);

//        var sender = Annotations.OfType<SenderAnnotation>().FirstOrDefault();
//        contentBuilder.AddSignature(sender?.Name);

//        var outputService = context.Services.GetRequiredService<IPipelineOutputService>();
//        var outputPath = outputService.GetOutputDirectory();
//        Directory.CreateDirectory(outputPath);
//        var outputFile = Path.Combine(outputPath, "email.txt");

//        return File.WriteAllTextAsync(outputFile, contentBuilder.Build(), Encoding.UTF8);
//    }

//    internal Dictionary<IResource, EnterpriseServiceResource> ResourceMapping { get; } = new();
//}


//public abstract class EnterpriseServiceResource
//    : Resource, IResourceWithParent<EnterpriseEnvironmentResource>
//{
//    protected EnterpriseServiceResource(string name, IResource resource,
//        EnterpriseEnvironmentResource enterpriseEnvironmentResource) : base(name)
//    {
//        TargetResource = resource;
//        Parent = enterpriseEnvironmentResource;
//        Annotations.Add(new PipelineStepAnnotation(_ =>
//        [
//            new PipelineStep
//            {
//                Name = $"{TargetResource.Name}-notification",
//                Action = ctx => NotifyAdded(ctx, Parent),
//                Tags = ["notification"],
//            }
//        ]));
//    }

//    private Task NotifyAdded(PipelineStepContext context, EnterpriseEnvironmentResource environment)
//    {
//        context.ReportingStep.Log(
//            LogLevel.Information,
//            $"**{TargetResource.Name}** has been added to **{environment.Name}**.",
//            enableMarkdown: true);

//        return Task.CompletedTask;
//    }

//    public EnterpriseEnvironmentResource Parent { get; }
//    public IResource TargetResource { get; }
//}

//public class EnterpriseProjectResource(
//    string name,
//    IResource resource,
//    EnterpriseEnvironmentResource enterpriseEnvironmentResource)
//    : EnterpriseServiceResource(name, resource, enterpriseEnvironmentResource);

//public class EnterpriseExternalResource(
//    string name,
//    IResource resource,
//    EnterpriseEnvironmentResource enterpriseEnvironmentResource)
//    : EnterpriseServiceResource(name, resource, enterpriseEnvironmentResource)
//{
//    public Uri Uri => ((ExternalServiceResource)TargetResource).Uri!;
//}

//public class EnterpriseContainerResource(
//    string name,
//    IResource resource,
//    EnterpriseEnvironmentResource enterpriseEnvironmentResource)
//    : EnterpriseServiceResource(name, resource, enterpriseEnvironmentResource)
//{
//    public (string Name,
//        string Source,
//        string Target,
//        ContainerMountType MountType,
//        bool ReadOnly)[] GetVolumes()
//    {
//        if (!TargetResource.TryGetContainerMounts(out var mounts))
//        {
//            return [];
//        }

//        return mounts.Select(x => (x.Source!, x.Source!, x.Target, x.Type, x.IsReadOnly)).ToArray();
//    }

//    public (string Scheme, int ExposedPort, int? InternalPort)[] GetPorts()
//    {
//        if (!TargetResource.TryGetEndpoints(out var endpoints))
//        {
//            return [];
//        }

//        return endpoints.Select(x => (x.UriScheme, x.Port ?? 80, x.TargetPort)).ToArray();
//    }
//}