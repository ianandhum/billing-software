using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// TextBox with Label
    /// </summary>
    public partial class TextInputBox : UserControl
    {
        public TextInputBox()
        {
            InitializeComponent();
            Text = "";
            LabelContent = "TextBox Heading";
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
                        DependencyProperty.Register(
                        "Text", typeof(string),
                        typeof(TextInputBox)
         );


        public string LabelContent
        {
            get { return (string)GetValue(labelContentProperty); }
            set { SetValue(labelContentProperty, value); }
        }
        public static readonly DependencyProperty labelContentProperty =
                        DependencyProperty.Register(
                        "LabelContent", typeof(string),
                        typeof(TextInputBox)
         );
        



    }
}
