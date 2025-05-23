using System.Globalization;

namespace MawWww.Pages.Tools.General;

#pragma warning disable CA1721
public class GpsCoordinate
{
    public float LatitudeDegrees { get; private set; }
    public float LongitudeDegrees { get; private set; }
    public float LatitudeMinutes { get; private set; }
    public float LongitudeMinutes { get; private set; }
    public float LatitudeSeconds { get; private set; }
    public float LongitudeSeconds { get; private set; }

    public GpsLatitudeReference LatitudeRef
    {
        get
        {
            if (LatitudeDegrees < 0.0f)
            {
                return GpsLatitudeReference.South;
            }

            return GpsLatitudeReference.North;
        }
    }

    public GpsLongitudeReference LongitudeRef
    {
        get
        {
            if (LongitudeDegrees < 0.0f)
            {
                return GpsLongitudeReference.West;
            }

            return GpsLongitudeReference.East;
        }
    }

    public GpsCoordinate(float latDegrees, float latMinutes, float latSeconds, float lngDegrees, float lngMinutes, float lngSeconds)
    {
        LatitudeDegrees = latDegrees;
        LatitudeMinutes = latMinutes;
        LatitudeSeconds = latSeconds;

        LongitudeDegrees = lngDegrees;
        LongitudeMinutes = lngMinutes;
        LongitudeSeconds = lngSeconds;
    }

    public GpsCoordinate(float latDegrees, float latMinutes, float lngDegrees, float lngMinutes)
    {
        GetDegreesMinutesSeconds(latDegrees, latMinutes, out float degrees, out float minutes, out float seconds);

        LatitudeDegrees = degrees;
        LatitudeMinutes = minutes;
        LatitudeSeconds = seconds;

        GetDegreesMinutesSeconds(lngDegrees, lngMinutes, out degrees, out minutes, out seconds);

        LongitudeDegrees = degrees;
        LongitudeMinutes = minutes;
        LongitudeSeconds = seconds;
    }

    public GpsCoordinate(float latDegrees, float lngDegrees)
    {
        GetDegreesMinutesSeconds(latDegrees, out float degrees, out float minutes, out float seconds);

        LatitudeDegrees = degrees;
        LatitudeMinutes = minutes;
        LatitudeSeconds = seconds;

        GetDegreesMinutesSeconds(lngDegrees, out degrees, out minutes, out seconds);

        LongitudeDegrees = degrees;
        LongitudeMinutes = minutes;
        LongitudeSeconds = seconds;
    }

    public static float GetDegrees(float degrees, float minutes, float seconds)
    {
        float sign = 1.0f;

        if (degrees < 0.0f)
        {
            sign = -1.0f;
        }

        return sign * (Math.Abs(degrees) + (minutes / 60.0f) + (seconds / 3600.0f));
    }

    public static float GetDegrees(float degrees, float minutes)
    {
        float sign = 1.0f;

        if (degrees < 0.0f)
        {
            sign = -1.0f;
        }

        return sign * (Math.Abs(degrees) + (minutes / 60.0f));
    }

    public static void GetDegreesMinutes(float degreeDecimals, out float degrees, out float minutes)
    {
        degrees = (float)Math.Truncate((double)degreeDecimals);
        minutes = (Math.Abs(degreeDecimals) - Math.Abs(degrees)) * 60.0f;
    }

    public static void GetDegreesMinutes(float degreeDecimals, float degreeMinutes, float degreeSeconds, out float degrees, out float minutes)
    {
        degrees = degreeDecimals;
        minutes = Math.Abs(degreeMinutes) + (Math.Abs(degreeSeconds) / 60.0f);
    }

    public static void GetDegreesMinutesSeconds(float degreeDecimals, float minuteDecimals, out float degrees, out float minutes, out float seconds)
    {
        degrees = degreeDecimals;
        minutes = Math.Abs((float)Math.Truncate((double)minuteDecimals));
        seconds = (Math.Abs(minuteDecimals) - minutes) * 60.0f;
    }

    public static void GetDegreesMinutesSeconds(float degreeDecimals, out float degrees, out float minutes, out float seconds)
    {
        degrees = (float)Math.Truncate((double)degreeDecimals);

        float totalMinutes = (Math.Abs(degreeDecimals) - Math.Abs(degrees)) * 60.0f;

        minutes = (float)Math.Truncate((double)totalMinutes);
        seconds = (totalMinutes - minutes) * 60.0f;
    }

    public void GetLatitudeDegreesMinutes(out float degrees, out float minutes)
    {
        GetDegreesMinutes(LatitudeDegrees, LatitudeMinutes, LatitudeSeconds, out degrees, out minutes);
    }

    public void GetLongitudeDegreesMinutes(out float degrees, out float minutes)
    {
        GetDegreesMinutes(LongitudeDegrees, LongitudeMinutes, LongitudeSeconds, out degrees, out minutes);
    }

    public void GetLatitudeDegreesMinutesSeconds(out float degrees, out float minutes, out float seconds)
    {
        degrees = LatitudeDegrees;
        minutes = LatitudeMinutes;
        seconds = LatitudeSeconds;
    }

    public void GetLongitudeDegreesMinutesSeconds(out float degrees, out float minutes, out float seconds)
    {
        degrees = LongitudeDegrees;
        minutes = LongitudeMinutes;
        seconds = LongitudeSeconds;
    }

    public void GetLatitudeDegrees(out float degrees)
    {
        degrees = GetDegrees(LatitudeDegrees, LatitudeMinutes, LatitudeSeconds);
    }

    public float GetLatitudeDegrees()
    {
        return GetDegrees(LatitudeDegrees, LatitudeMinutes, LatitudeSeconds);
    }

    public void GetLongitudeDegrees(out float degrees)
    {
        degrees = GetDegrees(LongitudeDegrees, LongitudeMinutes, LongitudeSeconds);
    }

    public float GetLongitudeDegrees()
    {
        return GetDegrees(LongitudeDegrees, LongitudeMinutes, LongitudeSeconds);
    }

    public static GpsCoordinate Parse(string latitude, string latitudeRef, string longitude, string longitudeRef)
    {
        // parse values that look like:  42 deg 16' 13.80"
        GpsLatitudeReference latRef = ParseLatitudeRef(latitudeRef);
        GpsLongitudeReference lngRef = ParseLongitudeRef(longitudeRef);
        ParseGpsCoordinate(latitude, out float latDegrees, out float latMinutes, out float latSeconds);
        ParseGpsCoordinate(longitude, out float lngDegrees, out float lngMinutes, out float lngSeconds);

        if (latRef == GpsLatitudeReference.South)
        {
            latDegrees *= -1.0f;
        }
        if (lngRef == GpsLongitudeReference.West)
        {
            lngDegrees *= -1.0f;
        }

        return new GpsCoordinate(latDegrees, latMinutes, latSeconds, lngDegrees, lngMinutes, lngSeconds);
    }

    public static GpsLatitudeReference ParseLatitudeRef(string latitudeRef)
    {
        GpsLatitudeReference latRef;

        if (string.Equals(latitudeRef, "north", StringComparison.OrdinalIgnoreCase))
        {
            latRef = GpsLatitudeReference.North;
        }
        else
        {
            latRef = GpsLatitudeReference.South;
        }

        return latRef;
    }

    public static GpsLongitudeReference ParseLongitudeRef(string longitudeRef)
    {
        GpsLongitudeReference lngRef;

        if (string.Equals(longitudeRef, "east", StringComparison.OrdinalIgnoreCase))
        {
            lngRef = GpsLongitudeReference.East;
        }
        else
        {
            lngRef = GpsLongitudeReference.West;
        }

        return lngRef;
    }

    public static void ParseGpsCoordinate(string coord, out float degrees, out float minutes, out float seconds)
    {
        ArgumentNullException.ThrowIfNull(coord);

        string[] splitTerms = new string[] { " ", "'", "\"", "deg", "N", "S", "E", "W" };

        string[] parts = coord.Split(splitTerms, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
        {
            throw new Exception("Expected to find deg, min, sec for the gps coord!");
        }

        degrees = float.Parse(parts[0], CultureInfo.InvariantCulture);
        minutes = float.Parse(parts[1], CultureInfo.InvariantCulture);
        seconds = float.Parse(parts[2], CultureInfo.InvariantCulture);
    }
}
#pragma warning restore CA1721
