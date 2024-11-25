using MawWww.Blog;
using MawWww.Models;

namespace MawWww.Pages.About;

public class NewsPageModel
    : MawPageModel
{
    readonly IBlogService _blogService;

    public IEnumerable<Post> Posts { get; set; } = [];

    public NewsPageModel (
        IBlogService blogService
    ) {
        ArgumentNullException.ThrowIfNull(blogService);

        _blogService = blogService;
    }

    public async Task OnGet()
    {
        Posts = await _blogService.GetLatestPostsAsync(_blogService.MawBlogId, 10);
    }
}
