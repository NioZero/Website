using Microsoft.Extensions.Logging;
using PersonalSite.Core.Content;
using PersonalSite.Core.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PersonalSite.Content;

public sealed class MarkdownContentParser(ILogger<MarkdownContentParser> logger) : IContentParser
{
    private readonly IDeserializer deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    public async Task<ContentPage> ParseAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Markdown file was not found: {filePath}", filePath);
        }

        var source = await File.ReadAllTextAsync(filePath, cancellationToken);
        var (metadata, markdown) = ParseDocument(source, filePath);
        metadata = ApplyMetadataDefaults(metadata, filePath);

        logger.LogDebug("Parsed content file {FilePath}.", filePath);

        return new ContentPage
        {
            SourcePath = filePath,
            Metadata = metadata,
            MarkdownContent = markdown
        };
    }

    private (PageMetadata Metadata, string Markdown) ParseDocument(string source, string filePath)
    {
        var normalized = source.TrimStart('\uFEFF').Replace("\r\n", "\n").Replace('\r', '\n');
        var lines = normalized.Split('\n');

        if (lines.Length == 0 || !string.Equals(lines[0].Trim(), "---", StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Markdown file must start with YAML front matter: {filePath}");
        }

        var endIndex = -1;
        for (var index = 1; index < lines.Length; index++)
        {
            if (string.Equals(lines[index].Trim(), "---", StringComparison.Ordinal))
            {
                endIndex = index;
                break;
            }
        }

        if (endIndex < 0)
        {
            throw new InvalidOperationException($"Markdown file has no closing YAML front matter delimiter: {filePath}");
        }

        var yaml = string.Join('\n', lines.Skip(1).Take(endIndex - 1));
        var markdown = string.Join('\n', lines.Skip(endIndex + 1));
        var metadata = deserializer.Deserialize<PageMetadata>(yaml) ?? new PageMetadata();

        return (metadata, markdown);
    }

    private static PageMetadata ApplyMetadataDefaults(PageMetadata metadata, string filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var title = string.IsNullOrWhiteSpace(metadata.Title) ? ToTitle(fileName) : metadata.Title;
        var slug = string.IsNullOrWhiteSpace(metadata.Slug) ? fileName : metadata.Slug;
        var layout = NormalizeTemplateName(metadata.Layout, "Main", nameof(metadata.Layout), filePath);
        var template = NormalizeTemplateName(metadata.Template, "Page", nameof(metadata.Template), filePath);

        return new PageMetadata
        {
            Title = title,
            Slug = slug,
            Description = metadata.Description,
            Order = metadata.Order,
            ShowInNavigation = metadata.ShowInNavigation,
            Layout = layout,
            Template = template
        };
    }

    private static string ToTitle(string value)
    {
        return string.Join(' ', value.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries)).Trim();
    }

    private static string NormalizeTemplateName(string value, string fallback, string fieldName, string filePath)
    {
        var templateName = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

        if (templateName.Contains('/') ||
            templateName.Contains('\\') ||
            templateName is "." or ".." ||
            templateName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new InvalidOperationException($"Markdown file '{filePath}' has invalid {fieldName} value '{value}'. Use a template file name without path separators.");
        }

        return Path.GetFileNameWithoutExtension(templateName);
    }
}
