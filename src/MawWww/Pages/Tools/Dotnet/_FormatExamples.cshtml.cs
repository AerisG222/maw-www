namespace MawWww.Pages.Tools.Dotnet;

public record FormatExample (
    string FormatString,
    string Result
);

public record FormatExampleGroup (
    string Name,
    IEnumerable<FormatExample> Examples
);
