using Microsoft.Extensions.Logging;
using PersonalSite.Core.Configuration;
using PersonalSite.Core.Paths;
using PersonalSite.Core.Services;

namespace PersonalSite.Content;

public sealed class ContentDiscoveryService(ILogger<ContentDiscoveryService> logger) : IContentDiscoveryService
{
    public Task<IReadOnlyList<string>> DiscoverMarkdownFilesAsync(
        SiteConfiguration site,
        string repositoryRoot,
        CancellationToken cancellationToken = default)
    {
        var contentRoot = RepositoryPaths.ResolveDirectory(repositoryRoot, site.ContentDirectory, nameof(site.ContentDirectory));
        if (!Directory.Exists(contentRoot))
        {
            throw new DirectoryNotFoundException($"Content directory was not found: {contentRoot}");
        }

        var files = Directory
            .EnumerateFiles(contentRoot, "*.md", SearchOption.AllDirectories)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        logger.LogInformation("Discovered {PageCount} Markdown content files.", files.Length);

        return Task.FromResult<IReadOnlyList<string>>(files);
    }
}
