namespace MawWww.Blog;

public interface IBlogRepository
{
    Task<IEnumerable<Blog>> GetBlogsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount, CancellationToken cancellationToken = default);
    Task<Post?> GetPostAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddPostAsync(Post post, CancellationToken cancellationToken = default);
}
