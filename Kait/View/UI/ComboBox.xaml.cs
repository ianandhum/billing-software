using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace View.UI
{
    /// <summary>
    /// ComboBox With Label 
    /// </summary>

    public partial class ComboBox : UserControl
    {
        public ComboBox()
        {
            InitializeComponent();
            ItemSource = new List<object>();
            
            
        }
      

        public string LabelContent
        {
            get { return (string)GetValue(labelContentProperty); }
            set { SetValue(labelContentProperty, value); }
        }
        public static readonly DependencyProperty labelContentProperty =
                        DependencyProperty.Register(
                        "LabelContent", typeof(string),
                        typeof(ComboBox)
         );





        public IEnumerable<object> ItemSource
        {
            get { return (IEnumerable<object>)GetValue(ItemSourceProperty); }
            set {

                SetValue(ItemSourceProperty, value);
                

            }
        }
        public static readonly DependencyProperty ItemSourceProperty =
                        DependencyProperty.Register(
                        "ItemSource", typeof(IEnumerable<object>),
                        typeof(ComboBox)
         );


        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedIndexProperty =
                        DependencyProperty.Register(
                        "SelectedIndex", typeof(int),
                        typeof(ComboBox)
         );


        public object SelectedItem
        {
            get { return (int)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
                        DependencyProperty.Register(
                        "SelectedItem", typeof(object),
                        typeof(ComboBox)
         );




    }


}
