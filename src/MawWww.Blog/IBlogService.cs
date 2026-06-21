namespace MawWww.Blog;

public interface IBlogService
{
    Guid MawBlogId { get; }

    Task<IEnumerable<Blog>> GetBlogsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount, CancellationToken cancellationToken = default);
    Task AddPostAsync(PostCreate post, CancellationToken cancellationToken = default);
}
