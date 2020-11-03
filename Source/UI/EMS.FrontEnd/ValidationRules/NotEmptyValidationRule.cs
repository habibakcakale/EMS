namespace EMS.FrontEnd.ValidationRules {
    using System.Globalization;
    using System.Windows.Controls;

    public class NotEmptyValidationRule : ValidationRule {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            //TODO: We  only care string values in here for now.
            var isValid = !string.IsNullOrWhiteSpace(value?.ToString());
            //TODO: Message should be a culture specific messages from either resx or somewhere else.
            return isValid ? ValidationResult.ValidResult : new ValidationResult(false, "Field is Required.");
        }
    }
}