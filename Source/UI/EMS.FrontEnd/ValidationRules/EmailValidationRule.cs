namespace EMS.FrontEnd.ValidationRules {
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;

    public class EmailValidationRule : ValidationRule {
        private static readonly Regex Regex = new Regex("^\\S+@\\S+$", RegexOptions.Compiled);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            var isValid = Regex.IsMatch(value?.ToString() ?? string.Empty);
            return isValid ? ValidationResult.ValidResult : new ValidationResult(false, "Please provide valid email address");
        }
    }
}