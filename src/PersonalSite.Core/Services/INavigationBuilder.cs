using PersonalSite.Core.Content;
using PersonalSite.Core.Navigation;

namespace PersonalSite.Core.Services;

public interface INavigationBuilder
{
    IReadOnlyList<NavigationItem> BuildNavigation(ContentPage activePage, IReadOnlyList<ContentPage> pages);
}
