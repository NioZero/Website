using PersonalSite.Core.Configuration;

namespace PersonalSite.Core.Services;

public interface IContentDiscoveryService
{
    Task<IReadOnlyList<string>> DiscoverMarkdownFilesAsync(
        SiteConfiguration site,
        string repositoryRoot,
        CancellationToken cancellationToken = default);
}
