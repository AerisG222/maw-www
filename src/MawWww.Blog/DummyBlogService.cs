namespace MawWww.Blog;

public class DummyBlogService
    : IBlogService
{
    readonly Guid _blogId = Guid.NewGuid();

    public Task<IEnumerable<Blog>> GetBlogsAsync()
    {
        return Task.FromResult(new Blog[] {
            new(_blogId, "Dummy Blog", "Copyright", "Description", new DateTime(2020, 01, 03))
        }.AsEnumerable());
    }

    public Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId)
    {
        return Task.FromResult(new Post[] {
            new Post(_blogId, Guid.NewGuid(), "Title", "Description", DateTime.Now)
        }.AsEnumerable());
    }

    public Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount)
    {
        return GetAllPostsAsync(blogId);
    }

    public Task AddPostAsync(Post post)
    {
        return Task.CompletedTask;
    }
}
