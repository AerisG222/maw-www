// don't follow conventions for google responses
#pragma warning disable IDE1006
#pragma warning disable CA1707

using System.Text.Json.Serialization;

namespace MawWww.Captcha;

public class CloudflareTurnstileResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("error-codes")]
    public string[]? error_codes { get; set; }

    [JsonPropertyName("challenge_ts")]
    public DateTime ChallengeTimestamp { get; set; }

    [JsonPropertyName("hostname")]
    public string? Hostname { get; set; }
}
