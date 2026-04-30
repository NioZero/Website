using PersonalSite.Core.Content;
using PersonalSite.Core.Navigation;
using PersonalSite.Core.Services;

namespace PersonalSite.Core.Routing;

public sealed class NavigationBuilder : INavigationBuilder
{
    public IReadOnlyList<NavigationItem> BuildNavigation(ContentPage activePage, IReadOnlyList<ContentPage> pages)
    {
        return pages
            .Where(page => page.Metadata.ShowInNavigation)
            .OrderBy(page => page.Metadata.Order)
            .ThenBy(page => page.Metadata.Title, StringComparer.OrdinalIgnoreCase)
            .Select(page => new NavigationItem
            {
                Title = page.Metadata.Title,
                Url = page.Url,
                Order = page.Metadata.Order,
                IsActive = string.Equals(page.Url, activePage.Url, StringComparison.OrdinalIgnoreCase)
            })
            .ToArray();
    }
}
