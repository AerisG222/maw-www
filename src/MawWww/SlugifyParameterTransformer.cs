using System.Text.RegularExpressions;

namespace MawWww;

// https://stackoverflow.com/a/53838799
public partial class SlugifyParameterTransformer
    : IOutboundParameterTransformer
{
    [GeneratedRegex("([a-z])([A-Z])", RegexOptions.Singleline, "en-US")]
    private static partial Regex slugRegex();

    public string? TransformOutbound(object? value)
    {
        if (value == null)
        {
            return null;
        }

        var str = value.ToString();

        if (string.IsNullOrEmpty(str))
        {
            return null;
        }

        return slugRegex()
            .Replace(str, "$1-$2")
            .ToLower();
    }
}
