﻿namespace MawWww.Captcha;

public interface ICaptchaService
{
    string SiteKey { get; }
    string ResponseFormFieldName { get; }
    Task<bool> VerifyAsync(string recaptchaResponse);
}
