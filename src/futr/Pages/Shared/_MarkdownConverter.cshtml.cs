namespace futr.Pages.Shared;

public class _MarkdownConverter
{
    public class ConversionOptions : Dictionary<string, object>
    {
    }

    public class Conversion
    {
        public string Target { get; set; }
        public ConversionOptions Options { get; set; } = new();

        public Conversion(string target)
        {
            Target = target;
        }
    }

    public class ConversionMap : Dictionary<string, Conversion>
    {
    }

    public class Model
    {
        public ConversionMap Conversions { get; set; } = new ConversionMap();
    }
}
