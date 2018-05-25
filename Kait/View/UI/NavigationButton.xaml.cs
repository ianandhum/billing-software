using System;
using System.Collections.Generic;
using System.Linq;
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

namespace View.UI
{
    /// <summary>
    /// Navigation Button with label
    /// </summary>
    public partial class NavigationButton : UserControl
    {
        public NavigationButton()
        {
            InitializeComponent();
        }
        public string BottomText
        {
            get { return (string)contentLabel.Content; }
            set { contentLabel.Content = value; }
        }
        public Visibility ContentVisible
        {
            get { return contentLabel.Visibility; }
            set { contentLabel.Visibility = value; }
        }
        public ImageSource IconImage
        {
            get { return imgBtnMain.Source; }
            set { imgBtnMain.Source = value; }
        }
        public event EventHandler<EventArgs> Click;

        private void ButtonImage_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, null);
        }
    }
}
