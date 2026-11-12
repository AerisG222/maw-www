using MawWww.Models;

namespace MawWww.Pages;

public sealed class ErrorPageModel
    : MawPageModel
{
    public int ErrorStatusCode { get; private set; }

    public void OnGet(int statusCode)
    {
        ErrorStatusCode = statusCode;
    }
}
