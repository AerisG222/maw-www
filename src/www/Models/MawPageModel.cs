using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace www.Models;

public class MawPageModel
    : PageModel
{
    const string _baseTitle = "mikeandwan.us";

    public string? Title { get; set; }
    public string HtmlPageTitle { get; private set; } = _baseTitle;
    public PrimaryNavArea Area { get; set; } = PrimaryNavArea.None;

    public void SetHtmlPageTitle(params object[] parts)
    {
        var sb = new StringBuilder(_baseTitle);

        foreach(var part in parts)
        {
            if(part != null)
            {
                sb.Append($" | {part.ToString().ToLower()}");
            }
        }

        HtmlPageTitle = sb.ToString();
    }
}
