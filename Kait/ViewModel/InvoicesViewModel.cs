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
    public class InvoicesViewModel:NotifyUIBase
    {
        public InvoicesViewModel()
        {
            InvoicesList = new ObservableCollection<Invoice>(App.DataProvider.Invoices.AsEnumerable());
            InvoicesList.CollectionChanged +=TrackChange;
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
        
        

        private ObservableCollection<Invoice> _InvoicesList;

        public ObservableCollection<Invoice> InvoicesList
        {
            get {
                return _InvoicesList;
            }
            set {
                _InvoicesList = value;
                TrackChange(this, null);
                RaisePropertyChanged("InvoicesList");
            }
        }

        private Invoice _SelectedInvoiceItem { get; set; }
        public Invoice SelectedInvoiceItem
        {
            get
            {
                return _SelectedInvoiceItem;
            }
            set
            {

                _SelectedInvoiceItem = value;
                RaisePropertyChanged("SelectedInvoiceItem");
            }
        }


        private ICommand _EditInvoiceCmd;
        public ICommand EditInvoiceCmd
        {
            get
            {
                if (_EditInvoiceCmd == null)
                    _EditInvoiceCmd = new RunCommand(EditInvoiceItem);
                return _EditInvoiceCmd;
            }
            set
            {
                _EditInvoiceCmd = value;
            }

        }

        private void EditInvoiceItem(object obj)
        {
            
            MainWindow.PageHostService.Navigate(
                new NewInvoice(
                    new InvoiceViewModel(SelectedInvoiceItem)
                )
            );
        }


        private Invoice _NewInvoiceItem { get; set; }
        public Invoice NewInvoiceItem
        {
            get
            {
                return _NewInvoiceItem;
            }
            set
            {

                _NewInvoiceItem = value;
                RaisePropertyChanged("NewInvoiceItem");
            }
        }

        private bool _IsNewInvoiceItemOpen { get; set; }
        public bool IsNewInvoiceItemOpen
        {
            get
            {
                return _IsNewInvoiceItemOpen;
            }
            set
            {
                _IsNewInvoiceItemOpen = value;
                RaisePropertyChanged("IsNewInvoiceItemOpen");
            }
        }



        private ICommand _AddInvoiceCmd;
        public ICommand AddInvoiceCmd
        {
            get
            {
                if (_AddInvoiceCmd == null)
                    _AddInvoiceCmd = new RunCommand(AddInvoiceItem);
                return _AddInvoiceCmd;
            }
            set
            {
                _AddInvoiceCmd = value;
            }

        }

        private void AddInvoiceItem(object obj)
        {
            MainWindow.PageHostService.Navigate(new NewInvoice());
        }



        /*
         * ChildWindowVisibility Handles
         * 
         */


        private ICommand _RmInvoiceCmd;
        public ICommand RmInvoiceCmd
        {
            get
            {
                if (_RmInvoiceCmd == null)
                    _RmInvoiceCmd = new RunCommand(RemoveInvoiceItem);
                return _RmInvoiceCmd;
            }
            set
            {
                _RmInvoiceCmd = value;
            }

        }
        private void RemoveInvoiceItem(object Item)
        {
            try
            {

                foreach (var invoiceProduct in (Item as Invoice).Products.ToList())
                {
                    App.DataProvider.InvoiceProducts.Remove(invoiceProduct);
                }
                App.DataProvider.Invoices.Remove((Invoice)Item);

                App.DataProvider.SaveChanges();

                InvoicesList.Remove((Invoice)Item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }


        public void TrackChange(object sender, NotifyCollectionChangedEventArgs e)
        {


            if (InvoicesList!=null && InvoicesList.Count == 0)
            {
                IsInvoiceListEmpty = true;
            }
            else
            {
                IsInvoiceListEmpty = false;

            }
        }

        private bool _IsInvoiceListEmpty;
        public bool IsInvoiceListEmpty
        {
            get
            {
                return _IsInvoiceListEmpty;
            }
            set
            {
                _IsInvoiceListEmpty = value;
                RaisePropertyChanged("IsInvoiceListEmpty");
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

        public static Expression<Func<Invoice,bool>> FindInvoices( string key, string type)
        {

            //CODE_CLEANUP: the return type should always be a valid Linq expression this makes code readablity sucking


            //show all if empty
            if(key == "" || key == null )
            {
                return (Invoice => true);
            }

            switch (type)
            {
                default:
                    return (Invoice=>true);
            }
        }
        private ICommand _SearchCmd;
        public ICommand SearchCmd
        {
            get
            {
                if (_SearchCmd == null)
                    _SearchCmd = new RunCommand(SearchInvoice);
                return _SearchCmd;
            }
            set
            {
                _SearchCmd = value;
            }

        }
        private void SearchInvoice(object Item)
        {
            InvoicesList = null;
            //BUG:Exception NotSupportedException: Reason Direct Call to FindInvoices function is not possible 

            InvoicesList = new ObservableCollection<Invoice>(
                     App.DataProvider.Invoices.Where(FindInvoices(SearchKey,SearchFilter))
                 );
           
        }

    }
}
