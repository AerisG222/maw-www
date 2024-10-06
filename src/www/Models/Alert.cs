namespace www.Models;

public record Alert
(
    AlertStatus Status,
    string Message
) {
    public string ColorClass => Status switch {
        AlertStatus.Success => "alert-success",
        AlertStatus.Info => "alert-info",
        AlertStatus.Warning => "alert-warning",
        AlertStatus.Error => "alert-error",
        _ => ""
    };

    public string IconClass => Status switch {
        AlertStatus.Success => "i-mdi-success-circle-outline",
        AlertStatus.Info => "i-mdi-information-outline",
        AlertStatus.Warning => "i-mdi-warning-circle-outline",
        AlertStatus.Error => "i-mdi-error-outline",
        _ => ""
    };
}
