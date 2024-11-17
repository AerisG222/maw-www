using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Dotnet;

public class PathsPageModel
    : MawPageModel
{
    // https://www.youtube.com/watch?v=cwZb2mqId0A
    public static readonly IEnumerable<(string apiCall, string value)> Examples = [
        ( @"Path.ChangeExtension(""C:\windows\test.txt"", "".dat"")", Path.ChangeExtension(@"C:\windows\test.txt", ".dat").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.ChangeExtension(""C:\windows\test.txt"", ""dat"")",  Path.ChangeExtension(@"C:\windows\test.txt", "dat").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.ChangeExtension(""/etc/passwd"", "".txt"")",         Path.ChangeExtension("/etc/passwd", ".txt").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.ChangeExtension(""/etc/passwd"", ""txt"")",          Path.ChangeExtension("/etc/passwd", "txt").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.ChangeExtension(""/etc/passwd/"", ""txt"")",         Path.ChangeExtension("/etc/passwd/", "txt").ToString(CultureInfo.InvariantCulture) ),

        ( @"Path.Combine(""C:\test\"", ""file.txt"")",   Path.Combine(@"C:\test\", "file.txt") ),
        ( @"Path.Combine(""C:\test"", ""file.txt"")",    Path.Combine(@"C:\test", "file.txt") ),
        ( @"Path.Combine(""C:\test"", ""C:\file.txt"")", Path.Combine(@"C:\test", @"C:\file.txt") ),
        ( @"Path.Combine(""/etc"", ""passwd"")",         Path.Combine("/etc", "passwd") ),
        ( @"Path.Combine(""etc"", ""passwd"")",          Path.Combine("etc", "passwd") ),
        ( @"Path.Combine(""/etc"", ""/usr"")",           Path.Combine("/etc", "/usr") ),

        ( @"Path.GetDirectoryName(""/etc/passwd"")",          Path.GetDirectoryName("/etc/passwd")! ),
        ( @"Path.GetDirectoryName(""/etc/passwd/"")",         Path.GetDirectoryName("/etc/passwd/")! ),
        ( @"Path.GetDirectoryName(""C:\windows\system32"")",  Path.GetDirectoryName(@"C:\windows\system32")! ),
        ( @"Path.GetDirectoryName(""C:\windows\system32\"")", Path.GetDirectoryName(@"C:\windows\system32\")! ),

        ( @"Path.GetExtension(""/etc/passwd"")",         Path.GetExtension("/etc/passwd") ),
        ( @"Path.GetExtension(""/etc/passwd/"")",        Path.GetExtension("/etc/passwd/") ),
        ( @"Path.GetExtension(""/etc/passwd/.txt"")",    Path.GetExtension("/etc/passwd/.txt") ),
        ( @"Path.GetExtension(""C:\windows\test.txt"")", Path.GetExtension(@"C:\windows\test.txt") ),

        ( @"Path.GetFileName(""/etc/passwd"")",         Path.GetFileName("/etc/passwd") ),
        ( @"Path.GetFileName(""/etc/passwd/"")",        Path.GetFileName("/etc/passwd/") ),
        ( @"Path.GetFileName(""/etc/passwd/.txt"")",    Path.GetFileName("/etc/passwd/.txt") ),
        ( @"Path.GetFileName(""C:\windows\test.txt"")", Path.GetFileName(@"C:\windows\test.txt") ),

        ( @"Path.GetFileNameWithoutExtension(""/etc/passwd"")",             Path.GetFileNameWithoutExtension("/etc/passwd") ),
        ( @"Path.GetFileNameWithoutExtension(""/etc/passwd/"")",            Path.GetFileNameWithoutExtension("/etc/passwd/") ),
        ( @"Path.GetFileNameWithoutExtension(""/etc/passwd/.txt"")",        Path.GetFileNameWithoutExtension("/etc/passwd/.txt") ),
        ( @"Path.GetFileNameWithoutExtension(""C:\windows\test.txt"")",     Path.GetFileNameWithoutExtension(@"C:\windows\test.txt") ),
        ( @"Path.GetFileNameWithoutExtension(""C:\windows\test.txt.pgp"")", Path.GetFileNameWithoutExtension(@"C:\windows\test.txt.pgp") ),

        ( @"Path.GetFullPath(""/etc/passwd"")",         Path.GetFullPath("/etc/passwd") ),
        ( @"Path.GetFullPath(""/etc/passwd/"")",        Path.GetFullPath("/etc/passwd/") ),
        ( @"Path.GetFullPath(""/etc/passwd/.txt"")",    Path.GetFullPath("/etc/passwd/.txt") ),
        ( @"Path.GetFullPath(""etc/passwd"")",          Path.GetFullPath("etc/passwd") ),
        ( @"Path.GetFullPath(""C:\windows\test.txt"")", Path.GetFullPath(@"C:\windows\test.txt") ),

        ( @"Path.GetInvalidFileNameChars()", string.Join(",", CharArrayToStringArray(Path.GetInvalidFileNameChars())) ),

        ( @"Path.GetInvalidPathChars()", string.Join(",", CharArrayToStringArray(Path.GetInvalidPathChars())) ),

        ( @"Path.GetPathRoot(""/etc/passwd"")",         Path.GetPathRoot("/etc/passwd")! ),
        ( @"Path.GetPathRoot(""/etc/passwd/"")",        Path.GetPathRoot("/etc/passwd/")! ),
        ( @"Path.GetPathRoot(""/etc/passwd/.txt"")",    Path.GetPathRoot("/etc/passwd/.txt")! ),
        ( @"Path.GetPathRoot(""etc/passwd"")",          Path.GetPathRoot("etc/passwd")! ),
        ( @"Path.GetPathRoot(""C:\windows\test.txt"")", Path.GetPathRoot(@"C:\windows\test.txt")! ),

        ( @"Path.GetRandomFileName()", Path.GetRandomFileName() ),

        ( @"Path.GetTempFileName()", Path.GetTempFileName() ),

        ( @"Path.GetTempPath()", Path.GetTempPath() ),

        ( @"Path.HasExtension(""/etc/passwd"")",         Path.HasExtension("/etc/passwd").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.HasExtension(""/etc/passwd/"")",        Path.HasExtension("/etc/passwd/").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.HasExtension(""/etc/passwd/.txt"")",    Path.HasExtension("/etc/passwd/.txt").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.HasExtension(""etc/passwd"")",          Path.HasExtension("etc/passwd").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.HasExtension(""C:\windows\test.txt"")", Path.HasExtension(@"C:\windows\test.txt").ToString(CultureInfo.InvariantCulture) ),

        ( @"Path.IsPathRooted(""/etc/passwd"")",          Path.IsPathRooted("/etc/passwd").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.IsPathRooted(""/etc/skel/"")",           Path.IsPathRooted("/etc/skel/").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.IsPathRooted(""C:\windows\system32"")",  Path.IsPathRooted(@"C:\windows\system32").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.IsPathRooted(""C:\windows\system32\"")", Path.IsPathRooted(@"C:\windows\system32\").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.IsPathRooted(""etc/passwd"")",           Path.IsPathRooted("etc/passwd").ToString(CultureInfo.InvariantCulture) ),
        ( @"Path.IsPathRooted(""windows\system32\"")",    Path.IsPathRooted(@"windows\system32\").ToString(CultureInfo.InvariantCulture) ),
    ];

    public IActionResult OnGet()
    {
        return Page();
    }

    static string[] CharArrayToStringArray(char[] array)
    {
        string[] result = new string[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            result[i] = array[i].ToString(CultureInfo.InvariantCulture);
        }

        return result;
    }
}
