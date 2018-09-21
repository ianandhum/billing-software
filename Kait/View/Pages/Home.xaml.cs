using Kait.ViewModel;
using Kait.ViewModel.Primitive;
using Provider.Data;
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

namespace Kait.View.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }

        
        private void NewInvoiceTrigger(object sender, EventArgs e)
        {
            MainWindow.PageHostService.Navigate(new NewInvoice());
        }

        private void NewPurchaseTrigger(object sender, EventArgs e)
        {
            MainWindow.PageHostService.Navigate(new NewPurchase());
        }

        private void ShowProductsView(object sender, EventArgs e)
        {
            MainWindow.PageHostService.Navigate(new ProductsView());  
        }

        private void ShowInvoicesView(object sender, EventArgs e)
        {
            MainWindow.PageHostService.Navigate(new InvoicesView());
        }

        private void ShowPurchasesView(object sender, EventArgs e)
        {
            MainWindow.PageHostService.Navigate(new PurchasesView());
        }

        private void ShowClientsView(object sender, EventArgs e)
        {
            MainWindow.PageHostService.Navigate(new ClientsView());
        }

        private void ShowVendorsView(object sender, EventArgs e)
        {
            MainWindow.PageHostService.Navigate(new VendorsView());
        }

    }

}
