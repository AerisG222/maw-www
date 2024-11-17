using System.ComponentModel.DataAnnotations;

namespace MawWww.Pages.Tools.General;

public enum GpsLongitudeReference
{
    [Display(Name = "East")]
    East,

    [Display(Name = "West (-)")]
    West
}
