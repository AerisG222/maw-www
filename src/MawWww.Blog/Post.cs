using NodaTime;

namespace MawWww.Blog;

public record Post
(
    Guid Id,
    Guid BlogId,
    string Title,
    string Content,
    Instant PublishDate
);
