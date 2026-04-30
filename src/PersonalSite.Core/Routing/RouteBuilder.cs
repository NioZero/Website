using PersonalSite.Core.Configuration;
using PersonalSite.Core.Content;
using PersonalSite.Core.Paths;
using PersonalSite.Core.Services;

namespace PersonalSite.Core.Routing;

public sealed class RouteBuilder : IRouteBuilder
{
    public string BuildUrl(ContentPage page)
    {
        var slug = NormalizeSlug(page.Metadata.Slug);

        if (string.IsNullOrWhiteSpace(slug) || string.Equals(slug, "index", StringComparison.OrdinalIgnoreCase))
        {
            return "/";
        }

        return $"/{slug}/";
    }

    public string BuildOutputPath(SiteConfiguration site, string repositoryRoot, ContentPage page)
    {
        var slug = NormalizeSlug(page.Metadata.Slug);
        var outputRoot = RepositoryPaths.ResolveDirectory(repositoryRoot, site.OutputDirectory, nameof(site.OutputDirectory));

        if (string.IsNullOrWhiteSpace(slug) || string.Equals(slug, "index", StringComparison.OrdinalIgnoreCase))
        {
            return Path.Combine(outputRoot, "index.html");
        }

        var pathSegments = slug.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return Path.Combine(pathSegments.Prepend(outputRoot).Append("index.html").ToArray());
    }

    private static string NormalizeSlug(string slug)
    {
        var normalized = (slug ?? string.Empty).Trim().Trim('/').Replace('\\', '/');

        if (normalized.Contains(':', StringComparison.Ordinal) ||
            normalized.Contains('?', StringComparison.Ordinal) ||
            normalized.Contains('#', StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"The slug '{slug}' contains characters that are not valid for static routes.");
        }

        if (Path.IsPathRooted(normalized))
        {
            throw new InvalidOperationException($"The slug '{slug}' is invalid because rooted paths are not allowed.");
        }

        var segments = normalized.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Any(segment => segment is "." or ".." || segment.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0))
        {
            throw new InvalidOperationException($"The slug '{slug}' contains an invalid path segment.");
        }

        return normalized.ToLowerInvariant();
    }
}
