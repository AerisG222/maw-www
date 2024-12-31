using Microsoft.Extensions.Caching.Hybrid;
using NodaTime;

namespace MawWww.Blog;

public class BlogService
    : IBlogService
{
    readonly IBlogRepository _repo;
    readonly IClock _clock;
    readonly HybridCache _cache;

    public Guid MawBlogId { get; } = new Guid("0193eaba-51c5-77b8-9f1e-181e2b818831");

    public BlogService(
        IBlogRepository repo,
        IClock clock,
        HybridCache cache
    ) {
        ArgumentNullException.ThrowIfNull(repo);
        ArgumentNullException.ThrowIfNull(clock);
        ArgumentNullException.ThrowIfNull(cache);

        _repo = repo;
        _clock = clock;
        _cache = cache;
    }

    public async Task<IEnumerable<Blog>> GetBlogsAsync()
    {
        return await _cache.GetOrCreateAsync(
            "blogs:all",
            async cancel => await _repo.GetBlogsAsync()
        );
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId)
    {
        return await _cache.GetOrCreateAsync(
            $"blogs:{blogId}:posts:all",
            async cancel => await _repo.GetAllPostsAsync(blogId),
            tags: [$"blogs:{blogId}:posts"]
        );
    }

    public async Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount)
    {
        return await _cache.GetOrCreateAsync(
            $"blogs:{blogId}:posts:latest:{postCount}",
            async cancel => await _repo.GetLatestPostsAsync(blogId, postCount),
            tags: [$"blogs:{blogId}:posts"]
        );
    }

    public async Task AddPostAsync(PostCreate post)
    {
        ArgumentNullException.ThrowIfNull(post);

        var newPost = new Post(
            Guid.CreateVersion7(),
            MawBlogId,
            post.Title,
            post.Content,
            _clock.GetCurrentInstant()
        );

        await _repo.AddPostAsync(newPost);
        await _cache.RemoveByTagAsync($"blogs:{newPost.BlogId}:posts");
    }
}
