namespace MawWww.Models;

public record PrimaryNavLink (
    string? Url,
    string Title,
    string? Icon = null,
    string? ImageUrl = null,
    string? AdditionalImageClasses = null,
    bool IsActive = false
);
