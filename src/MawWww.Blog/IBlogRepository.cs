﻿namespace MawWww.Blog;

public interface IBlogRepository
{
    Task<IEnumerable<Blog>> GetBlogsAsync();
    Task<IEnumerable<Post>> GetAllPostsAsync(Guid blogId);
    Task<IEnumerable<Post>> GetLatestPostsAsync(Guid blogId, short postCount);
    Task<Post?> GetPostAsync(Guid id);
    Task AddPostAsync(Post post);
}