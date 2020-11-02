namespace EMS.FrontEnd.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using Annotations;
    using Common;
    using Microsoft.Extensions.DependencyInjection;
    using Views;

    public class MainWindowViewModel:INotifyPropertyChanged {
        private readonly IServiceProvider provider;
        private ObservableCollection<RouteItem> routes;
        private RouteItem selectedRoute;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<RouteItem> Routes {
            get => routes;
            set {
                if (Equals(value, routes)) return;
                routes = value;
                OnPropertyChanged();
            }
        }

        public RouteItem SelectedRoute {
            get => selectedRoute;
            set {
                if (Equals(value, selectedRoute)) return;
                selectedRoute = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RouteContent));
            }
        }

        public UserControl RouteContent {
            get {
                if (SelectedRoute != null)
                    return provider.GetService(SelectedRoute.Content) as UserControl;
                return null;
            }
        }

        public MainWindowViewModel(IServiceProvider provider) {
            this.provider = provider;
            this.Routes = new ObservableCollection<RouteItem>(new List<RouteItem>() {
                new RouteItem() {
                    Title = "Users",
                    Content = typeof(UserListView)
                }
            });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class RouteItem {
            public string Title { get; set; }
            public Type Content { get; set; }
        }
    }
}