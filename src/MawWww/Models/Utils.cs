using System.Text;

namespace MawWww.Models;

public static class Utils
{
    public static Stream ToStream(this string input)
    {
        var ms = new MemoryStream();

        using (var writer = new StreamWriter(ms, Encoding.UTF8, 4096, true))
        {
            writer.Write(input);
            writer.Flush();

            ms.Seek(0, SeekOrigin.Begin);
        }

        return ms;
    }

    public static string ToHexString(byte[] byteArray)
    {
        ArgumentNullException.ThrowIfNull(byteArray);

        StringBuilder builder = new(byteArray.Length * 2);

        foreach (byte b in byteArray)
        {
            builder.Append(FormattableString.Invariant($"{b:X2}"));
        }

        return builder.ToString();
    }
}
