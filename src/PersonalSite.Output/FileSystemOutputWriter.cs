using System.Text;
using Microsoft.Extensions.Logging;
using PersonalSite.Core.Configuration;
using PersonalSite.Core.Content;
using PersonalSite.Core.Paths;
using PersonalSite.Core.Services;

namespace PersonalSite.Output;

public sealed class FileSystemOutputWriter(ILogger<FileSystemOutputWriter> logger) : IOutputWriter
{
    public Task CleanOutputDirectoryAsync(SiteConfiguration site, string repositoryRoot, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var outputRoot = ResolveOutputRoot(site, repositoryRoot);

        if (Directory.Exists(outputRoot))
        {
            Directory.Delete(outputRoot, recursive: true);
        }

        Directory.CreateDirectory(outputRoot);
        logger.LogInformation("Cleaned output directory: {OutputDirectory}", outputRoot);

        return Task.CompletedTask;
    }

    public async Task WritePageAsync(SiteConfiguration site, string repositoryRoot, ContentPage page, string html, CancellationToken cancellationToken = default)
    {
        var directory = Path.GetDirectoryName(page.OutputPath);
        if (string.IsNullOrWhiteSpace(directory))
        {
            throw new InvalidOperationException($"Could not resolve output directory for page '{page.Metadata.Title}'.");
        }

        Directory.CreateDirectory(directory);
        await File.WriteAllTextAsync(page.OutputPath, html, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), cancellationToken);

        logger.LogInformation("Wrote {Url} -> {OutputPath}", page.Url, page.OutputPath);
    }

    public async Task CopyAssetsAsync(SiteConfiguration site, string repositoryRoot, CancellationToken cancellationToken = default)
    {
        var assetsRoot = RepositoryPaths.ResolveDirectory(repositoryRoot, site.AssetsDirectory, nameof(site.AssetsDirectory));
        if (!Directory.Exists(assetsRoot))
        {
            logger.LogWarning("Assets directory was not found and will be skipped: {AssetsDirectory}", assetsRoot);
            return;
        }

        var destinationRoot = Path.Combine(ResolveOutputRoot(site, repositoryRoot), "assets");
        Directory.CreateDirectory(destinationRoot);

        foreach (var sourceFile in Directory.EnumerateFiles(assetsRoot, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var relativePath = Path.GetRelativePath(assetsRoot, sourceFile);
            var destinationFile = Path.Combine(destinationRoot, relativePath);
            var destinationDirectory = Path.GetDirectoryName(destinationFile);

            if (!string.IsNullOrWhiteSpace(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            await using var source = File.OpenRead(sourceFile);
            await using var destination = File.Create(destinationFile);
            await source.CopyToAsync(destination, cancellationToken);
        }

        logger.LogInformation("Copied static assets to {AssetsOutputDirectory}", destinationRoot);
    }

    private static string ResolveOutputRoot(SiteConfiguration site, string repositoryRoot)
    {
        var outputRoot = RepositoryPaths.ResolveDirectory(repositoryRoot, site.OutputDirectory, nameof(site.OutputDirectory));
        var root = NormalizePath(repositoryRoot);
        var output = NormalizePath(outputRoot);

        if (string.Equals(root, output, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The output directory cannot be the repository root.");
        }

        var sourceDirectories = new[]
        {
            (Path: RepositoryPaths.ResolveDirectory(repositoryRoot, site.ContentDirectory, nameof(site.ContentDirectory)), Name: nameof(site.ContentDirectory)),
            (Path: RepositoryPaths.ResolveDirectory(repositoryRoot, site.TemplatesDirectory, nameof(site.TemplatesDirectory)), Name: nameof(site.TemplatesDirectory)),
            (Path: RepositoryPaths.ResolveDirectory(repositoryRoot, site.AssetsDirectory, nameof(site.AssetsDirectory)), Name: nameof(site.AssetsDirectory))
        };

        foreach (var source in sourceDirectories)
        {
            if (PathsOverlap(outputRoot, source.Path))
            {
                throw new InvalidOperationException($"The output directory cannot overlap with '{source.Name}'. Generated output must be disposable.");
            }
        }

        return outputRoot;
    }

    private static bool PathsOverlap(string first, string second)
    {
        return IsSameOrChild(first, second) || IsSameOrChild(second, first);
    }

    private static bool IsSameOrChild(string candidate, string parent)
    {
        var normalizedCandidate = NormalizePath(candidate);
        var normalizedParent = NormalizePath(parent);

        return string.Equals(normalizedCandidate, normalizedParent, StringComparison.OrdinalIgnoreCase) ||
            normalizedCandidate.StartsWith(normalizedParent + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizePath(string path)
    {
        return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }
}
