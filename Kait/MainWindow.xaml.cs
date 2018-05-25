using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kait.View.Pages;

namespace Kait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Data();
            PART_FrameService = PART_Frame.NavigationService;
            PART_FrameService.Navigate(new Home());
           
                
        }
        public static NavigationService PART_FrameService { get; set; }
        private  class Data:INotifyPropertyChanged
        {
            public Data()
            {
                Item = new List<string>();
                Item.Add("Hell");
                Item.Add("show ");
                Item.Add("colleage");
                Item.Add("clash");
                Item.Add("clan");
                Item.Add("clogue");
                ItemText = "Hell";
                slIndex = 0;
                slItem = Item.ElementAt(0);
            }

            private string _ItemText;
            public string ItemText {
                get { return _ItemText; }
                set
                {
                    _ItemText = value;
                    NotifyChange();
                }
            }
            private List<String> _Items;

            public event PropertyChangedEventHandler PropertyChanged;
            public void NotifyChange([CallerMemberName] string property="")
            {
                if (PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
            }

            public List<String> Item
            {
                get { return _Items; }
                set
                {
                    this._Items = value;
                    NotifyChange();
                }
            }
            private String _slItem;
            public String slItem
            {
                get { return _slItem; }
                set
                {
                    this._slItem = value;
                    NotifyChange();
                }
            }
            private int _slIndex;
            public int slIndex
            {
                get { return _slIndex; }
                set
                {
                    this._slIndex = value;
                    NotifyChange();
                }
            }

        }

        private void NavigationButton_Click(object sender, EventArgs e)
        {
            PART_FrameService.Navigate(new NewInvoice());
        }
    }

}