namespace EMS.FrontEnd.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows.Data;
    using Common;

    public class UserListViewModel {
        public ObservableCollection<User> Users { get; set; }

        public UserListViewModel() {
            this.Users = new ObservableCollection<User>(new List<User>() {
                new User() {
                    Email = "habibakcakale@gmail.com",
                    Name = "Habib Akcakale",
                    CreatedAt = DateTime.Now,
                    Gender = "Male",
                    Id = 1,
                    Status = "Active",
                    UpdatedAt = DateTime.Now
                }
            });
        }
    }

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