namespace PersonalSite.Core.Content;

public sealed class ContentPage
{
    public string SourcePath { get; init; } = string.Empty;
    public PageMetadata Metadata { get; init; } = new();
    public string MarkdownContent { get; init; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public string OutputPath { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
