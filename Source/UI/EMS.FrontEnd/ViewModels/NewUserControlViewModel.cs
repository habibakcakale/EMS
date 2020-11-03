namespace EMS.FrontEnd.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.User;

    public class NewUserControlViewModel : ViewModelBase {
        private string name;
        private string email;
        private Gender gender;
        private UserStatus status;

        public int Id { get; set; }

        public string Name {
            get => name;
            set {
                if (value == name) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public string Email {
            get => email;
            set {
                if (value == email) return;
                email = value;
                OnPropertyChanged();
            }
        }

        public Gender Gender {
            get => gender;
            set {
                if (value == gender) return;
                gender = value;
                OnPropertyChanged();
            }
        }

        public UserStatus Status {
            get => status;
            set {
                if (value == status) return;
                status = value;
                OnPropertyChanged();
            }
        }

        public bool DialogResult { get; set; }

        public static IEnumerable<Gender> Genders { get; set; } = Enum.GetValues(typeof(Gender)).Cast<Gender>();
        public static IEnumerable<UserStatus> Statuses { get; set; } = Enum.GetValues(typeof(UserStatus)).Cast<UserStatus>();
    }
}