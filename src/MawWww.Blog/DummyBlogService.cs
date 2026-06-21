using NodaTime;

namespace MawWww.Blog;

public class DummyBlogService
    : IBlogService
{
    public Guid MawBlogId { get; } = Guid.NewGuid();

    public Task<IEnumerable<Blog>> GetBlogsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Blog[] {
            new(MawBlogId, "Dummy Blog", "Copyright", "Description", new Instant())
        }.AsEnumerable());
    }

    public Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Post[] {
            new(MawBlogId, Guid.NewGuid(), "Title", "Description", new Instant())
        }.AsEnumerable());
    }

    public Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount, CancellationToken cancellationToken = default)
    {
        return GetAllPostsAsync(blogId, cancellationToken);
    }

    public Task AddPostAsync(PostCreate post, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
