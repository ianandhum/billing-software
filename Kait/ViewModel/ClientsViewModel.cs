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
    public class ClientsViewModel:NotifyUIBase
    {
        public ClientsViewModel()
        {
            ClientsList = new ObservableCollection<Client>(App.DataProvider.Clients.AsEnumerable());
            ClientsList.CollectionChanged +=TrackChange;
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

        private ObservableCollection<Client> _ClientsList;

        public ObservableCollection<Client> ClientsList
        {
            get {
                return _ClientsList;
            }
            set {
                _ClientsList = value;
                TrackChange(this, null);
                RaisePropertyChanged("ClientsList");
            }
        }

        private Client _SelectedClientItem { get; set; }
        public Client SelectedClientItem
        {
            get
            {
                return _SelectedClientItem;
            }
            set
            {

                _SelectedClientItem = value;
                RaisePropertyChanged("SelectedClientItem");
            }
        }


        private ICommand _EditClientCmd;
        public ICommand EditClientCmd
        {
            get
            {
                if (_EditClientCmd == null)
                    _EditClientCmd = new RunCommand(EditClientItem);
                return _EditClientCmd;
            }
            set
            {
                _EditClientCmd = value;
            }

        }

        private void EditClientItem(object obj)
        {
            IsEditClientItemOpen = true;
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


        private Client _NewClientItem { get; set; }
        public Client NewClientItem
        {
            get
            {
                return _NewClientItem;
            }
            set
            {

                _NewClientItem = value;
                RaisePropertyChanged("NewClientItem");
            }
        }

        private bool _IsNewClientItemOpen { get; set; }
        public bool IsNewClientItemOpen
        {
            get
            {
                return _IsNewClientItemOpen;
            }
            set
            {
                _IsNewClientItemOpen = value;
                RaisePropertyChanged("IsNewClientItemOpen");
            }
        }



        private ICommand _AddClientCmd;
        public ICommand AddClientCmd
        {
            get
            {
                if (_AddClientCmd == null)
                    _AddClientCmd = new RunCommand(AddClientItem);
                return _AddClientCmd;
            }
            set
            {
                _AddClientCmd = value;
            }

        }

        private void AddClientItem(object obj)
        {
            try
            {
                //save new Client
                if ((bool)obj)
                {
                    if (NewClientItem != null)
                    {
                        App.DataProvider.Clients.Add(NewClientItem);
                        if (App.DataProvider.SaveChanges()==1)
                        {
                            ClientsList = new ObservableCollection<Client>(App.DataProvider.Clients.AsEnumerable());
                        }
                        else
                        {
                            Console.WriteLine("Error Saving Data to ClientList");
                        }
                        IsNewClientItemOpen = false;
                    }
                }
                // if mode is show new Client layout
                else
                {
                    NewClientItem = new Client();
                    IsNewClientItemOpen = true;
                }
            }
            catch (InvalidCastException e)
            {

                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        //Save updation to Clientitem
        private ICommand _SaveEditsClientCmd;
        public ICommand SaveEditsClientCmd
        {
            get
            {
                if (_SaveEditsClientCmd == null)
                    _SaveEditsClientCmd = new RunCommand(SaveEditsClientItem);
                return _SaveEditsClientCmd;
            }
            set
            {
                _SaveEditsClientCmd = value;
            }

        }

        /*
         * ChildWindowVisibility Handles
         * 
         */
        private bool _IsEditClientItemOpen { get; set; }
        public bool IsEditClientItemOpen
        {
            get
            {
                return _IsEditClientItemOpen;
            }
            set
            {
                _IsEditClientItemOpen = value;
                RaisePropertyChanged("IsEditClientItemOpen");
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

        private void SaveEditsClientItem(object obj)
        {
            if (EditMode)
            {
                App.DataProvider.SaveChanges();
            }
            IsEditClientItemOpen = false;
            
        }

        private ICommand _RmClientCmd;
        public ICommand RmClientCmd
        {
            get
            {
                if (_RmClientCmd == null)
                    _RmClientCmd = new RunCommand(RemoveClientItem);
                return _RmClientCmd;
            }
            set
            {
                _RmClientCmd = value;
            }

        }
        private void RemoveClientItem(object Item)
        {
            try
            {
                ClientsList.Remove((Client)Item);
                App.DataProvider.Clients.Remove((Client)Item);
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


            if (ClientsList!=null && ClientsList.Count == 0)
            {
                IsClientListEmpty = true;
            }
            else
            {
                IsClientListEmpty = false;

            }
        }

        private bool _IsClientListEmpty;
        public bool IsClientListEmpty
        {
            get
            {
                return _IsClientListEmpty;
            }
            set
            {
                _IsClientListEmpty = value;
                RaisePropertyChanged("IsClientListEmpty");
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

        public static Expression<Func<Client,bool>> FindClients( string key, string type)
        {

            //CODE_CLEANUP: the return type should always be a valid Linq expression this makes code readablity sucking


            //show all if empty
            if(key == "" || key == null )
            {
                return (Client => true);
            }

            switch (type)
            {
                case "Name":
                    return (Client=>Client.Name.ToLower().Contains(key.ToLower()));
                case "Address":
                    return (Client=> Client.BillingAddress.ToLower().Contains(key.ToLower()) ||
                                     Client.ShippingAddress.ToLower().Contains(key.ToLower())
                            );
                 case "All":
                    return (Client => Client.BillingAddress.ToLower().Contains(key.ToLower()) || 
                    
                                       Client.Name.ToLower().Contains(key.ToLower()) || 
                                       
                                       Client.ShippingAddress.ToString().ToLower().Contains(key.ToLower()) || 
                                       
                                       Client.Details.ToLower().Contains(key.ToLower())
                          );
                default:
                    return (Client=>true);
            }
        }
        private ICommand _SearchCmd;
        public ICommand SearchCmd
        {
            get
            {
                if (_SearchCmd == null)
                    _SearchCmd = new RunCommand(SearchClient);
                return _SearchCmd;
            }
            set
            {
                _SearchCmd = value;
            }

        }
        private void SearchClient(object Item)
        {
            ClientsList = null;
            //BUG:Exception NotSupportedException: Reason Direct Call to FindClients function is not possible 

            ClientsList = new ObservableCollection<Client>(
                     App.DataProvider.Clients.Where(FindClients(SearchKey,SearchFilter))
                 );
           
        }

    }
}
