using System;
using System.Collections.Generic;
using System.Linq;
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
using Kait.ViewModel;
using MahApps.Metro.Controls;

namespace Kait.View.Pages
{
    /// <summary>
    /// Interaction logic for NewInvoice.xaml
    /// </summary>
    public partial class NewInvoice : Page
    {
        public NewInvoice()
        {
            InitializeComponent();
            dateSlip.Content = DateTime.Now.ToLongDateString();
            DataContext = new NewInvoiceViewModel();
        }

        //
        //Control members
        //

        private void NavigateToTab(TabType tabType)
        {
            InvoiceNavTab.SelectedIndex = (int)tabType;
        }
        private enum TabType
        {
            ProductDetails,//0
            ClientDetails,//1
            PrintView//2
        }

        //
        //Interface(UI) members
        //

        //Backbutton at the top
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.PART_FrameService.CanGoBack)
            {
                MainWindow.PART_FrameService.GoBack();
            }

            
        }

        //Next button at product view
        private void NextBtnProductsTab_Click(object sender, RoutedEventArgs e)
        {
            NavigateToTab(TabType.ClientDetails);
        }

        private void DGridInvoiceProducts_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Console.WriteLine(e.Row.Item.GetType());
        }
    }
}
