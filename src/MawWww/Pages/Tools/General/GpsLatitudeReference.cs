using System.ComponentModel.DataAnnotations;

namespace MawWww.Pages.Tools.General;

public enum GpsLatitudeReference
{
    [Display(Name = "North")]
    North,

    [Display(Name = "South (-)")]
    South
}
