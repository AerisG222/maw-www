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

    public async Task<IEnumerable<Blog>> GetBlogsAsync(CancellationToken cancellationToken = default)
    {
        return await _cache.GetOrCreateAsync(
            "blogs:all",
            async cancel => await _repo.GetBlogsAsync(cancel),
            cancellationToken: cancellationToken
        );
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId, CancellationToken cancellationToken = default)
    {
        return await _cache.GetOrCreateAsync(
            $"blogs:{blogId}:posts:all",
            async cancel => await _repo.GetAllPostsAsync(blogId, cancel),
            tags: [BuildBlogPostsCacheTag(blogId)],
            cancellationToken: cancellationToken
        );
    }

    public async Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount, CancellationToken cancellationToken = default)
    {
        return await _cache.GetOrCreateAsync(
            $"blogs:{blogId}:posts:latest:{postCount}",
            async cancel => await _repo.GetLatestPostsAsync(blogId, postCount, cancel),
            tags: [BuildBlogPostsCacheTag(blogId)],
            cancellationToken: cancellationToken
        );
    }

    public async Task AddPostAsync(PostCreate post, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(post);

        var newPost = new Post(
            Guid.CreateVersion7(),
            MawBlogId,
            post.Title,
            post.Content,
            _clock.GetCurrentInstant()
        );

        await _repo.AddPostAsync(newPost, cancellationToken);
        await _cache.RemoveByTagAsync(BuildBlogPostsCacheTag(newPost.BlogId), cancellationToken);
    }

    static string BuildBlogPostsCacheTag(Guid blogId) => $"blogs:{blogId}:posts";
}
