namespace EMS.FrontEnd.ViewModels {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Commands;
    using Common;
    using Common.User;
    using Integration.User;
    using MaterialDesignThemes.Wpf;
    using MediatR;
    using Services;

    public class UserListViewModel : ViewModelBase {
        private readonly IMediator mediator;
        private readonly ICsvExportService exportService;
        private Pagination pageInfo;
        private string search;
        private int? currentPage;
        private ObservableCollection<User> users;
        private User selectedUser;

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

        public User SelectedUser {
            get => selectedUser;
            set {
                if (Equals(value, selectedUser)) return;
                selectedUser = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<int> Pages => Enumerable.Range(1, PageInfo?.Pages ?? 1);
        public ICommand LoadUserCommand { get; set; }
        public ICommand SaveUserCommand { get; set; }
        public ICommand DeleteUsersCommand { get; set; }
        public ICommand DownloadUsersCommand { get; set; }

        private Task DownloadUsers(object args) {
            return exportService.ExportAsync("./users.csv", this.Users);
        }

        public UserListViewModel(IMediator mediator, ICsvExportService exportService) {
            this.mediator = mediator;
            this.exportService = exportService;
            this.LoadUserCommand = new RelayCommand<object>(LoadUsers);
            this.SaveUserCommand = new RelayCommand<DialogClosingEventArgs>(SaveUser);
            this.DeleteUsersCommand = new RelayCommand<User>(DeleteUsers, user => this.SelectedUser != null);
            this.DownloadUsersCommand = new RelayCommand<object>(DownloadUsers, args => this.Users?.Count > 0);
        }

        private async Task DeleteUsers(User user) {
            await mediator.Send(new DeleteUser.Request {Id = user.Id});
            await LoadUsers(null);
        }

        private async Task SaveUser(DialogClosingEventArgs args) {
            var user = args?.Parameter as NewUserControlViewModel;
            if (user == null) return;
            if (user.Id > 0)
                await mediator.Send(new UpdateUser.Request {
                    Id = user.Id,
                    Gender = user.Gender,
                    Name = user.Name,
                    Status = user.Status,
                    Email = user.Email
                });
            await mediator.Send(new CreateUser.Request {
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
}