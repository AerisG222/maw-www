using NodaTime;

namespace MawWww.Blog;

public class DummyBlogService
    : IBlogService
{
    public Guid MawBlogId { get; } = Guid.NewGuid();

    public Task<IEnumerable<Blog>> GetBlogsAsync()
    {
        return Task.FromResult(new Blog[] {
            new(MawBlogId, "Dummy Blog", "Copyright", "Description", new Instant())
        }.AsEnumerable());
    }

    public Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId)
    {
        return Task.FromResult(new Post[] {
            new(MawBlogId, Guid.NewGuid(), "Title", "Description", new Instant())
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
