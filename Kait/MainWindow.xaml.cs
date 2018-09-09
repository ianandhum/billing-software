using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Interactivity;
using System.Windows.Shapes;
using Kait.View.Pages;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;

namespace Kait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            PageHostService = PageHost.NavigationService;
            PageHostService.Navigate(new Home());
           
                
        }
        public static NavigationService PageHostService { get; set; }
        private void NavigationButton_Click(object sender, EventArgs e)
        {
            if (PageHostService.CanGoBack)
            {
                PageHostService.Navigate(new Home());
            }
            
        }
        /* 
         *  The following part of code is from stackoverflow.com
         *  Thanks to serge_gubenko and Wong Jia Hau
         *  //
         *  Modified to suit our needs 
         *  //  Animation now gives fade in and out effect(was slide in and out originally)
         *  
         */
        private void PART_Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var @double = new DoubleAnimation();
            @double.Duration = TimeSpan.FromSeconds(0.8);
            @double.DecelerationRatio = 0.5;
            @double.To = 1.0;
            
            @double.From = 0.0;
            (e.Content as Page).BeginAnimation(OpacityProperty, @double);
        }

        private void ExitApp(object sender, EventArgs e)
        {
            
        }
    }

}