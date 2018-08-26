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

            if (Products.Count() > 0)
            {
                SelectedProduct = Products.FirstOrDefault();
                MeasureTypes = Enum.GetValues(typeof(Measure)).Cast<Measure>();

            }
            IsProductListEmpty = true;
        }
        void InitializeInvoiceProducts()
        {
            AddedInvoiceProducts = new ObservableCollection<AddedInvoiceProductsVM>();
            
        }
        void SyncAddProductFields(Product product)
        {
            if (product != null)
            {
                SelectedMeasureType = product.MU;
                Quantity = 1.0;
                Discount = 0.0;
            }
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
        

        private ObservableCollection<AddedInvoiceProductsVM> _InvoiceProductsVM;


        public ObservableCollection<AddedInvoiceProductsVM> AddedInvoiceProducts
        {
            get
            {
                return _InvoiceProductsVM;
            }
            set
            {
                _InvoiceProductsVM = value;
                RaisePropertyChanged("AddedInvoiceProducts");
            }
        }
        private AddedInvoiceProductsVM _SelectedProductItem { get; set; }
        public AddedInvoiceProductsVM SelectedProductItem
        {
            get
            {
                return _SelectedProductItem;
            }
            set
            {

                _SelectedProductItem = value;
                RaisePropertyChanged("SelectedProductItem");
            }
        }
        private bool _IsProductListEmpty;
        public bool IsProductListEmpty
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
        public IEnumerable<Measure> MeasureTypes { get; set; }
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
                if (value >= 0.0)
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
                if (0.0 <= value && value <= 100.0)
                {

                    discount = value;
                    RaisePropertyChanged("Discount");
                }

            }
        }
        private AddedInvoiceProductsVM CloneProductToInvoiceProduct(Product product) {
            AddedInvoiceProductsVM invoiceProduct = null;
            if (product != null) {
                invoiceProduct = new AddedInvoiceProductsVM()
                {
                     InvoiceProducts=new InvoiceProducts(){
                        CESSPercent = (float)product.CESSPercent,
                        Description = product.Description,
                        HSN = product.HSN,
                        Name = product.Name,
                        Price = (float)product.Price,
                    }
                };
            }
            else
            {
                invoiceProduct = new AddedInvoiceProductsVM();
            }
            return invoiceProduct;


        }
        private void AddProductItem(object parameter) {

            AddedInvoiceProductsVM ip = CloneProductToInvoiceProduct(SelectedProduct);
            ip.InvoiceProducts.DiscountPercent = (float)discount;
            ip.InvoiceProducts.Quantity = (float)quantity;
            ip.InvoiceProducts.MU = measure_type.ToString();
            ip.InvoiceProducts.Total =(float) Decimal.Round((Decimal)(ip.InvoiceProducts.Quantity * ip.InvoiceProducts.Price),2);
            
            AddedInvoiceProducts.Add(ip);
            SelectedProduct = null;
        }
        private bool IsProductEmpty() {

            if(SelectedProduct!=null)
                 return true;
            return false;
        }
        private ICommand _AddItemCmd;
        public ICommand AddItemCmd
        {
            get
            {
                if (_AddItemCmd == null)
                    _AddItemCmd = new RunCommand(AddProductItem, IsProductEmpty);
                return _AddItemCmd;
            }
            set
            {
                _AddItemCmd = value;
            }
        }

        /*
         * TODO || NOTWorking
         *  Mechanism to add commands in the product collection  
        */
        private ICommand _RmProductCmd;
        public ICommand RmProductCmd
        {
            get
            {
                if (_RmProductCmd == null)
                    _RmProductCmd = new RunCommand(RemoveProductItem);
                return _RmProductCmd;
            }
            set
            {
                _RmProductCmd = value;
            }

        }
        private ICommand _SourceUpdatedCmd;
        public ICommand SourceUpdatedCmd
        {
            get
            {
                if (_SourceUpdatedCmd == null)
                    _SourceUpdatedCmd = new RunCommand(InvoiceProductRowAdded);
                return _SourceUpdatedCmd;
            }
            set
            {
                _SourceUpdatedCmd = value;
            }

        }

        private void InvoiceProductUpdated() {
            if (AddedInvoiceProducts.Count > 0)
            {
                IsProductListEmpty = false;
            }
            else
            {
                IsProductListEmpty = true;
            }
            int index = 1;
            foreach (var InvoiceItem in AddedInvoiceProducts)
            {
                /*
                 * Re index slno after list reorder or update
                 */
                AddedInvoiceProducts.ElementAt(index- 1).SlNo = index;
                index++;
            }
        }

        private void InvoiceProductRowAdded(object obj)
        {
            if (AddedInvoiceProducts.Count != 0)
                AddedInvoiceProducts.ElementAt(AddedInvoiceProducts.Count - 1).SlNo=AddedInvoiceProducts.Count;
            InvoiceProductUpdated();
        }

        private void RemoveProductItem(object Item)
        {
            try
            {
                AddedInvoiceProducts.Remove((AddedInvoiceProductsVM)Item);
                InvoiceProductUpdated();
            }
            catch(InvalidCastException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }


        /*
* Test function is just used for debugging
*/
        private void CmdTest(object  e) {
            Console.WriteLine(e.ToString());

        }
    }
   
       
    
}
