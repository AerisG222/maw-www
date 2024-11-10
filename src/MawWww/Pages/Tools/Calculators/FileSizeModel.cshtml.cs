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
    public double SizeOfFile { get; set; } = 0;

    [Required(ErrorMessage = "Please select size unit.")]
    public string Unit { get; set; } = FileSizeUnit.UnitMB.Name;
}

public record FileSizeUnit (
    string Name,
    string Abbreviation,
    double BitsInUnit
) {
    public static readonly FileSizeUnit UnitBits =  new("Bits",      "b",   1);
    public static readonly FileSizeUnit UnitBytes = new("Bytes",     "B",   8);
    public static readonly FileSizeUnit UnitKB =    new("Kilobyte",  "KB",  8d * 1000);
    public static readonly FileSizeUnit UnitKiB =   new("Kibibyte",  "KiB", 8d * 1024);
    public static readonly FileSizeUnit UnitMB =    new("Megabyte",  "MB",  8d * 1000 * 1000);
    public static readonly FileSizeUnit UnitMiB =   new("Mebibyte",  "MiB", 8d * 1024 * 1024);
    public static readonly FileSizeUnit UnitGB =    new("Gigabyte",  "GB",  8d * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitGiB =   new("Gibibyte",  "GiB", 8d * 1024 * 1024 * 1024);
    public static readonly FileSizeUnit UnitTB =    new("Terabyte",  "TB",  8d * 1000 * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitTiB =   new("Tebibyte",  "TiB", 8d * 1024 * 1024 * 1024 * 1024);
    public static readonly FileSizeUnit UnitPB =    new("Petabyte",  "PB",  8d * 1000 * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitPiB =   new("Pebibyte",  "PiB", 8d * 1024 * 1024 * 1024 * 1024);
    public static readonly FileSizeUnit UnitEB =    new("Exabyte",   "EB",  8d * 1000 * 1000 * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitEiB =   new("Exbibyte",  "EiB", 8d * 1024 * 1024 * 1024 * 1024 * 1024);
    public static readonly FileSizeUnit UnitZB =    new("Zettabyte", "ZB",  8d * 1000 * 1000 * 1000 * 1000 * 1000 * 1000);
    public static readonly FileSizeUnit UnitZiB =   new("Zebibyte",  "ZiB", 8d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024);

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
