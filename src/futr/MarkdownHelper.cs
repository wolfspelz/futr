using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace futr;

public static class MarkdownHelper
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .Use<HeadingLevelExtension>()
        .Build();

    public static string ToHtml(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;
        return Markdown.ToHtml(markdown, Pipeline);
    }
}

public class HeadingLevelExtension : IMarkdownExtension
{
    public int Offset { get; set; } = 2;

    public void Setup(MarkdownPipelineBuilder pipeline) { }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            var headingRenderer = htmlRenderer.ObjectRenderers.FindExact<HeadingRenderer>();
            if (headingRenderer != null)
            {
                htmlRenderer.ObjectRenderers.Remove(headingRenderer);
                htmlRenderer.ObjectRenderers.Add(new OffsetHeadingRenderer(Offset));
            }
        }
    }
}

public class OffsetHeadingRenderer : HeadingRenderer
{
    private readonly int _offset;

    public OffsetHeadingRenderer(int offset)
    {
        _offset = offset;
    }

    protected override void Write(HtmlRenderer renderer, HeadingBlock obj)
    {
        var originalLevel = obj.Level;
        obj.Level = Math.Min(obj.Level + _offset, 6);
        base.Write(renderer, obj);
        obj.Level = originalLevel;
    }
}
