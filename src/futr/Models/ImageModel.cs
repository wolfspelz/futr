namespace futr.Models;

public class ImageModel
{
    public string Link { get; internal set; } = "";
    public string Text { get; internal set; } = "";
    public string Page { get; internal set; } = "";
    public string Author { get; internal set; } = "";
    public string License { get; internal set; } = "";
    public string Legal { get; internal set; } = "";
    public List<string> Tags { get; internal set; } = new();
}
