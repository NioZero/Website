using Microsoft.Extensions.Logging;
using PersonalSite.Core.Paths;
using PersonalSite.Core.Rendering;
using PersonalSite.Core.Services;
using RazorLight;

namespace PersonalSite.Rendering;

public sealed class RazorLightTemplateRenderer(ILogger<RazorLightTemplateRenderer> logger) : ITemplateRenderer
{
    private readonly Dictionary<string, RazorLightEngine> engines = new(StringComparer.OrdinalIgnoreCase);
    private readonly object lockObject = new();

    public async Task<string> RenderPageAsync(RenderContext context, string repositoryRoot, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var templatesRoot = RepositoryPaths.ResolveDirectory(repositoryRoot, context.Site.TemplatesDirectory, nameof(context.Site.TemplatesDirectory));
        if (!Directory.Exists(templatesRoot))
        {
            throw new DirectoryNotFoundException($"Templates directory was not found: {templatesRoot}");
        }

        var engine = GetEngine(templatesRoot);

        var pageTemplate = NormalizeTemplateName(context.Page.Metadata.Template, "Page", "template");
        var templateKey = $"Pages/{pageTemplate}.cshtml";

        logger.LogDebug("Rendering {PageTitle} with template {TemplateKey}.", context.Page.Metadata.Title, templateKey);

        return await engine.CompileRenderAsync(templateKey, context);
    }

    private RazorLightEngine GetEngine(string templatesRoot)
    {
        lock (lockObject)
        {
            if (engines.TryGetValue(templatesRoot, out var engine))
            {
                return engine;
            }

            engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(templatesRoot)
                .UseMemoryCachingProvider()
                .Build();

            engines.Add(templatesRoot, engine);
            return engine;
        }
    }

    private static string NormalizeTemplateName(string value, string fallback, string fieldName)
    {
        var templateName = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

        if (templateName.Contains('/') ||
            templateName.Contains('\\') ||
            templateName is "." or ".." ||
            templateName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new InvalidOperationException($"The page {fieldName} '{value}' is invalid. Use a template file name without path separators.");
        }

        return Path.GetFileNameWithoutExtension(templateName);
    }
}
