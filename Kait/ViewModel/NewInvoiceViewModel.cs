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
            PaymentModes = Enum.GetValues(typeof(PaymentType)).Cast<PaymentType>();

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
        public IEnumerable<PaymentType> PaymentModes { get; set; }
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
                if (value > 0.0)
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
                         //TODO Add all necessary init Data
                        ProductId=product.ProductId,
                        CESSPercent = product.CESSPercent,
                        Description = product.Description,
                        HSN = product.HSN,
                        Name = product.Name,
                        Price = product.Price,

                    }
                };
            }
            else
            {
                invoiceProduct = new AddedInvoiceProductsVM();
            }
            return invoiceProduct;


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

        private void AddProductItem(object parameter)
        {

            AddedInvoiceProductsVM ip = CloneProductToInvoiceProduct(SelectedProduct);
            ip.InvoiceProducts.DiscountPercent =(decimal) discount;
            ip.InvoiceProducts.Quantity = (decimal)quantity;
            ip.InvoiceProducts.MU = measure_type;
            // TODO : Allow App Settings for precision (currently it is 2,hardcoded)
            ip.InvoiceProducts.Total = Decimal.Round((decimal)(ip.InvoiceProducts.Quantity * ip.InvoiceProducts.Price), 2);
            AddedInvoiceProducts.Add(ip);
            SelectedProduct = null;
        }

        private ICommand _EditProductCmd;
        public ICommand EditProductCmd
        {
            get
            {
                if (_EditProductCmd == null)
                    _EditProductCmd = new RunCommand(EditProductItem);
                return _EditProductCmd;
            }
            set
            {
                _EditProductCmd = value;
            }

        }

        private void EditProductItem(object obj)
        {
            IsEditProductItemOpen = true;
        }

        private ICommand _InvoiceProductSourceUpdatedCmd;
        public ICommand InvoiceProductSourceUpdatedCmd
        {
            get
            {
                if (_InvoiceProductSourceUpdatedCmd == null)
                    _InvoiceProductSourceUpdatedCmd = new RunCommand(InvoiceProductRowAdded);
                return _InvoiceProductSourceUpdatedCmd;
            }
            set
            {
                _InvoiceProductSourceUpdatedCmd = value;
            }

        }

        private void InvoiceProductRowAdded(object obj)
        {
            if (AddedInvoiceProducts.Count != 0)
                AddedInvoiceProducts.ElementAt(AddedInvoiceProducts.Count - 1).SlNo=AddedInvoiceProducts.Count;
            InvoiceProductUpdated();
        }

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
        private void InvoiceProductUpdated()
        {
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
                AddedInvoiceProducts.ElementAt(index - 1).SlNo = index;
                index++;
            }
        }

        /*
         * ChildWindowVisibility Handles
         * 
         */
        private bool _IsEditProductItemOpen { get; set; }
        public bool IsEditProductItemOpen
        {
            get
            {
                return _IsEditProductItemOpen;
            }
            set
            {
                _IsEditProductItemOpen = value;
                RaisePropertyChanged("IsEditProductItemOpen");
            }
        }

        private bool _IsAddPaymentOpen { get; set; }
        public bool IsAddPaymentOpen
        {
            get
            {
                return _IsAddPaymentOpen;
            }
            set
            {
                _IsAddPaymentOpen = value;
                RaisePropertyChanged("IsAddPaymentOpen");
            }
        }

        private bool _IsAddShippingChargeOpen { get; set; }
        public bool IsAddShippingChargeOpen
        {
            get
            {
                return _IsAddShippingChargeOpen;
            }
            set
            {
                _IsAddShippingChargeOpen = value;
                RaisePropertyChanged("IsAddShippingChargeOpen");
            }
        }

        private bool _IsDiscountAllOpen { get; set; }
        public bool IsDiscountAllOpen
        {
            get
            {
                return _IsDiscountAllOpen;
            }
            set
            {
                _IsDiscountAllOpen = value;
                RaisePropertyChanged("IsDiscountAllOpen");
            }
        }

        private bool _DiscountAllTrigger { get; set; }
        public bool DiscountAllTrigger
        {
            get
            {
                return _DiscountAllTrigger;
            }
            set
            {
                _DiscountAllTrigger = value;
                if (value)
                {
                    IsDiscountAllOpen = true;
                }
                RaisePropertyChanged("DiscountAllTrigger");
            }
        }
        private bool _AddPaymentTrigger { get; set; }
        public bool AddPaymentTrigger
        {
            get
            {
                return _AddPaymentTrigger;
            }
            set
            {
                _AddPaymentTrigger = value;
                if (value)
                {
                    IsAddPaymentOpen = true;
                }
                RaisePropertyChanged("AddPaymentTrigger");
            }
        }
        private bool _ShippingChargeTrigger { get; set; }
        public bool ShippingChargeTrigger
        {
            get
            {
                return _ShippingChargeTrigger;
            }
            set
            {

                _ShippingChargeTrigger = value;
                if (value)
                {
                    IsAddShippingChargeOpen = true;
                }
                RaisePropertyChanged("ShippingChargeTrigger");
            }
        }

        // Commands For ChildWindows

        
        private ICommand _AddPaymentCmd;
        public ICommand AddPaymentCmd
        {
            get
            {
                if (_AddPaymentCmd == null)
                    _AddPaymentCmd = new RunCommand(AddPayment);
                return _AddPaymentCmd;
            }
            set
            {
                _AddPaymentCmd = value;
            }

        }

        private void AddPayment(object obj)
        {
            
            IsAddPaymentOpen=false;
            if (obj!=null && (Boolean)obj)
            {
                // execute ok button
                Console.WriteLine("Ok Btn Payment");
            }
        }
        private ICommand _DiscountAllCmd;
        public ICommand DiscountAllCmd
        {
            get
            {
                if (_DiscountAllCmd == null)
                    _DiscountAllCmd = new RunCommand(ApplyDiscountOnAll);
                return _DiscountAllCmd;
            }
            set
            {
                _DiscountAllCmd = value;
            }

        }

        private void ApplyDiscountOnAll(object obj)
        {
            IsDiscountAllOpen = false;
            if (obj!=null && (Boolean)obj)
            {
                //Execute ok button
                Console.WriteLine("Ok Btn Discount");
                
            }
        }
        private ICommand _ShippingChargeCmd;
        public ICommand ShippingChargeCmd
        {
            get
            {
                if (_ShippingChargeCmd == null)
                    _ShippingChargeCmd = new RunCommand(AddShippingCharge);
                return _ShippingChargeCmd;
            }
            set
            {
                _ShippingChargeCmd = value;
            }

        }


        private void AddShippingCharge(object obj)
        {
            
            IsAddShippingChargeOpen = false;
            if (obj != null && (Boolean)obj)
            {
                //Execute ok button
                Console.WriteLine("Ok Btn shipping");
            }
        }


        //save edit command
        private ICommand _SaveEditsProductCmd;
        public ICommand SaveEditsProductCmd
        {
            get
            {
                if (_SaveEditsProductCmd == null)
                    _SaveEditsProductCmd = new RunCommand(SaveEditsProductItem);
                return _SaveEditsProductCmd;
            }
            set
            {
                _SaveEditsProductCmd = value;
            }

        }

        private void SaveEditsProductItem(object obj)
        {
            IsEditProductItemOpen = false;
        }



        /*
        * Test function is just used for debugging
        */
        private void CmdTest(object  e) {
            Console.WriteLine(e.ToString());

        }
    }
   
       
    
}
