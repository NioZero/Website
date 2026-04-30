using PersonalSite.Core.Configuration;

namespace PersonalSite.Core.Services;

public interface IStaticSiteGenerator
{
    Task GenerateAsync(SiteConfiguration site, string repositoryRoot, CancellationToken cancellationToken = default);
}
