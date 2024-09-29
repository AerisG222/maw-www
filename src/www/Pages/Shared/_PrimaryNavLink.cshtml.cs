namespace www.Pages.Shared;

public record PrimaryNavLink (
    string Url,
    string? Icon = null,
    string? ImageUrl = null,
    bool IsActive = false
);
