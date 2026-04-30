using PersonalSite.Core.Rendering;

namespace PersonalSite.Core.Services;

public interface ITemplateRenderer
{
    Task<string> RenderPageAsync(RenderContext context, string repositoryRoot, CancellationToken cancellationToken = default);
}
