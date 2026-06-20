using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using NodaTime;

namespace MawWww.Blog;

public class BlogService
    : IBlogService
{
    readonly IBlogRepository _repo;
    readonly IClock _clock;
    readonly HybridCache _cache;

    public Guid MawBlogId { get; }

    public BlogService(
        IBlogRepository repo,
        IClock clock,
        HybridCache cache,
        IOptions<BlogConfig> config
    ) {
        ArgumentNullException.ThrowIfNull(repo);
        ArgumentNullException.ThrowIfNull(clock);
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(config);

        _repo = repo;
        _clock = clock;
        _cache = cache;
        MawBlogId = config.Value.MawBlogId;
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
            tags: [BuildBlogPostsCacheTag(blogId)]
        );
    }

    public async Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount)
    {
        return await _cache.GetOrCreateAsync(
            $"blogs:{blogId}:posts:latest:{postCount}",
            async cancel => await _repo.GetLatestPostsAsync(blogId, postCount),
            tags: [BuildBlogPostsCacheTag(blogId)]
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
        await _cache.RemoveByTagAsync(BuildBlogPostsCacheTag(newPost.BlogId));
    }

    static string BuildBlogPostsCacheTag(Guid blogId) => $"blogs:{blogId}:posts";
}
