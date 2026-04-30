namespace PersonalSite.Core.Navigation;

public sealed class NavigationItem
{
    public string Title { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public int Order { get; init; }
    public bool IsActive { get; init; }
}
