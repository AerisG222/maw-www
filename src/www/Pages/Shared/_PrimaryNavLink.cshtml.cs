namespace www.Pages.Shared;

public record PrimaryNavLink (
    string Url,
    string Title,
    string? Icon = null,
    string? ImageUrl = null,
    bool IsActive = false
);
