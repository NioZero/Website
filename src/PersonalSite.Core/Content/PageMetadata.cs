namespace PersonalSite.Core.Content;

public sealed class PageMetadata
{
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Order { get; init; }
    public bool ShowInNavigation { get; init; }
    public string Layout { get; init; } = "Main";
    public string Template { get; init; } = "Page";
    public List<SocialLink> Social { get; init; } = new List<SocialLink>();
}
