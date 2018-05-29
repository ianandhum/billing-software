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
using System.Windows.Shapes;
using Kait.View.Pages;
using MahApps.Metro.Controls;

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
            PART_FrameService = PART_Frame.NavigationService;
            PART_FrameService.Navigate(new Home());
           
                
        }
        public static NavigationService PART_FrameService { get; set; }
       

        private void NavigationButton_Click(object sender, EventArgs e)
        {
            if (PART_FrameService.CanGoBack)
            {
                PART_FrameService.Navigate(new Home());
            }
            
        }
    }

}