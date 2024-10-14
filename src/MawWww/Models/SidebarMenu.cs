namespace MawWww.Models;

public record SidebarMenu(
    SidebarMenuItem[] Items,
    string Title = ""
);
