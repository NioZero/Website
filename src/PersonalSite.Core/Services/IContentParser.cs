using PersonalSite.Core.Content;

namespace PersonalSite.Core.Services;

public interface IContentParser
{
    Task<ContentPage> ParseAsync(string filePath, CancellationToken cancellationToken = default);
}
