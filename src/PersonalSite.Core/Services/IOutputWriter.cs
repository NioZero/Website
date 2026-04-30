using PersonalSite.Core.Configuration;
using PersonalSite.Core.Content;

namespace PersonalSite.Core.Services;

public interface IOutputWriter
{
    Task CleanOutputDirectoryAsync(SiteConfiguration site, string repositoryRoot, CancellationToken cancellationToken = default);
    Task WritePageAsync(SiteConfiguration site, string repositoryRoot, ContentPage page, string html, CancellationToken cancellationToken = default);
    Task CopyAssetsAsync(SiteConfiguration site, string repositoryRoot, CancellationToken cancellationToken = default);
}
