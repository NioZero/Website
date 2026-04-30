namespace PersonalSite.Core.Configuration;

public sealed class SiteConfiguration
{
    public string SiteName { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = string.Empty;
    public string Language { get; init; } = "en";
    public string Author { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ContentDirectory { get; init; } = "content";
    public string TemplatesDirectory { get; init; } = "templates";
    public string AssetsDirectory { get; init; } = "assets";
    public string OutputDirectory { get; init; } = "output";
}
