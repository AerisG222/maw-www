namespace www.Models;

public record SidebarMenu(
    SidebarMenuItem[] Items,
    string Title = ""
);
