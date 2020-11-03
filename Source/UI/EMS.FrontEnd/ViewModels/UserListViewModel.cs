namespace EMS.FrontEnd.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using System.Windows.Input;
    using Commands;
    using Common;
    using Common.User;
    using Integration.User;
    using MediatR;

    public class UserListViewModel : ViewModelBase {
        private readonly IMediator mediator;
        private Pagination pageInfo;
        private string search;
        private int? currentPage;
        private ObservableCollection<User> users;

        public ObservableCollection<User> Users {
            get => users;
            set {
                if (Equals(value, users)) return;
                users = value;
                OnPropertyChanged();
            }
        }

        public Pagination PageInfo {
            get => pageInfo;
            set {
                if (Equals(value, pageInfo)) return;
                pageInfo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Pages));
            }
        }

        public string Search {
            get => search;
            set {
                if (value == search) return;
                search = value;
                OnPropertyChanged();
            }
        }

        public int? CurrentPage {
            get => currentPage;
            set {
                if (value == currentPage) return;
                currentPage = value;
                OnPropertyChanged();
            }
        }

        public User User { get; set; }
        public IEnumerable<int> Pages => Enumerable.Range(1, PageInfo?.Pages ?? 1);

        public ICommand LoadUserCommand { get; set; }
        public ICommand SaveUserCommand { get; set; }

        public UserListViewModel(IMediator mediator) {
            this.mediator = mediator;
            this.LoadUserCommand = new RelayCommand<object>(LoadUsers);
            this.SaveUserCommand = new RelayCommand<User>(SaveUser);

        }

        private async Task SaveUser(User user) {
            if (user.Id > 0)
                await mediator.Send(new UpdateUser.Request() {
                    Id = user.Id,
                    Gender = user.Gender,
                    Name = user.Name,
                    Status = user.Status,
                    Email = user.Email
                });
            await mediator.Send(new CreateUser.Request() {
                Email = user.Email,
                Gender = user.Gender,
                Name = user.Name,
                Status = user.Status
            });
        }

        private async Task LoadUsers(object request) {
            var userList = await mediator.Send(new GetUserList.Request() {
                Name = this.Search,
                Page = this.CurrentPage ?? 0
            });
            this.Users = new ObservableCollection<User>(userList.Items);
            this.PageInfo = userList.Pagination;
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