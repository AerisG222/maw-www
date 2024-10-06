namespace MawWww.Blog;

public record Post
(
    Guid Id,
    Guid BlogId,
    string Title,
    string Description,
    DateTime PublishDate
);
