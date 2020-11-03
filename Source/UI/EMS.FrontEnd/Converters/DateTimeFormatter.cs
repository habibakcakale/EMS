namespace EMS.FrontEnd.Converters {
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeFormatter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is DateTime date) {
                return date.ToShortDateString();
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return System.Convert.ChangeType(value, targetType, culture);
        }
    }
}