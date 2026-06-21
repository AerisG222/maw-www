using Dapper;
using Npgsql;

namespace MawWww.Blog;

[DapperAot]
public class BlogRepository
    : IBlogRepository
{
    readonly NpgsqlDataSource _dataSource;

    public BlogRepository(NpgsqlDataSource dataSource)
    {
        ArgumentNullException.ThrowIfNull(dataSource);

        _dataSource = dataSource;
    }

    public async Task<IEnumerable<Blog>> GetBlogsAsync(CancellationToken cancellationToken = default)
    {
        using var conn = await _dataSource.OpenConnectionAsync(cancellationToken);

        return await conn.QueryAsync<Blog>(
            new CommandDefinition("SELECT * FROM blog.get_blogs();", cancellationToken: cancellationToken)
        );
    }

    public Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId, CancellationToken cancellationToken = default)
    {
        return InternalGetPostsAsync(blogId, cancellationToken: cancellationToken);
    }

    public Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount, CancellationToken cancellationToken = default)
    {
        return InternalGetPostsAsync(blogId, postCount: postCount, cancellationToken: cancellationToken);
    }

    public async Task<Post?> GetPostAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await InternalGetPostsAsync(postId: id, cancellationToken: cancellationToken);

        return result.FirstOrDefault();
    }

    public async Task AddPostAsync(Post post, CancellationToken cancellationToken = default)
    {
        using var conn = await _dataSource.OpenConnectionAsync(cancellationToken);

        await conn.QuerySingleOrDefaultAsync<Guid>(
            new CommandDefinition(
                @"SELECT * FROM blog.add_post(
                    @id,
                    @blogId,
                    @title,
                    @content,
                    @publishDate
                );",
                new
                {
                    id = post.Id,
                    blogId = post.BlogId,
                    title = post.Title,
                    content = post.Content,
                    publishDate = post.PublishDate
                },
                cancellationToken: cancellationToken
            )
        );
    }

    async Task<IEnumerable<Post>> InternalGetPostsAsync(Guid? blogId = null, Guid? postId = null, short? postCount = null, CancellationToken cancellationToken = default)
    {
        using var conn = await _dataSource.OpenConnectionAsync(cancellationToken);

        return await conn.QueryAsync<Post>(
            new CommandDefinition(
                "SELECT id AS Id, blog_id AS BlogId, title AS Title, content AS Content, published as PublishDate FROM blog.get_posts(@blogId, @postId, @postCount);",
                new
                {
                    blogId,
                    postId,
                    postCount
                },
                cancellationToken: cancellationToken
            )
        );
    }
}
