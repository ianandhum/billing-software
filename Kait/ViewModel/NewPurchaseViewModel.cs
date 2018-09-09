using System;
using System.Collections.Generic;
using System.Linq;
using Provider.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Kait.Support;
using Kait.BusinessLogic;
using Kait.ViewModel.Primitive;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace Kait.ViewModel
{
    public class NewPurchaseViewModel : NotifyUIBase
    {

        private IDialogCoordinator DialogCoordinator;

        public NewPurchaseViewModel(IDialogCoordinator iDialogCoordinator)
        {
            InitializeViewModel();
            DialogCoordinator = iDialogCoordinator;

        }
        private void InitializeViewModel()
        {
            CurrentDate = DateTime.Now;

            string NextPurchaseId="1";
            try
            {
               NextPurchaseId = (App.DataProvider.Purchases.Max((x) => x.PurchaseId) + 1).ToString();
                
            }
            catch (Exception e)
            {

                Console.WriteLine(e.StackTrace);
                MetaData = new MetaData(App.DataProvider)
                {

                    Purchase = new MetaData.PurchaseMeta()
                    {
                        NextPurchaseId = NextPurchaseId,
                        Prefix = App.GetConfig("PurchasePrefix"),
                        TypeOnPrint = App.GetConfig("FirstTypeOnPurchasePrint")
                    }
                };
                
            }
            

            NewPurchase = new PurchaseViewModel()
            {
                IssueDate = DateTime.Today
            };
            InitializeAddProductSection();
            InitializePurchaseProducts();
            InitializePayments();
            InitializeVendorSection();
        }
        void InitializeNavigation() {
            CurrentTab = 0;
        }

        void InitializePayments()
        {
            Payments = new PurchasePayments();
        }
        void InitializeAddProductSection()
        {
            Products = new ObservableCollection<Product>((from product in App.DataProvider.Products select product));

            if (Products.Count() > 0)
            {
                SelectedProduct = Products.FirstOrDefault();
                MeasureTypes = Enum.GetValues(typeof(Measure)).Cast<Measure>();
                
            }
            IsProductListEmpty = true;
        }
        void InitializePurchaseProducts()
        {
            AddedPurchaseProducts = new ObservableCollection<PurchaseProductsViewModel>();
            PaymentModes = Enum.GetValues(typeof(PaymentType)).Cast<PaymentType>();

        }
        void SyncAddProductFields(Product product)
        {
            if (product != null)
            {
                SelectedMeasureType = product.MU;
                Quantity = 1;
                Discount = 0;
            }
        }

        public MetaData MetaData { get; set; }


        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get
            {
                return _CurrentDate;
            }
            set
            {
                _CurrentDate = value;
                RaisePropertyChanged("CurrentDate");
            }

        }

        private PurchaseViewModel _NewPurchase;
        public PurchaseViewModel NewPurchase
        {
            get
            {
                return _NewPurchase;
            }
            set
            {
                _NewPurchase = value;
                RaisePropertyChanged("NewPurchase");
            }

        }

        public ObservableCollection<Product> Products { get; set; }

        private ObservableCollection<PurchaseProductsViewModel> _PurchaseProductsVM;
        public ObservableCollection<PurchaseProductsViewModel> AddedPurchaseProducts
        {
            get
            {
                return _PurchaseProductsVM;
            }
            set
            {
                _PurchaseProductsVM = value;
                RaisePropertyChanged("AddedPurchaseProducts");
            }
        }

        private PurchaseProductsViewModel _SelectedProductItem { get; set; }
        public PurchaseProductsViewModel SelectedProductItem
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

        private decimal _Quantity;
        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                if (value > 0)
                {

                    _Quantity = value;
                    RaisePropertyChanged("Quantity");
                }

            }
        }

        private decimal _Discount;
        public decimal Discount
        {
            get
            {
                return _Discount;
            }
            set
            {
                if (0 <= value && value <= 100)
                {

                    _Discount = value;
                    RaisePropertyChanged("Discount");
                }

            }
        }

        
        private decimal _PayAmount;
        public decimal PayAmount
        {
            get
            {
               
                return _PayAmount;
            }
            set
            {
                if (value >= 0 && value <= NewPurchase.Total)
                {

                    _PayAmount = value;
                    RaisePropertyChanged("PayAmount");
                }

            }
        }

        private PaymentType _PayType;
        public PaymentType PayType
        {
            get
            {
                return _PayType;
            }
            set
            {
                if (value >= 0)
                {

                    _PayType = value;
                    RaisePropertyChanged("PayType");
                }

            }
        }

        private decimal _DiscountAll;
        public decimal DiscountAll
        {
            get
            {
                return _DiscountAll;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {

                    _DiscountAll = value;
                    RaisePropertyChanged("DiscountAll");
                }

            }
        }
        private decimal _TotalDiscount;
        public decimal TotalDiscount
        {
            get
            {
                return _TotalDiscount;
            }
            set
            {
                if (value >= 0)
                {

                    _TotalDiscount = value;
                    RaisePropertyChanged("TotalDiscount");
                }

            }
        }
        
        private PurchaseProductsViewModel CloneProductToPurchaseProduct(Product product) {
            PurchaseProductsViewModel PurchaseProduct = null;
            if (product != null) {
                PurchaseProduct = new PurchaseProductsViewModel()
                {
                    //TODO Add all necessary init Data
                    ProductId = product.ProductId,
                    CESSPercent = product.CESSPercent,
                    Description = product.Description,
                    HSN = product.HSN,
                    Name = product.Name,
                    Price = product.Price,
                    Tax = product.Taxes,
                    InclusiveTax = product.TaxIncluded,

                };
            }
            else
            {
                PurchaseProduct = new PurchaseProductsViewModel();
            }
            return PurchaseProduct;


        }
        private bool IsProductEmpty() {

            if (SelectedProduct != null)
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

            PurchaseProductsViewModel ip = CloneProductToPurchaseProduct(SelectedProduct);
            ip.DiscountPercent = _Discount;
            ip.Quantity = _Quantity;
            ip.MU = measure_type;
            AddedPurchaseProducts.Add(ip);
            PurchaseDataUpdated();
            //TODO : remove added product from list so that user cannot add duplicate entry for same product in AddedPurchaseProducts 
            //Solution Must Be Expanded To reflect every type of change in AddedPurchaseProducts add,remove

            //ON HOLD
            //CODE:      Products.Remove(SelectedProduct);
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
            PurchaseDataUpdated();
        }

        private ICommand _PurchaseProductSourceUpdatedCmd;
        public ICommand PurchaseProductSourceUpdatedCmd
        {
            get
            {
                if (_PurchaseProductSourceUpdatedCmd == null)
                    _PurchaseProductSourceUpdatedCmd = new RunCommand(PurchaseProductRowAdded);
                return _PurchaseProductSourceUpdatedCmd;
            }
            set
            {
                _PurchaseProductSourceUpdatedCmd = value;
            }

        }
        private void PurchaseProductRowAdded(object obj)
        {
            
            PurchaseDataUpdated();
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
                AddedPurchaseProducts.Remove((PurchaseProductsViewModel)Item);
                PurchaseDataUpdated();
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        private bool _RoundOffTotal { get; set; }
        public bool RoundOffTotal
        {
            get
            {
                return _RoundOffTotal;
            }
            set
            {
                _RoundOffTotal = value;
                
                // Reflect rounding to total
                PurchaseDataUpdated();
                RaisePropertyChanged("RoundOffTotal");
            }
        }



        private void PurchaseDataUpdated()
        {

            try
            {
                // Initialise Total count to default before calculation

                TotalDiscount = 0;
                NewPurchase.Total = 0;
                NewPurchase.TotalTax = 0;
                NewPurchase.SubTotal = 0;
                if (AddedPurchaseProducts.Count > 0)
                {
                    IsProductListEmpty = false;
                }
                else
                {
                    IsProductListEmpty = true;
                }
                for (int index = 0; index < AddedPurchaseProducts.Count; index++)
                {
                    PurchaseProductsViewModel Item = AddedPurchaseProducts.ElementAt(index);
                    // TODO : Allow App Settings for precision (currently it is 2,hardcoded)
                    Item.TotalNoTax = Item.Quantity * Item.Price;

                    if (Item.InclusiveTax)
                    {
                        //calculate inclusive tax
                        Item.TotalNoTax = Item.TotalNoTax / ((Item.Tax.Rate / 100) + 1);
                    }

                    if (Item.DiscountPercent > 0)
                    {
                        Item.IsDiscount = true;
                    }

                    //calculate discount from discount percentage
                    Decimal DiscountAmt =
                        Item.DiscountPercent * Item.TotalNoTax / 100;

                    //deduct disount amount from item net total
                    //Item.TotalNoTax -= DiscountAmt;

                    // Find total tax amount from tax rate
                    Item.TotalTax =
                        Item.TotalNoTax * (Item.Tax.Rate) / 100;


                    //calculate total amoount
                    Item.Total =
                        Item.TotalNoTax + Item.TotalTax;

                    /*
                     *  Round to 2 decimal points
                     *  Item.Total = Decimal.Round(Item.Total, 2);
                     *  DiscountAmt = Decimal.Round(DiscountAmt, 2);
                    */
                    //update Purchase data
                    TotalDiscount += DiscountAmt;
                    NewPurchase.SubTotal += Item.TotalNoTax;
                    NewPurchase.TotalTax += Item.TotalTax;

                    Item.Total = Decimal.Round(Item.Total, 2);
                    //NewPurchase.SubTotal  -= NewPurchase.TotalTax;
                    // Re index slno after list reorder or update
                    Item.SlNo = index + 1;

                }
                // Find Grant total
                NewPurchase.Total = NewPurchase.SubTotal + NewPurchase.TotalTax - TotalDiscount;
                
                TotalDiscount = Decimal.Round(TotalDiscount,2);
                NewPurchase.Discount =TotalDiscount;
                NewPurchase.SubTotal = Decimal.Round(NewPurchase.SubTotal,2);
                NewPurchase.Total = Decimal.Round(NewPurchase.Total,(RoundOffTotal)?0:2);
                NewPurchase.TotalTax = Decimal.Round(NewPurchase.TotalTax,2);

            }
            catch (OverflowException e)
            {   
                //if integer calculation is very large and cannot be affored by Decimal type
                Console.WriteLine(e.StackTrace);
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
                if (value)
                {
                    PayAmount = NewPurchase.Total;
                }
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
                else
                {

                    for (int index = 1; index <= AddedPurchaseProducts.Count; index++)
                    {
                        var prevDiscountPercent = AddedPurchaseProducts.ElementAt(index - 1).DiscountPercent;

                        var newDiscountPercent = prevDiscountPercent -  DiscountAll;
                        
                        AddedPurchaseProducts.ElementAt(index - 1).DiscountPercent = 
                                (newDiscountPercent >= 0 && newDiscountPercent <= 100) ? 
                                    newDiscountPercent : 
                                    prevDiscountPercent;
                        
                    }
                    PurchaseDataUpdated();
                    DiscountAll = 0;
                    //DiscountAll removed action
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
                else
                {
                    PayAmount = 0;
                    PurchaseDataUpdated();
                }
                RaisePropertyChanged("AddPaymentTrigger");
            }
        }

        

        // Commands For ChildWindows


        public PurchasePayments Payments { get; set; }
    
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
                // Add Payment if obj is true
                if (PayAmount >= 1)
                {
                    Payments.Payment = new Payment()
                    {
                        Flow=CashFlow.Inflow,
                        Amount=PayAmount,
                        Type=PayType
                    };
                    SavePurchaseTest(null);
                    return;
                }

            }

            //if noop
            Payments.Payment=null;
            AddPaymentTrigger = false;

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
                //Save _Discount if obj is true
                if (DiscountAll >= 0)
                {
                    // TODO Seeking better method for discount on all items action
                    for (int index = 1; index <= AddedPurchaseProducts.Count; index++)
                    {
                        var prevDiscountPercent = AddedPurchaseProducts.ElementAt(index - 1).DiscountPercent;

                        var newDiscountPercent = prevDiscountPercent + DiscountAll;

                        AddedPurchaseProducts.ElementAt(index - 1).DiscountPercent =
                                (newDiscountPercent >= 0 && newDiscountPercent <= 100) ?
                                    newDiscountPercent :
                                    prevDiscountPercent;

                    }
                    PurchaseDataUpdated();
                    return;
                }

            }

            //if noop
            for (int index = 1; index <= AddedPurchaseProducts.Count; index++)
            {
                var prevDiscountPercent = AddedPurchaseProducts.ElementAt(index - 1).DiscountPercent;

                var newDiscountPercent = prevDiscountPercent - DiscountAll;

                AddedPurchaseProducts.ElementAt(index - 1).DiscountPercent =
                        (newDiscountPercent >= 0 && newDiscountPercent <= 100) ?
                            newDiscountPercent :
                            prevDiscountPercent;

            }
            PurchaseDataUpdated();
            DiscountAll = 0;
            DiscountAllTrigger = false;
        }
        

        //Save updation to productitem
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
            PurchaseDataUpdated();
        }

        /*
        * Test Functions
        */
        private void SavePurchaseTest(object  e) {
            


        }

        //Vendor Section

        void InitializeVendorSection() {

            Vendors = new ObservableCollection<Vendor>((from c in App.DataProvider.Vendors select c));
            
        }

        private ObservableCollection<Vendor> _Vendors;

        public ObservableCollection<Vendor> Vendors
        {
            get { return _Vendors; }
            set
            {
                _Vendors = value;
                RaisePropertyChanged("Vendors");
            }

        }

        private Vendor _PurchaseVendor;

        public Vendor PurchaseVendor
        {
            get { return _PurchaseVendor; }
            set
            {
                _PurchaseVendor = value;
                RaisePropertyChanged("PurchaseVendor");
            }

        }

        private bool _IsBothAddressSame;

        public bool IsBothAddressSame
        {
            get { return _IsBothAddressSame; }
            set {
                _IsBothAddressSame = value;
                if(value && PurchaseVendor != null)
                {
                    PurchaseVendor.ShippingAddress = PurchaseVendor.BillingAddress;
                    PurchaseVendor.ShippingZIP = PurchaseVendor.BillingZIP;
                    PurchaseVendor.ShippingCity = PurchaseVendor.BillingCity;

                }
                RaisePropertyChanged("IsBothAddressSame");
            }
        }

        private String _NewVendorName;

        public String NewVendorName
        {
            get { return _NewVendorName; }
            set {
                _NewVendorName = value;
                if (PurchaseVendor == null)
                {
                    PurchaseVendor = new Vendor()
                    {
                        Name = value
                    };
                }
                RaisePropertyChanged("NewVendorName");
            }
        }




        //Navigation Items


        private int _CurrentTab;

        public int CurrentTab
        {
            get { return _CurrentTab; }
            set {
                _CurrentTab = value;
                RaisePropertyChanged("CurrentTab");  
            }
        }

        private ICommand _NavigateToCmd;
        public ICommand NavigateToCmd
        {
            get
            {
                if (_NavigateToCmd == null)
                    _NavigateToCmd = new RunCommand(NavigateTo);
                return _NavigateToCmd;
            }
            set
            {
                _NavigateToCmd = value;
            }

        }

        private void NavigateTo(object obj)
        {
            try
            {
                if (obj == null) throw new InvalidOperationException("Object is null");
                CurrentTab = (Int16)obj;
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }


        // Final Action
        private ICommand _SavePurchaseCmd;
        public ICommand SavePurchaseCmd
        {
            get
            {
                if (_SavePurchaseCmd == null)
                    _SavePurchaseCmd = new RunCommand(SavePurchase);
                return _SavePurchaseCmd;
            }
            set
            {
                _SavePurchaseCmd = value;
            }

        }
        

        public void SavePurchase(object obj)
        {
            //Sample Code for saving data to database
            foreach (var Purchase_product in AddedPurchaseProducts)
            {
                NewPurchase.GetPurchase().Products.Add(Purchase_product.GetPurchaseProducts());
            }

            if (Payments != null)
                NewPurchase.GetPurchase().Payments.Add(Payments);
            NewPurchase.GetPurchase().Vendor = PurchaseVendor;
            App.DataProvider.Purchases.Add(NewPurchase.GetPurchase());

            //TODO Catch Inner Exceptions too

            //try
            //{
                App.DataProvider.SaveChanges();
            //}
            //catch(Exception e)
            //{
                //DialogCoordinator.ShowMessageAsync(this, "Error Occured", "Error occured while saving Purchase\n"+e.Message,MessageDialogStyle.Affirmative);
                //Console.WriteLine("Error occured while updating database");
                //Console.WriteLine(e.StackTrace);
            //}
            /*
            NewPurchase = new PurchaseViewModel();
            AddedPurchaseProducts.Clear();
            PurchaseDataUpdated();
            */
        }

        
    }
    

}
