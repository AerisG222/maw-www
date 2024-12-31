using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Blog;
using MawWww.Models;

namespace MawWww.Pages.Admin;

public class CreateBlogPostPageModel
    : MawFormPageModel<CreateBlogPostForm>
{
    public const string BEHAVIOR_PREVIEW = "preview";
    public const string BEHAVIOR_SAVE = "save";

    readonly IBlogService _service;

    public CreateBlogPostPageModel(IBlogService service)
    {
        ArgumentNullException.ThrowIfNull(service);

        _service = service;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            if(Form.Behavior == BEHAVIOR_SAVE)
            {
                var post = new PostCreate(
                    Form.Title,
                    Form.Content
                );

                await _service.AddPostAsync(post);

                SubmitSuccess = true;
            }
        }

        return Page();
    }
}

public class CreateBlogPostForm
{
    [Required(ErrorMessage = "Please enter the title.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the content.")]
    public string Content { get; set; } = string.Empty;

    [Required()]
    public string Behavior { get; set; } = string.Empty;
}
