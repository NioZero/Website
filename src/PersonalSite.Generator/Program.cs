using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalSite.Content;
using PersonalSite.Core.Configuration;
using PersonalSite.Core.Routing;
using PersonalSite.Core.Services;
using PersonalSite.Generator;
using PersonalSite.Output;
using PersonalSite.Rendering;

var repositoryRoot = ResolveRepositoryRoot();

var configuration = new ConfigurationBuilder()
    .SetBasePath(repositoryRoot)
    .AddJsonFile(Path.Combine("config", "site.json"), optional: false, reloadOnChange: false)
    .Build();

var site = configuration.Get<SiteConfiguration>();
if (site is null)
{
    throw new InvalidOperationException("Could not read site configuration from config/site.json.");
}

using var services = ConfigureServices();
var generator = services.GetRequiredService<IStaticSiteGenerator>();
await generator.GenerateAsync(site, repositoryRoot);

static ServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    services.AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss ";
        });
        builder.SetMinimumLevel(LogLevel.Information);
    });

    services.AddSingleton<IContentDiscoveryService, ContentDiscoveryService>();
    services.AddSingleton<IContentParser, MarkdownContentParser>();
    services.AddSingleton<IMarkdownRenderer, MarkdigMarkdownRenderer>();
    services.AddSingleton<IRouteBuilder, RouteBuilder>();
    services.AddSingleton<INavigationBuilder, NavigationBuilder>();
    services.AddSingleton<ITemplateRenderer, RazorLightTemplateRenderer>();
    services.AddSingleton<IOutputWriter, FileSystemOutputWriter>();
    services.AddSingleton<IStaticSiteGenerator, StaticSiteGenerator>();

    return services.BuildServiceProvider();
}

static string ResolveRepositoryRoot()
{
    var candidates = new[]
    {
        Directory.GetCurrentDirectory(),
        AppContext.BaseDirectory
    };

    foreach (var candidate in candidates)
    {
        var directory = new DirectoryInfo(candidate);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "PersonalSite.sln")) ||
                File.Exists(Path.Combine(directory.FullName, "config", "site.json")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }
    }

    throw new DirectoryNotFoundException("Could not locate the repository root. Run the generator from the solution root or a child directory.");
}
