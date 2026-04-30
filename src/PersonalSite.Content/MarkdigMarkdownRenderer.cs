using Markdig;
using PersonalSite.Core.Services;

namespace PersonalSite.Content;

public sealed class MarkdigMarkdownRenderer : IMarkdownRenderer
{
    private readonly MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public string RenderToHtml(string markdown)
    {
        return Markdown.ToHtml(markdown ?? string.Empty, pipeline);
    }
}
