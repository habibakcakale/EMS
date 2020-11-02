namespace EMS.FrontEnd.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.ObjectModel;
    using Common;

    /// <summary>
    /// Interaction logic for UserListView.xaml
    /// </summary>
    public partial class UserList : UserControl
    {
        public static readonly DependencyProperty UsersProperty = DependencyProperty.Register(
            nameof(Users), typeof(ObservableCollection<User>), typeof(UserList), new PropertyMetadata(default(ObservableCollection<User>)));

        public ObservableCollection<User> Users {
            get => (ObservableCollection<User>) GetValue(UsersProperty);
            set => SetValue(UsersProperty, value);
        }
        public UserList()
        {
            InitializeComponent();
        }
    }
}
