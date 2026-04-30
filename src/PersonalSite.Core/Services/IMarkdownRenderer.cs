namespace PersonalSite.Core.Services;

public interface IMarkdownRenderer
{
    string RenderToHtml(string markdown);
}
