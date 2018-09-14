using Kait.Support;
using Provider.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Kait.ViewModel
{
    public class ProductsViewModel:NotifyUIBase
    {
        public ProductsViewModel()
        {
            ProductsList = new ObservableCollection<Product>(App.DataProvider.Products.AsEnumerable());
            ProductsList.CollectionChanged +=TrackChange;
            CurrentDate = DateTime.Today;
            Taxes = App.DataProvider.Taxes.ToList();
            MeasureTypes = Enum.GetValues(typeof(Measure)).Cast<Measure>();
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

        private ObservableCollection<Product> _ProductsList;

        public ObservableCollection<Product> ProductsList
        {
            get {
                return _ProductsList;
            }
            set {
                _ProductsList = value;
                TrackChange(this, null);
                RaisePropertyChanged("ProductsList");
            }
        }

        private Product _SelectedProductItem { get; set; }
        public Product SelectedProductItem
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


        private Product _NewProductItem { get; set; }
        public Product NewProductItem
        {
            get
            {
                return _NewProductItem;
            }
            set
            {

                _NewProductItem = value;
                RaisePropertyChanged("NewProductItem");
            }
        }

        private bool _IsNewProductItemOpen { get; set; }
        public bool IsNewProductItemOpen
        {
            get
            {
                return _IsNewProductItemOpen;
            }
            set
            {
                _IsNewProductItemOpen = value;
                RaisePropertyChanged("IsNewProductItemOpen");
            }
        }



        private ICommand _AddProductCmd;
        public ICommand AddProductCmd
        {
            get
            {
                if (_AddProductCmd == null)
                    _AddProductCmd = new RunCommand(AddProductItem);
                return _AddProductCmd;
            }
            set
            {
                _AddProductCmd = value;
            }

        }

        private void AddProductItem(object obj)
        {
            try
            {
                //save new product
                if ((bool)obj)
                {
                    if (NewProductItem != null)
                    {
                        App.DataProvider.Products.Add(NewProductItem);
                        if (App.DataProvider.SaveChanges()==1)
                        {
                            ProductsList = new ObservableCollection<Product>(App.DataProvider.Products.AsEnumerable());
                        }
                        else
                        {
                            Console.WriteLine("Error Saving Data to ProductList");
                        }
                        IsNewProductItemOpen = false;
                    }
                }
                // if mode is show new product layout
                else
                {
                    NewProductItem = new Product();
                    IsNewProductItemOpen = true;
                }
            }
            catch (InvalidCastException e)
            {

                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
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

        private void SaveEditsProductItem(object obj)
        {
            if (EditMode)
            {
                App.DataProvider.SaveChanges();
            }
            IsEditProductItemOpen = false;
            
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
                ProductsList.Remove((Product)Item);
                App.DataProvider.Products.Remove((Product)Item);
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
            if (ProductsList.Count == 0)
            {
                IsProductListEmpty = true;
            }
            else
            {
                IsProductListEmpty = false;

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

        private string _PrimaryElementContent;

        public string PrimaryElementContent
        {
            get { return _PrimaryElementContent; }
            set {
                _PrimaryElementContent = value;
                RaisePropertyChanged("PrimaryElementContent");
            }
        }

    }
}
