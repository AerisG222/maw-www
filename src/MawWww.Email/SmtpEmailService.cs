﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MawWww.Email;

public class SmtpEmailService
    : IEmailService
{
    readonly SmtpEmailConfig _config;
    readonly ILogger _log;

    public string FromAddress => _config.User;

    public SmtpEmailService(
        ILogger<SmtpEmailService> log,
        IOptions<SmtpEmailConfig> config
    ) {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(log);

        _config = config.Value;
        _log = log;
    }

    public virtual Task SendHtmlAsync(string recipient, string from, string subject, string body)
    {
        return SendAsync(recipient, from, subject, body, true);
    }

    public virtual Task SendAsync(string recipient, string from, string subject, string body)
    {
        return SendAsync(recipient, from, subject, body, false);
    }

    protected virtual async Task SendAsync(string recipient, string from, string subject, string body, bool html)
    {
        _log.LogInformation("sending email to: {Recipient}, from: {From}, subject: {Subject}", recipient, from, subject);

        using var smtp = new SmtpClient();
        var builder = new BodyBuilder();

        if (html)
        {
            builder.HtmlBody = body;
        }
        else
        {
            builder.TextBody = body;
        }

        using var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(null, from));
        msg.To.Add(new MailboxAddress(null, recipient));
        msg.Subject = subject;
        msg.Body = builder.ToMessageBody();

        // http://stackoverflow.com/questions/33496290/how-to-send-email-by-using-mailkit
        await smtp.ConnectAsync(_config.Server, _config.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_config.User, _config.Password);
        await smtp.SendAsync(msg);
        await smtp.DisconnectAsync(true);
    }
}
