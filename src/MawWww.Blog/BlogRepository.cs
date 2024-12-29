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

    public async Task<IEnumerable<Blog>> GetBlogsAsync()
    {
        using var conn = await _dataSource.OpenConnectionAsync();

        return await conn.QueryAsync<Blog>("SELECT * FROM blog.get_blogs();");
    }

    public Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId)
    {
        return InternalGetPostsAsync(blogId);
    }

    public Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount)
    {
        return InternalGetPostsAsync(blogId, postCount: postCount);
    }

    public async Task<Post?> GetPostAsync(Guid id)
    {
        var result = await InternalGetPostsAsync(postId: id);

        return result.FirstOrDefault();
    }

    public async Task AddPostAsync(Post post)
    {
        using var conn = await _dataSource.OpenConnectionAsync();

        var result = await conn.QuerySingleOrDefaultAsync<short>(
            @"SELECT * FROM blog.add_post(
                @id,
                @blogId,
                @title,
                @description,
                @publishDate
            );",
            new
            {
                id = post.Id,
                blogId = post.BlogId,
                title = post.Title,
                description = post.Description,
                publishDate = post.PublishDate
            });

        if (result <= 0)
        {
            throw new Exception("Did not save blog post!");
        }
    }

    async Task<IEnumerable<Post>> InternalGetPostsAsync(Guid? blogId = null, Guid? postId = null, short? postCount = null)
    {
        using var conn = await _dataSource.OpenConnectionAsync();

        return await conn.QueryAsync<Post>(
            "SELECT * FROM blog.get_posts(@blogId, @postId, @postCount);",
            new
            {
                blogId,
                postId,
                postCount
            }
        );
    }
}
