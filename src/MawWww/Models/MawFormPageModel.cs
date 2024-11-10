using Microsoft.AspNetCore.Mvc;

namespace MawWww.Models;

public class MawFormPageModel
    : MawPageModel
{
    public bool SubmitAttempted { get; protected set; }
    public bool SubmitSuccess { get; protected set; }
}

public class MawFormPageModel<T>
    : MawFormPageModel where T : new()
{
    [BindProperty]
    public T Form { get; set; } = new();
}
