using MawWww.Models;

namespace MawWww.Pages.Scratch;

public static class ScratchMenu
{
    public static readonly Menu Menu = new (
        [
            new MenuItem("./DressUp",           "Dress Up"),
            new MenuItem("./FishyFishy",        "Fishy Fishy Cross My Ocean"),
            new MenuItem("./GoofyCarRace",      "Goofy Car Race"),
            new MenuItem("./HideAndSeek",       "Hide and Seek"),
            new MenuItem("./Hipnotize",         "Hipnotize"),
            new MenuItem("./KittyKittyScratch", "Kitty Kitty Scratch"),
            new MenuItem("./Maze",              "Maze"),
            new MenuItem("./Me",                "Me"),
            new MenuItem("./MooSic",            "Moo Sic"),
            new MenuItem("./Pong",              "Pong"),
            new MenuItem("./RaceCar2000",       "Race Car 2000"),
            new MenuItem("./SuperMan",          "Super Man"),
            new MenuItem("./Untitled",          "Untitled"),
            new MenuItem("./WhosFaster",        "Who's Faster"),
            new MenuItem("./WorkAndFade",       "Work and Fade"),
            new MenuItem("./YummyYummyBaby",    "Yummy Yummy Baby")
        ]
    );

    public static readonly IEnumerable<Menu> Menus = [Menu];
}
