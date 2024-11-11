using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Calculators;

public class BandwidthModel
    : MawFormPageModel<BandwidthForm>
{
    public List<BandwidthResult> Results { get; private set; } = [];
    public IEnumerable<string> AllFileSizeUnits = FileSizeUnit.AllUnits.Select(x => x.Name);
    public IEnumerable<string> AllTimeScales = TimeScale.AllScales.Select(x => x.Name);

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
        var sizeUnit = FileSizeUnit.AllUnits
            .SingleOrDefault(x => string.Equals(x.Name, Form.SizeUnit, StringComparison.OrdinalIgnoreCase));
        var timeScale = TimeScale.AllScales
            .SingleOrDefault(x => string.Equals(x.Name, Form.Scale, StringComparison.OrdinalIgnoreCase));

        if (sizeUnit == null || timeScale == null)
        {
            return false;
        }

        double sizeInBits = Form.Size * sizeUnit.BitsInUnit;

        foreach (var bsi in BandwidthSizeInfo.AllSizes)
        {
            Results.Add(new BandwidthResult(bsi,  sizeInBits / bsi.Bps / timeScale.SecondsInUnit));
        }

        return true;
    }
}

public class BandwidthForm
{
    [Required(ErrorMessage = "Please enter size of file.")]
    public double Size { get; set; } = 100;

    [Required(ErrorMessage = "Please select size unit.")]
    public string SizeUnit { get; set; } = FileSizeUnit.UnitMB.Name;

    [Required(ErrorMessage = "Please select time unit.")]
    public string Scale { get; set; } = TimeScale.Seconds.Name;
}

public record BandwidthSizeInfo (
    string Name,
    string Speed,
    long Bps
) {
    public static readonly BandwidthSizeInfo Modem14 =      new("Modem",             "14.4 Kbps",         14_400);
    public static readonly BandwidthSizeInfo Modem28 =      new("Modem",             "28.8 Kbps",         28_800);
    public static readonly BandwidthSizeInfo Modem33 =      new("Modem",             "33.6 Kbps",         33_600);
    public static readonly BandwidthSizeInfo Modem56 =      new("Modem",             "56.6 Kbps",         56_600);
    public static readonly BandwidthSizeInfo ISDN =         new("ISDN",              "64 Kbps",           64_000);
    public static readonly BandwidthSizeInfo CableDsl128 =  new("Cable/DSL",         "128 Kbps",         128_000);
    public static readonly BandwidthSizeInfo CableDsl256 =  new("Cable/DSL",         "256 Kbps",         256_000);
    public static readonly BandwidthSizeInfo CableDsl512 =  new("Cable/DSL",         "512 Kbps",         512_000);
    public static readonly BandwidthSizeInfo T1 =           new("T1",                "1.54 Mbps",      1_540_000);
    public static readonly BandwidthSizeInfo Ethernet =     new("Ethernet",          "10 Mbps",       10_000_000);
    public static readonly BandwidthSizeInfo WirelessB =    new("WiFi 802.11b",      "11 Mbps",       11_000_000);
    public static readonly BandwidthSizeInfo T3 =           new("T3",                "45 Mbps",       45_000_000);
    public static readonly BandwidthSizeInfo WirelessG =    new("WiFi 802.11g",      "54 Mbps",       54_000_000);
    public static readonly BandwidthSizeInfo FastEthernet = new("Fast Ethernet",     "100 Mbps",     100_000_000);
    public static readonly BandwidthSizeInfo OC3 =          new("OC3",               "155 Mbps",     155_000_000);
    public static readonly BandwidthSizeInfo WirelessN =    new("WiFi 802.11n",      "600 Mbps",     600_000_000);
    public static readonly BandwidthSizeInfo OC12 =         new("OC12",              "622 Mbps",     622_000_000);
    public static readonly BandwidthSizeInfo GigE =         new("GigE",              "1 Gbps",     1_000_000_000);
    public static readonly BandwidthSizeInfo WirelessAc =   new("WiFi 802.11ac",     "1.3 Gbps",   1_300_000_000);
    public static readonly BandwidthSizeInfo OC48 =         new("OC48",              "2.4 Gbps",   2_400_000_000);
    public static readonly BandwidthSizeInfo SsUsb =        new("SuperSpeed USB",    "5 Gbps",     5_000_000_000);
    public static readonly BandwidthSizeInfo WirelessAd =   new("WiFi 802.11ad",     "7 Gbps",     7_000_000_000);
    public static readonly BandwidthSizeInfo OC192 =        new("OC192",             "9.4 Gbps",   9_400_000_000);
    public static readonly BandwidthSizeInfo TenGigE =      new("10 GigE",           "10 Gbps",   10_000_000_000);
    public static readonly BandwidthSizeInfo SsUsb20 =      new("SuperSpeed USB 20", "20 Gbps",   20_000_000_000);
    public static readonly BandwidthSizeInfo Thunderbolt3 = new("Thunderbolt 3",     "40 Gbps",   40_000_000_000);
    public static readonly BandwidthSizeInfo HunGigE =      new("100 GigE",          "100 Gbps", 100_000_000_000);

    public static readonly BandwidthSizeInfo[] AllSizes = [
        HunGigE,
        Thunderbolt3,
        SsUsb20,
        TenGigE,
        OC192,
        WirelessAd,
        SsUsb,
        OC48,
        WirelessAc,
        GigE,
        OC12,
        WirelessN,
        OC3,
        FastEthernet,
        WirelessG,
        T3,
        WirelessB,
        Ethernet,
        T1,
        CableDsl512,
        CableDsl256,
        CableDsl128,
        ISDN,
        Modem56,
        Modem33,
        Modem28,
        Modem14
    ];
}

public record BandwidthResult (
    BandwidthSizeInfo BandwidthSizeInfo,
    double Time
);
