namespace MawWww.Blog;

public record Blog
(
    Guid Id,
    string Title,
    string Copyright,
    string Description,
    DateTime LastPostDate
);
