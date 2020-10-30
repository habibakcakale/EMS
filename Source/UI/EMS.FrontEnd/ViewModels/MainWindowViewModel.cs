using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.FrontEnd.ViewModels {
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Windows;
    using Common;

    public class MainWindowViewModel {

        public MainWindowViewModel(IHttpClientFactory clientFactory) {

        }
    }
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore() => new BindingProxy();

        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}