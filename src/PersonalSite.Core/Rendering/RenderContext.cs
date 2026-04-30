using PersonalSite.Core.Configuration;
using PersonalSite.Core.Content;
using PersonalSite.Core.Navigation;

namespace PersonalSite.Core.Rendering;

public sealed class RenderContext
{
    public SiteConfiguration Site { get; init; } = new();
    public ContentPage Page { get; init; } = new();
    public IReadOnlyList<ContentPage> Pages { get; init; } = Array.Empty<ContentPage>();
    public IReadOnlyList<NavigationItem> Navigation { get; init; } = Array.Empty<NavigationItem>();
}
