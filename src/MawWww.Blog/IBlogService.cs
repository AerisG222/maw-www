namespace MawWww.Blog;

public interface IBlogService
{
    Guid MawBlogId { get; }

    Task<IEnumerable<Blog>> GetBlogsAsync();
    Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId);
    Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount);
    Task AddPostAsync(Post post);
}
