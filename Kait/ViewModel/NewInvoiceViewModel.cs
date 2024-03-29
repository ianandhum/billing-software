﻿using System;
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
    public class NewInvoiceViewModel : NotifyUIBase
    {

        private IDialogCoordinator DialogCoordinator;

        public NewInvoiceViewModel(IDialogCoordinator iDialogCoordinator)
        {
            InitializeViewModel();
            DialogCoordinator = iDialogCoordinator;
            FreshInvoice = true;

        }
        public NewInvoiceViewModel(IDialogCoordinator iDialogCoordinator,InvoiceViewModel existingViewModel)
        {
            InitializeViewModelWithInvoice(existingViewModel);
            DialogCoordinator = iDialogCoordinator;
            FreshInvoice = false;
        }
        
        private bool FreshInvoice { get; set; }
        private void InitializeViewModel()
        {
            CurrentDate = DateTime.Today;
            string NextInvoiceId = "1";
            try
            {
                NextInvoiceId = (App.DataProvider.Invoices.Max((x) => x.InvoiceId) + 1).ToString();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.StackTrace);

            }
            finally
            {
                MetaData = new MetaData(App.DataProvider)
                {

                    Invoice = new MetaData.InvoiceMeta()
                    {
                        NextInvoiceId = NextInvoiceId,
                        Prefix = App.GetConfig("InvoicePrefix"),
                        TypeOnPrint = App.GetConfig("FirstTypeOnInvoicePrint")
                    }
                };
            }

            NewInvoice = new InvoiceViewModel()
            {
                IssueDate = DateTime.Today,
                DueDate = DateTime.Today
            };
            InitializeAddProductSection();
            InitializeInvoiceProducts();
            InitializePayments();
            InitializeClientSection();
        }
        private void InitializeViewModelWithInvoice(InvoiceViewModel invoice)
        {
            CurrentDate = DateTime.Today;
            MetaData = new MetaData(App.DataProvider)
            {

                Invoice = new MetaData.InvoiceMeta()
                {
                    NextInvoiceId = invoice.InvoiceId.ToString(),
                    Prefix = App.GetConfig("InvoicePrefix"),
                    TypeOnPrint = App.GetConfig("FirstTypeOnInvoicePrint")
                }
            };
            NewInvoice = invoice;
            InitializeAddProductSection();
            InitializeInvoiceProducts(new ObservableCollection<InvoiceProducts>(invoice.GetInvoice().Products));
            InitializePayments(invoice.GetInvoice().Payments.FirstOrDefault());
            InitializeClientSection(invoice.GetInvoice().Client);
            InvoiceDataUpdated();
        }
        void InitializeNavigation() {
            CurrentTab = 0;
        }

        void InitializePayments(InvoicePayments invoicePayment=null)
        {
            InvoicePayments payments = new InvoicePayments();
            if(invoicePayment != null)
            {
                payments = invoicePayment;
                
            }
            Payments = payments;
        }
        void InitializeAddProductSection()
        {
            Products = new ObservableCollection<Product>((from product in App.DataProvider.Products select product));

            if (Products.Count() > 0)
            {
                SelectedProduct = Products.FirstOrDefault();
                MeasureTypes = Enum.GetValues(typeof(Measure)).Cast<Measure>();
                
            }
        }
        void InitializeInvoiceProducts(ObservableCollection<InvoiceProducts> IProducts=null)
        {

            ObservableCollection<InvoiceProductsViewModel> invoiceProductsVMs=new ObservableCollection<InvoiceProductsViewModel>();
            if (IProducts != null) foreach (var item in IProducts)
            {
                invoiceProductsVMs.Add(new InvoiceProductsViewModel(item));
            }

            AddedInvoiceProducts = invoiceProductsVMs;
            if (IProducts is null)
                IsProductListEmpty = true;
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

        private InvoiceViewModel _NewInvoice;
        public InvoiceViewModel NewInvoice
        {
            get
            {
                return _NewInvoice;
            }
            set
            {
                _NewInvoice = value;
                RaisePropertyChanged("NewInvoice");
            }

        }

        public ObservableCollection<Product> Products { get; set; }

        private ObservableCollection<InvoiceProductsViewModel> _InvoiceProductsVM;
        public ObservableCollection<InvoiceProductsViewModel> AddedInvoiceProducts
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

        private InvoiceProductsViewModel _SelectedProductItem { get; set; }
        public InvoiceProductsViewModel SelectedProductItem
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
                if (value >= 0 && value <= NewInvoice.Total)
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
        
        private InvoiceProductsViewModel CloneProductToInvoiceProduct(Product product) {
            InvoiceProductsViewModel invoiceProduct = null;
            if (product != null) {
                invoiceProduct = new InvoiceProductsViewModel()
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
                invoiceProduct = new InvoiceProductsViewModel();
            }
            return invoiceProduct;


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

            InvoiceProductsViewModel ip = CloneProductToInvoiceProduct(SelectedProduct);
            ip.DiscountPercent = _Discount;
            ip.Quantity = _Quantity;
            ip.MU = measure_type;
            AddedInvoiceProducts.Add(ip);
            InvoiceDataUpdated();
            //TODO : remove added product from list so that user cannot add duplicate entry for same product in AddedInvoiceProducts 
            //Solution Must Be Expanded To reflect every type of change in AddedInvoiceProducts add,remove

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
            InvoiceDataUpdated();
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
            
            InvoiceDataUpdated();
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
                AddedInvoiceProducts.Remove((InvoiceProductsViewModel)Item);
                if (!FreshInvoice)
                    App.DataProvider.InvoiceProducts.Remove(((InvoiceProductsViewModel)Item).GetInvoiceProducts());
                InvoiceDataUpdated();
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
                InvoiceDataUpdated();
                RaisePropertyChanged("RoundOffTotal");
            }
        }



        private void InvoiceDataUpdated()
        {

            try
            {
                // Initialise Total count to default before calculation

                TotalDiscount = 0;
                NewInvoice.Total = 0;
                NewInvoice.TotalTax = 0;
                NewInvoice.SubTotal = 0;
                if (AddedInvoiceProducts.Count > 0)
                {
                    IsProductListEmpty = false;
                }
                else
                {
                    IsProductListEmpty = true;
                }
                for (int index = 0; index < AddedInvoiceProducts.Count; index++)
                {
                    InvoiceProductsViewModel Item = AddedInvoiceProducts.ElementAt(index);
                    // TODO : Allow App Settings for precision (currently it is App.GetConfig("RoundOffValues"),hardcoded)
                    Item.TotalNoTax = Item.Quantity * Item.Price;

                    //calculate discount from discount percentage
                    Decimal DiscountAmt =
                        Item.DiscountPercent * Item.TotalNoTax / 100;
                    
                    if (Item.InclusiveTax)
                    {
                        //calculate inclusive tax
                        Item.TotalNoTax = Item.TotalNoTax - DiscountAmt;
                        Item.TotalNoTax = Item.TotalNoTax / ((Item.Tax.Rate / 100) + 1);
                    }

                    if (Item.DiscountPercent > 0)
                    {
                        Item.IsDiscount = true;
                    }

                    //deduct disount amount from item net total
                    //Item.TotalNoTax -= DiscountAmt;

                    if (Item.InclusiveTax)
                    {
                        Item.TotalTax = Item.TotalNoTax * Item.Tax.Rate / 100;

                    }
                    else
                    {
                        // Find total tax amount from tax rate
                        Item.TotalTax =
                            (Item.TotalNoTax - DiscountAmt) * (Item.Tax.Rate) / 100;
                    }
                    //calculate total amoount
                    Item.Total =
                        Item.TotalNoTax + Item.TotalTax;

                    /*
                     *  Round to App.GetConfig("RoundOffValues") decimal points
                     *  Item.Total = Decimal.Round(Item.Total, App.GetConfig("RoundOffValues"));
                     *  DiscountAmt = Decimal.Round(DiscountAmt, App.GetConfig("RoundOffValues"));
                    */
                    //update invoice data
                    TotalDiscount += DiscountAmt;
                    NewInvoice.SubTotal += Item.TotalNoTax;
                    NewInvoice.TotalTax += Item.TotalTax;

                    Item.Total = Decimal.Round(Item.Total, Convert.ToInt16(App.GetConfig("RoundOffValues")));
                    //NewInvoice.SubTotal  -= NewInvoice.TotalTax;
                    // Re index slno after list reorder or update
                    Item.SlNo = index + 1;

                }
                // Find Grant total
                NewInvoice.Total = NewInvoice.SubTotal + NewInvoice.TotalTax - TotalDiscount  + NewInvoice.ShippingCharge;
                
                TotalDiscount = Decimal.Round(TotalDiscount, Convert.ToInt16(App.GetConfig("RoundOffValues")));
                NewInvoice.Discount =TotalDiscount;
                NewInvoice.SubTotal = Decimal.Round(NewInvoice.SubTotal, Convert.ToInt16(App.GetConfig("RoundOffValues")));
                NewInvoice.Total = Decimal.Round(NewInvoice.Total,(RoundOffTotal)?0: Convert.ToInt16(App.GetConfig("RoundOffValues")));
                NewInvoice.TotalTax = Decimal.Round(NewInvoice.TotalTax, Convert.ToInt16(App.GetConfig("RoundOffValues")));

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
                    PayAmount = NewInvoice.Total;
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

                    for (int index = 1; index <= AddedInvoiceProducts.Count; index++)
                    {
                        var prevDiscountPercent = AddedInvoiceProducts.ElementAt(index - 1).DiscountPercent;

                        var newDiscountPercent = prevDiscountPercent -  DiscountAll;
                        
                        AddedInvoiceProducts.ElementAt(index - 1).DiscountPercent = 
                                (newDiscountPercent >= 0 && newDiscountPercent <= 100) ? 
                                    newDiscountPercent : 
                                    prevDiscountPercent;
                        
                    }
                    InvoiceDataUpdated();
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
                    InvoiceDataUpdated();
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
                else
                {
                    NewInvoice.ShippingCharge = 0;
                    InvoiceDataUpdated();
                }
                RaisePropertyChanged("ShippingChargeTrigger");
            }
        }

        // Commands For ChildWindows


        public InvoicePayments Payments { get; set; }
    
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
                    for (int index = 1; index <= AddedInvoiceProducts.Count; index++)
                    {
                        var prevDiscountPercent = AddedInvoiceProducts.ElementAt(index - 1).DiscountPercent;

                        var newDiscountPercent = prevDiscountPercent + DiscountAll;

                        AddedInvoiceProducts.ElementAt(index - 1).DiscountPercent =
                                (newDiscountPercent >= 0 && newDiscountPercent <= 100) ?
                                    newDiscountPercent :
                                    prevDiscountPercent;

                    }
                    InvoiceDataUpdated();
                    return;
                }

            }

            //if noop
            for (int index = 1; index <= AddedInvoiceProducts.Count; index++)
            {
                var prevDiscountPercent = AddedInvoiceProducts.ElementAt(index - 1).DiscountPercent;

                var newDiscountPercent = prevDiscountPercent - DiscountAll;

                AddedInvoiceProducts.ElementAt(index - 1).DiscountPercent =
                        (newDiscountPercent >= 0 && newDiscountPercent <= 100) ?
                            newDiscountPercent :
                            prevDiscountPercent;

            }
            InvoiceDataUpdated();
            DiscountAll = 0;
            DiscountAllTrigger = false;
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
                //Shipping and packaging charge added if obj is true
                if (NewInvoice.ShippingCharge > 0)
                {
                    InvoiceDataUpdated();
                    return;
                }
                
            }
            NewInvoice.ShippingCharge = 0;

            //if noop
            ShippingChargeTrigger = false;
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
            InvoiceDataUpdated();
        }

        /*
        * Test Functions
        */
        private void SaveInvoiceTest(object  e) {
            


        }

        //Client Section

        void InitializeClientSection(Client client = null) {

            Clients = new ObservableCollection<Client>((from c in App.DataProvider.Clients select c));
            if (client != null)
            {
                InvoiceClient = client;
            }
        }

        private ObservableCollection<Client> _Clients;

        public ObservableCollection<Client> Clients
        {
            get { return _Clients; }
            set
            {
                _Clients = value;
                RaisePropertyChanged("Clients");
            }

        }

        private Client _InvoiceClient;

        public Client InvoiceClient
        {
            get { return _InvoiceClient; }
            set
            {
                _InvoiceClient = value;
                if (_InvoiceClient is null)
                    _InvoiceClient = new Client();
                NewClientName = _InvoiceClient.Name;
                RaisePropertyChanged("InvoiceClient");
            }

        }

        private bool _IsBothAddressSame;

        public bool IsBothAddressSame
        {
            get { return _IsBothAddressSame; }
            set {
                _IsBothAddressSame = value;
                if (InvoiceClient != null)
                    InvoiceClient = new Client();
                if(value)
                {
                    InvoiceClient.ShippingAddress = InvoiceClient.BillingAddress;
                    InvoiceClient.ShippingZIP = InvoiceClient.BillingZIP;
                    InvoiceClient.ShippingCity = InvoiceClient.BillingCity;

                }
                RaisePropertyChanged("IsBothAddressSame");
            }
        }

        private String _NewClientName;

        public String NewClientName
        {
            get { return _NewClientName; }
            set {
                _NewClientName = value;
                if (InvoiceClient == null)
                {
                    InvoiceClient = new Client()
                    {
                        Name = value
                    };
                }
                RaisePropertyChanged("NewClientName");
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
        private ICommand _SaveInvoiceCmd;
        public ICommand SaveInvoiceCmd
        {
            get
            {
                if (_SaveInvoiceCmd == null)
                    _SaveInvoiceCmd = new RunCommand(SaveInvoice);
                return _SaveInvoiceCmd;
            }
            set
            {
                _SaveInvoiceCmd = value;
            }

        }
        

        public void SaveInvoice(object obj)
        {
            Invoice Invoice = NewInvoice.GetInvoice();
            //Sample Code for saving data to database
            foreach (var invoice_product in Invoice.Products.ToList())
            {
                if (!AddedInvoiceProducts.Where(x=>x.ProductId == invoice_product.ProductId).Any())
                {
                    Invoice.Products.Remove(invoice_product);
                    App.DataProvider.InvoiceProducts.Remove(invoice_product);
                    Console.WriteLine("Product:{0} Removed" ,invoice_product.Name);
                }
            }

            foreach (var invoice_product in AddedInvoiceProducts)
            {
                Invoice.Products.Add(invoice_product.GetInvoiceProducts());
                Console.WriteLine("Product:{0} Added", invoice_product.Name);

            }


            if (Payments != null)
                Invoice.Payments.Add(Payments);
            if(InvoiceClient != null)
                Invoice.Client = InvoiceClient;
            if(!App.DataProvider.Invoices.Any(item=> item.InvoiceId==Invoice.InvoiceId))
                App.DataProvider.Invoices.Add(Invoice);
            try
            {
                App.DataProvider.SaveChanges();
                
            }
            catch(Exception e)
            {
                DialogCoordinator.ShowMessageAsync(this, "Error Occured", "Error occured while saving invoice\n"+e.Message,MessageDialogStyle.Affirmative);
                Console.WriteLine("Error occured while updating database");
                Console.WriteLine(e.StackTrace);
            }
            
            /*
            NewInvoice = new InvoiceViewModel();
            AddedInvoiceProducts.Clear();
            InvoiceDataUpdated();
            */
        }

        
    }
    

}
