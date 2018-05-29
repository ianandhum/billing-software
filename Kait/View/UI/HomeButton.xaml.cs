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
    /// Interaction logic for HomeButton.xaml
    /// </summary>
    public partial class HomeButton : UserControl
    {
        public HomeButton()
        {
            InitializeComponent();
        }
        public string Heading
        {
            get { return (string)GetValue(HeadingProperty); }
            set { SetValue(HeadingProperty, value); }
        }
        public static readonly DependencyProperty HeadingProperty =
                        DependencyProperty.Register(
                        "Heading", typeof(string),
                        typeof(HomeButton)
         );

        public string SubHeading
        {
            get { return (string)GetValue(SubHeadingProperty); }
            set { SetValue(SubHeadingProperty, value); }
        }
        public static readonly DependencyProperty SubHeadingProperty =
                        DependencyProperty.Register(
                        "SubHeading", typeof(string),
                        typeof(HomeButton)
         );
        public ImageSource Source
        {
            get { return Img.Source; }
            set { Img.Source = value; }
        }
        public event EventHandler<EventArgs> Handler;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Handler?.Invoke(this, e);
        }
    }
}
