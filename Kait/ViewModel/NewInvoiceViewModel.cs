using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Support;
using Provider.Data;
using System.Data.Entity;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Kait.Support;

namespace Kait.ViewModel
{
    public class NewInvoiceViewModel : NotifyUIBase
    {

        public NewInvoiceViewModel()
        {
            NewInvoice = new Invoice();
            InitializeAddProductSection();
            InitializeInvoiceProducts();
        }

        void InitializeAddProductSection()
        {
            Products = (from product in App.DataProvider.Products select product).ToList();

            if (Products.Count()>0)
            {
                SelectedProduct = Products.FirstOrDefault();
                MeasureTypes = Enum.GetValues(typeof(Measure)).Cast<Measure>();

            }
        }
        void InitializeInvoiceProducts()
        {
            
            AddedInvoiceProducts = new ObservableCollection<InvoiceProducts>();
        }
        void SyncAddProductFields(Product product)
        {
            SelectedMeasureType = product.MU;
            Quantity = 1.0;
            Discount = 0.0;
        }

        private Invoice _newinvoice;
        public Invoice NewInvoice
        {
            get
            {
                return _newinvoice;
            }
            set
            {
                _newinvoice = value;
                RaisePropertyChanged("NewInvoice");
            }

        }
        
        public IEnumerable<Product> Products { get; set; }
        
        private ICollection<InvoiceProducts> invoiceProducts;

        public ICollection<InvoiceProducts> AddedInvoiceProducts
        {
            get
            {
                return invoiceProducts;
            }
            set
            {
                invoiceProducts = value;
                
               
                RaisePropertyChanged("AddedInvoiceProducts");
            }
        }
        private Visibility _IsProductListEmpty;
        public  Visibility IsProductListEmpty
        {
            get
            {
                return _IsProductListEmpty;
            }
            set
            {
                _IsProductListEmpty = value;
                RaisePropertyChanged("IsProductListEmpty");
            }
        }
        private Product product_selected;

        public Product SelectedProduct {
            get {
                return product_selected;
                }
            set {
                
                product_selected = value;
                SyncAddProductFields(value);
                RaisePropertyChanged("SelectedProduct");
            }
        }
        public IEnumerable<Measure> MeasureTypes { get; set;}
        private Measure measure_type;
        public Measure SelectedMeasureType {
            get {
                return measure_type;
            }
            set {
                measure_type = value;
                RaisePropertyChanged("SelectedMeasureType");
            }
        }
        private double quantity;
        public double Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                if (value>=0.0)
                {

                    quantity = value;
                    RaisePropertyChanged("Quantity");
                }

            }
        }
        private double discount;
        public double Discount
        {
            get
            {
                return discount;
            }
            set
            {
                if(0.0<=value && value <= 100.0)
                {

                    discount = value;
                    RaisePropertyChanged("Discount");
                }

            }
        }
        private InvoiceProducts CloneProductToInvoiceProduct(Product product) {
            InvoiceProducts invoiceProduct=null;
            if (product !=null) {
                invoiceProduct = new InvoiceProducts()
                {
                    CESSPercent = (float)product.CESSPercent,
                    Description = product.Description,
                    HSN = product.HSN,
                    Name = product.Name,
                    Price = (float)product.Price,
                };
            }
            else
            {
                invoiceProduct = new InvoiceProducts();
            }
            return invoiceProduct;
            

        }
        private void AddProductItem() {
            
            InvoiceProducts ip = CloneProductToInvoiceProduct(SelectedProduct);
            ip.DiscountPercent = (float)discount;
            ip.Quantity = (float)quantity;
            ip.MU = measure_type.ToString();
            
            AddedInvoiceProducts.Add(ip);
            if (invoiceProducts.Count > 0)
            {
                IsProductListEmpty = Visibility.Collapsed;
            }
            else
            {
                IsProductListEmpty = Visibility.Visible;
            }
        }
        private ICommand _AddItemCmd;
        public ICommand AddItemCmd
        {
            get
            {
                if (_AddItemCmd == null)
                    _AddItemCmd = new RunCommand(AddProductItem);
                return _AddItemCmd;
            }
            set
            {
                _AddItemCmd = value;
            }
        }
       
    }
   
       
    
}
