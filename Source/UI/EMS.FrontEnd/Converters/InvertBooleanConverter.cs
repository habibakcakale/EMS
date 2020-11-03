namespace EMS.FrontEnd.Converters {
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBooleanConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var original = (bool) value;
            return !original;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var original = (bool) value;
            return !original;
        }
    }

    public class MergeBooleanConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.LongLength <= 0) return false;
            return values.Cast<bool>().All(item => !item);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}