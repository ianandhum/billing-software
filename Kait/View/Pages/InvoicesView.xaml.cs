﻿using Kait.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Kait.View.Pages
{
    /// <summary>
    /// Interaction logic for InvoicesView.xaml
    /// </summary>
    public partial class InvoicesView : Page
    {
        private InvoicesViewModel viewModel = new InvoicesViewModel();

        public InvoicesView()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SearchKey_Hit(object sender, KeyEventArgs e)
        {
            // Not Working
            if (e.Key == Key.Return)
                (viewModel).SearchCmd.Execute(null);
        }
    }
}