namespace PersonalSite.Core.Paths;

public static class RepositoryPaths
{
    public static string ResolveDirectory(string repositoryRoot, string configuredPath, string settingName)
    {
        if (string.IsNullOrWhiteSpace(repositoryRoot))
        {
            throw new ArgumentException("Repository root cannot be empty.", nameof(repositoryRoot));
        }

        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            throw new InvalidOperationException($"Configuration value '{settingName}' cannot be empty.");
        }

        if (Path.IsPathRooted(configuredPath))
        {
            throw new InvalidOperationException($"Configuration value '{settingName}' must be relative to the repository root.");
        }

        var root = Path.GetFullPath(repositoryRoot);
        var resolved = Path.GetFullPath(Path.Combine(root, configuredPath));
        var relative = Path.GetRelativePath(root, resolved);

        if (relative == "." || relative.StartsWith("..", StringComparison.Ordinal) || Path.IsPathRooted(relative))
        {
            throw new InvalidOperationException($"Configuration value '{settingName}' must resolve inside the repository root.");
        }

        return resolved;
    }
}
