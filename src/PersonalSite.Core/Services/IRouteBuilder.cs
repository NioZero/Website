using PersonalSite.Core.Configuration;
using PersonalSite.Core.Content;

namespace PersonalSite.Core.Services;

public interface IRouteBuilder
{
    string BuildUrl(ContentPage page);
    string BuildOutputPath(SiteConfiguration site, string repositoryRoot, ContentPage page);
}
