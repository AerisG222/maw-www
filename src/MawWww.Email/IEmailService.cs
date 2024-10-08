﻿namespace MawWww.Email;

public interface IEmailService
{
    string FromAddress { get; }
    Task SendAsync(string recipient, string from, string subject, string body);
    Task SendHtmlAsync(string recipient, string from, string subject, string body);
}
