namespace MawWww.Captcha;

public interface ICaptchaFeature
{
    Task<ICaptchaService> GetServiceAsync();
}
