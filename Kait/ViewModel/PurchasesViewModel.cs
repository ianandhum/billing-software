using Kait.Support;
using Kait.View.Pages;
using Kait.ViewModel.Primitive;
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
    public class PurchasesViewModel:NotifyUIBase
    {
        public PurchasesViewModel()
        {
            PurchasesList = new ObservableCollection<Purchase>(App.DataProvider.Purchases.AsEnumerable());
            PurchasesList.CollectionChanged +=TrackChange;
            CurrentDate = DateTime.Today;
            
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
        
        

        private ObservableCollection<Purchase> _PurchasesList;

        public ObservableCollection<Purchase> PurchasesList
        {
            get {
                return _PurchasesList;
            }
            set {
                _PurchasesList = value;
                TrackChange(this, null);
                RaisePropertyChanged("PurchasesList");
            }
        }

        private Purchase _SelectedPurchaseItem { get; set; }
        public Purchase SelectedPurchaseItem
        {
            get
            {
                return _SelectedPurchaseItem;
            }
            set
            {

                _SelectedPurchaseItem = value;
                RaisePropertyChanged("SelectedPurchaseItem");
            }
        }


        private ICommand _EditPurchaseCmd;
        public ICommand EditPurchaseCmd
        {
            get
            {
                if (_EditPurchaseCmd == null)
                    _EditPurchaseCmd = new RunCommand(EditPurchaseItem);
                return _EditPurchaseCmd;
            }
            set
            {
                _EditPurchaseCmd = value;
            }

        }

        private void EditPurchaseItem(object obj)
        {
            
            MainWindow.PageHostService.Navigate(
                new NewPurchase(
                    new PurchaseViewModel(SelectedPurchaseItem)
                )
            );
        }


        private Purchase _NewPurchaseItem { get; set; }
        public Purchase NewPurchaseItem
        {
            get
            {
                return _NewPurchaseItem;
            }
            set
            {

                _NewPurchaseItem = value;
                RaisePropertyChanged("NewPurchaseItem");
            }
        }

        private bool _IsNewPurchaseItemOpen { get; set; }
        public bool IsNewPurchaseItemOpen
        {
            get
            {
                return _IsNewPurchaseItemOpen;
            }
            set
            {
                _IsNewPurchaseItemOpen = value;
                RaisePropertyChanged("IsNewPurchaseItemOpen");
            }
        }



        private ICommand _AddPurchaseCmd;
        public ICommand AddPurchaseCmd
        {
            get
            {
                if (_AddPurchaseCmd == null)
                    _AddPurchaseCmd = new RunCommand(AddPurchaseItem);
                return _AddPurchaseCmd;
            }
            set
            {
                _AddPurchaseCmd = value;
            }

        }

        private void AddPurchaseItem(object obj)
        {
            MainWindow.PageHostService.Navigate(new NewPurchase());
        }



        /*
         * ChildWindowVisibility Handles
         * 
         */


        private ICommand _RmPurchaseCmd;
        public ICommand RmPurchaseCmd
        {
            get
            {
                if (_RmPurchaseCmd == null)
                    _RmPurchaseCmd = new RunCommand(RemovePurchaseItem);
                return _RmPurchaseCmd;
            }
            set
            {
                _RmPurchaseCmd = value;
            }

        }
        private void RemovePurchaseItem(object Item)
        {
            try
            {

                foreach (var PurchaseProduct in (Item as Purchase).Products.ToList())
                {
                    App.DataProvider.PurchaseProducts.Remove(PurchaseProduct);
                }
                App.DataProvider.Purchases.Remove((Purchase)Item);

                App.DataProvider.SaveChanges();

                PurchasesList.Remove((Purchase)Item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }


        public void TrackChange(object sender, NotifyCollectionChangedEventArgs e)
        {


            if (PurchasesList!=null && PurchasesList.Count == 0)
            {
                IsPurchaseListEmpty = true;
            }
            else
            {
                IsPurchaseListEmpty = false;

            }
        }

        private bool _IsPurchaseListEmpty;
        public bool IsPurchaseListEmpty
        {
            get
            {
                return _IsPurchaseListEmpty;
            }
            set
            {
                _IsPurchaseListEmpty = value;
                RaisePropertyChanged("IsPurchaseListEmpty");
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
            SearchFilter = Filters[0];
        }

        public static Expression<Func<Purchase,bool>> FindPurchases( string key, string type)
        {

            //CODE_CLEANUP: the return type should always be a valid Linq expression this makes code readablity sucking


            //show all if empty
            if(key == "" || key == null )
            {
                return (Purchase => true);
            }

            switch (type)
            {
                default:
                    return (Purchase=>true);
            }
        }
        private ICommand _SearchCmd;
        public ICommand SearchCmd
        {
            get
            {
                if (_SearchCmd == null)
                    _SearchCmd = new RunCommand(SearchPurchase);
                return _SearchCmd;
            }
            set
            {
                _SearchCmd = value;
            }

        }
        private void SearchPurchase(object Item)
        {
            PurchasesList = null;
            //BUG:Exception NotSupportedException: Reason Direct Call to FindPurchases function is not possible 

            PurchasesList = new ObservableCollection<Purchase>(
                     App.DataProvider.Purchases.Where(FindPurchases(SearchKey,SearchFilter))
                 );
           
        }

    }
}
