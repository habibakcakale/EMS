namespace EMS.FrontEnd.ValidationRules {
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class BindingGroupValidationRule : ValidationRule {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            var gr = (BindingGroup) value;
            var isValid = gr != null && gr.HasValidationError;
            return isValid ? ValidationResult.ValidResult : new ValidationResult(false, string.Empty);
        }
    }
}