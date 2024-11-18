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

        StringBuilder builder = new(byteArray.Length);

        foreach (byte b in byteArray)
        {
            builder.Append(FormattableString.Invariant($"{b:X}"));
        }

        return builder.ToString();
    }
}
