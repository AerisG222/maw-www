using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Calculators;

public class FileSizeModel
    : MawFormPageModel<FileSizeForm>
{
    public List<FileSizeResult> Results { get; private set; } = [];
    public IEnumerable<string> AllUnits = FileSizeUnit.AllUnits.Select(x => x.Name);

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAsync()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            SubmitSuccess = CalculateResults();
        }

        return Page();
    }

    bool CalculateResults()
    {
        var unit = FileSizeUnit.AllUnits
            .SingleOrDefault(x => string.Equals(x.Name, Form.Unit, StringComparison.OrdinalIgnoreCase));

        if (unit == null)
        {
            return false;
        }

        double sizeInBytes = Form.SizeOfFile * unit.BitsInUnit;

        foreach (var fsu in FileSizeUnit.AllUnits)
        {
            Results.Add(new FileSizeResult(fsu, sizeInBytes / fsu.BitsInUnit));
        }

        return true;
    }
}

public class FileSizeForm
{
    [Required(ErrorMessage = "Please enter size of file.")]
    public double SizeOfFile { get; set; } = 100;

    [Required(ErrorMessage = "Please select size unit.")]
    public string Unit { get; set; } = FileSizeUnit.UnitMB.Name;
}

public record FileSizeUnit (
    string Name,
    string Abbreviation,
    double BitsInUnit
) {
    public static readonly FileSizeUnit UnitBits =  new("Bits",       "b",   1);
    public static readonly FileSizeUnit UnitBytes = new("Bytes",      "B",   8);
    public static readonly FileSizeUnit UnitKB =    new("Kilobytes",  "KB",  8d * 1000);
    public static readonly FileSizeUnit UnitKiB =   new("Kibibytes",  "KiB", 8d * 1024);
    public static readonly FileSizeUnit UnitMB =    new("Megabytes",  "MB",  8d * 1000 * 1000);
    public static readonly FileSizeUnit UnitMiB =   new("Mebibytes",  "MiB", 8d * 1024 * 1024);
    public static readonly FileSizeUnit UnitGB =    new("Gigabytes",  "GB",  8d * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitGiB =   new("Gibibytes",  "GiB", 8d * 1024 * 1024 * 1024);
    public static readonly FileSizeUnit UnitTB =    new("Terabytes",  "TB",  8d * 1000 * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitTiB =   new("Tebibytes",  "TiB", 8d * 1024 * 1024 * 1024 * 1024);
    public static readonly FileSizeUnit UnitPB =    new("Petabytes",  "PB",  8d * 1000 * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitPiB =   new("Pebibytes",  "PiB", 8d * 1024 * 1024 * 1024 * 1024);
    public static readonly FileSizeUnit UnitEB =    new("Exabytes",   "EB",  8d * 1000 * 1000 * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitEiB =   new("Exbibytes",  "EiB", 8d * 1024 * 1024 * 1024 * 1024 * 1024);
    public static readonly FileSizeUnit UnitZB =    new("Zettabytes", "ZB",  8d * 1000 * 1000 * 1000 * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitZiB =   new("Zebibytes",  "ZiB", 8d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024);

    public static readonly FileSizeUnit[] AllUnits = [
            UnitBits,
            UnitBytes,
            UnitKB ,
            UnitKiB,
            UnitMB ,
            UnitMiB,
            UnitGB ,
            UnitGiB,
            UnitTB ,
            UnitTiB,
            UnitPB,
            UnitPiB,
            UnitEB ,
            UnitEiB,
            UnitZB,
            UnitZiB
        ];
}

public record FileSizeResult (
    FileSizeUnit Unit,
    double SizeInUnits
);
