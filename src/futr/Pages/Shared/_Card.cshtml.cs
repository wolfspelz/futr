namespace futr.Pages.Shared;

public class Card
{
    public class Model
    {
        public string Text { get; set; } = "";
        public string Title { get; set; } = "";
        public string Link { get; set; } = "";
        public string Anchor { get; set; } = "";
        public bool TitleHasLink { get; set; } = false;
        public bool TextHasLink { get; set; } = false;
        public string ImgSrc { get; set; } = "";
        public string ImgAlt { get; set; } = "";
        public string ImgCaption { get; set; } = "";
    }
}
