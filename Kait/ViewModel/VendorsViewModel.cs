using Kait.Support;
using Kait.View.Pages;
using Provider.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Input;

namespace Kait.ViewModel
{
    public class VendorsViewModel:NotifyUIBase
    {
        public VendorsViewModel()
        {
            VendorsList = new ObservableCollection<Vendor>(App.DataProvider.Vendors.AsEnumerable());
            VendorsList.CollectionChanged +=TrackChange;
            CurrentDate = DateTime.Today;
            Taxes = App.DataProvider.Taxes.ToList();
            MeasureTypes = Enum.GetValues(typeof(Measure)).Cast<Measure>();

            // initialize search filters in search section
            InitFilters();
        }

        private DateTime _CurrentDate { get; set; }

        public DateTime CurrentDate {
            get{
                return _CurrentDate;
            }
            set
            {
                _CurrentDate = value;
                RaisePropertyChanged("CurrentDate");
            }
        }

        public List<Taxes> Taxes { get; private set; }

        public IEnumerable<Measure> MeasureTypes { get; }

        private ObservableCollection<Vendor> _VendorsList;

        public ObservableCollection<Vendor> VendorsList
        {
            get {
                return _VendorsList;
            }
            set {
                _VendorsList = value;
                TrackChange(this, null);
                RaisePropertyChanged("VendorsList");
            }
        }

        private Vendor _SelectedVendorItem { get; set; }
        public Vendor SelectedVendorItem
        {
            get
            {
                return _SelectedVendorItem;
            }
            set
            {

                _SelectedVendorItem = value;
                RaisePropertyChanged("SelectedVendorItem");
            }
        }


        private ICommand _EditVendorCmd;
        public ICommand EditVendorCmd
        {
            get
            {
                if (_EditVendorCmd == null)
                    _EditVendorCmd = new RunCommand(EditVendorItem);
                return _EditVendorCmd;
            }
            set
            {
                _EditVendorCmd = value;
            }

        }

        private void EditVendorItem(object obj)
        {
            IsEditVendorItemOpen = true;
            try
            {
                if ((bool)obj)
                {
                    EditMode = true;
                }
                else
                {
                    EditMode = false;
                }
            }
            catch (InvalidCastException e)
            {

                EditMode = false;
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }


        private Vendor _NewVendorItem { get; set; }
        public Vendor NewVendorItem
        {
            get
            {
                return _NewVendorItem;
            }
            set
            {

                _NewVendorItem = value;
                RaisePropertyChanged("NewVendorItem");
            }
        }

        private bool _IsNewVendorItemOpen { get; set; }
        public bool IsNewVendorItemOpen
        {
            get
            {
                return _IsNewVendorItemOpen;
            }
            set
            {
                _IsNewVendorItemOpen = value;
                RaisePropertyChanged("IsNewVendorItemOpen");
            }
        }



        private ICommand _AddVendorCmd;
        public ICommand AddVendorCmd
        {
            get
            {
                if (_AddVendorCmd == null)
                    _AddVendorCmd = new RunCommand(AddVendorItem);
                return _AddVendorCmd;
            }
            set
            {
                _AddVendorCmd = value;
            }

        }

        private void AddVendorItem(object obj)
        {
            try
            {
                //save new Vendor
                if ((bool)obj)
                {
                    if (NewVendorItem != null)
                    {
                        App.DataProvider.Vendors.Add(NewVendorItem);
                        if (App.DataProvider.SaveChanges()==1)
                        {
                            VendorsList = new ObservableCollection<Vendor>(App.DataProvider.Vendors.AsEnumerable());
                        }
                        else
                        {
                            Console.WriteLine("Error Saving Data to VendorList");
                        }
                        IsNewVendorItemOpen = false;
                    }
                }
                // if mode is show new Vendor layout
                else
                {
                    NewVendorItem = new Vendor();
                    IsNewVendorItemOpen = true;
                }
            }
            catch (InvalidCastException e)
            {

                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        //Save updation to Vendoritem
        private ICommand _SaveEditsVendorCmd;
        public ICommand SaveEditsVendorCmd
        {
            get
            {
                if (_SaveEditsVendorCmd == null)
                    _SaveEditsVendorCmd = new RunCommand(SaveEditsVendorItem);
                return _SaveEditsVendorCmd;
            }
            set
            {
                _SaveEditsVendorCmd = value;
            }

        }

        /*
         * ChildWindowVisibility Handles
         * 
         */
        private bool _IsEditVendorItemOpen { get; set; }
        public bool IsEditVendorItemOpen
        {
            get
            {
                return _IsEditVendorItemOpen;
            }
            set
            {
                _IsEditVendorItemOpen = value;
                RaisePropertyChanged("IsEditVendorItemOpen");
            }
        }

        private bool _EditMode { get; set; }
        public bool EditMode {
            get {
                return _EditMode;
            }
            set {
                _EditMode = value;
                if (value)
                {
                    PrimaryElementContent = "Save";
                }
                else
                {
                    PrimaryElementContent = "Close";
                }
                RaisePropertyChanged("EditMode");
            }
        }

        private void SaveEditsVendorItem(object obj)
        {
            if (EditMode)
            {
                App.DataProvider.SaveChanges();
            }
            IsEditVendorItemOpen = false;
            
        }

        private ICommand _RmVendorCmd;
        public ICommand RmVendorCmd
        {
            get
            {
                if (_RmVendorCmd == null)
                    _RmVendorCmd = new RunCommand(RemoveVendorItem);
                return _RmVendorCmd;
            }
            set
            {
                _RmVendorCmd = value;
            }

        }
        private void RemoveVendorItem(object Item)
        {
            try
            {
                VendorsList.Remove((Vendor)Item);
                App.DataProvider.Vendors.Remove((Vendor)Item);
                App.DataProvider.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }


        public void TrackChange(object sender, NotifyCollectionChangedEventArgs e)
        {


            if (VendorsList!=null && VendorsList.Count == 0)
            {
                IsVendorListEmpty = true;
            }
            else
            {
                IsVendorListEmpty = false;

            }
        }

        private bool _IsVendorListEmpty;
        public bool IsVendorListEmpty
        {
            get
            {
                return _IsVendorListEmpty;
            }
            set
            {
                _IsVendorListEmpty = value;
                RaisePropertyChanged("IsVendorListEmpty");
            }
        }

        private string _PrimaryElementContent;

        public string PrimaryElementContent
        {
            get { return _PrimaryElementContent; }
            set {
                _PrimaryElementContent = value;
                RaisePropertyChanged("PrimaryElementContent");
            }
        }



        private string _SearchKey { get; set; }
        public string SearchKey
        {
            get
            {
                return _SearchKey;
            }
            set
            {

                _SearchKey = value;
                RaisePropertyChanged("SearchKey");
            }
        }

        private string _SearchFilter { get; set; }
        public string SearchFilter
        {
            get
            {
                return _SearchFilter;
            }
            set
            {

                _SearchFilter = value;
                RaisePropertyChanged("SearchFilter");
            }
        }

        

        //Search Section

        public static List<string> Filters { get; private set; }

        public void InitFilters()
        {
            Filters = new List<string>();
            Filters.Add("All");
            Filters.Add("Name");
            Filters.Add("Description");
            Filters.Add("HSN");
            SearchFilter = Filters[0];
        }

        public static Expression<Func<Vendor,bool>> FindVendors( string key, string type)
        {

            //CODE_CLEANUP: the return type should always be a valid Linq expression this makes code readablity sucking


            //show all if empty
            if(key == "" || key == null )
            {
                return (Vendor => true);
            }

            switch (type)
            {
                case "Name":
                    return (Vendor=>Vendor.Name.ToLower().Contains(key.ToLower()));
                case "Address":
                    return (Vendor=> Vendor.BillingAddress.ToLower().Contains(key.ToLower()) ||
                                     Vendor.ShippingAddress.ToLower().Contains(key.ToLower())
                            );
                 case "All":
                    return (Vendor => Vendor.BillingAddress.ToLower().Contains(key.ToLower()) || 
                    
                                       Vendor.Name.ToLower().Contains(key.ToLower()) || 
                                       
                                       Vendor.ShippingAddress.ToString().ToLower().Contains(key.ToLower()) || 
                                       
                                       Vendor.Details.ToLower().Contains(key.ToLower())
                          );
                default:
                    return (Vendor=>true);
            }
        }
        private ICommand _SearchCmd;
        public ICommand SearchCmd
        {
            get
            {
                if (_SearchCmd == null)
                    _SearchCmd = new RunCommand(SearchVendor);
                return _SearchCmd;
            }
            set
            {
                _SearchCmd = value;
            }

        }
        private void SearchVendor(object Item)
        {
            VendorsList = null;
            //BUG:Exception NotSupportedException: Reason Direct Call to FindVendors function is not possible 

            VendorsList = new ObservableCollection<Vendor>(
                     App.DataProvider.Vendors.Where(FindVendors(SearchKey,SearchFilter))
                 );
           
        }

    }
}
