﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EMS.FrontEnd.Controls {
    using System.Linq;
    using Common.User;
    using MaterialDesignThemes.Wpf;
    using ViewModels;

    /// <summary>
    /// Interaction logic for NewUserControl.xaml
    /// </summary>
    public partial class NewUserControl : UserControl {
        public NewUserControl() {
            InitializeComponent();
            this.DataContext = new NewUserControlViewModel(); // TODO: Inject 
        }

    }
}
