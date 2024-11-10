namespace MawWww.Models;

public record AlertProps
(
    MawFormPageModel MawFormPageModel,
    string? SuccessMessage = null,
    string? ErrorMessage = null
) {
    public Alert? Alert {
        get {
            if(
                !MawFormPageModel.SubmitAttempted ||
                (MawFormPageModel.SubmitSuccess && string.IsNullOrEmpty(SuccessMessage)) ||
                (!MawFormPageModel.SubmitSuccess && string.IsNullOrEmpty(ErrorMessage))
            ) {
                return null;
            }

            return MawFormPageModel.SubmitSuccess ?
                new Alert(AlertStatus.Success, SuccessMessage!) :
                new Alert(AlertStatus.Error, ErrorMessage!);
        }
    }
}
