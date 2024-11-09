using MawWww.Models;

namespace MawWww.Pages.Scratch;

public static class ScratchMenu
{
    public static readonly SidebarMenu Menu = new (
        [
            new SidebarMenuItem("./DressUp",           "Dress Up"),
            new SidebarMenuItem("./FishyFishy",        "Fishy Fishy Cross My Ocean"),
            new SidebarMenuItem("./GoofyCarRace",      "Goofy Car Race"),
            new SidebarMenuItem("./HideAndSeek",       "Hide and Seek"),
            new SidebarMenuItem("./Hipnotize",         "Hipnotize"),
            new SidebarMenuItem("./KittyKittyScratch", "Kitty Kitty Scratch"),
            new SidebarMenuItem("./Maze",              "Maze"),
            new SidebarMenuItem("./Me",                "Me"),
            new SidebarMenuItem("./MooSic",            "Moo Sic"),
            new SidebarMenuItem("./Pong",              "Pong"),
            new SidebarMenuItem("./RaceCar2000",       "Race Car 2000"),
            new SidebarMenuItem("./SuperMan",          "Super Man"),
            new SidebarMenuItem("./Untitled",          "Untitled"),
            new SidebarMenuItem("./WhosFaster",        "Who's Faster"),
            new SidebarMenuItem("./WorkAndFade",       "Work and Fade"),
            new SidebarMenuItem("./YummyYummyBaby",    "Yummy Yummy Baby")
        ]
    );
}
