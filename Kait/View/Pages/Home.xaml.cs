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

        private void NaigateToInvoiceAll(object sender, EventArgs e)
        {
            //testing invoice preview
            MainWindow.PageHostService.Navigate(
                new NewInvoice(
                    new InvoiceViewModel(
                        App.DataProvider.Invoices.Where(
                            x=>x.InvoiceId==140
                        ).FirstOrDefault()
                    )
                 )
            );
        }
        

        private void NaigateToPurchaseOrder(object sender, EventArgs e)
        {
            //testing invoice preview
            MainWindow.PageHostService.Navigate(
                new NewPurchase(
                    new PurchaseViewModel(
                        App.DataProvider.Purchases.Where(
                            x => x.PurchaseId == 24
                        ).FirstOrDefault()
                    )
                 )
            );
        }
    }

}
