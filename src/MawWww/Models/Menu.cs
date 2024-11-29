namespace MawWww.Models;

public record Menu(
    MenuItem[] Items,
    string Title = ""
);
