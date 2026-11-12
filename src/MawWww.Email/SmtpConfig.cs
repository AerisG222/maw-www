namespace MawWww.Email;

public sealed class SmtpEmailConfig
{
    public string Server { get; set; } = null!;
    public int Port { get; set; }
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
}
