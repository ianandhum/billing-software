using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Provider.Data;
namespace Kait
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {



        public static DataContext DataProvider { get; set; }

        public static Dictionary<string, string> Settings;
        public void InitailizeDataContext(object sender, StartupEventArgs e)
        {
            Thread StartAsyncEF6Migration = new Thread(CreateContext);
            StartAsyncEF6Migration.Start();
        }
        public void CreateContext()
        {
            Dispatcher.BeginInvoke(new Action(ActionCreateContext));
        }
        
        public  void  ActionCreateContext()
        {
            DataProvider = new DataContext();
            /* TODO
             *  An App level settings config.
             *   convert it into a lookup or dictionary for easy get set interface
             */
            Settings=DataProvider.Settings.ToDictionary(t => t.Key, t => t.Value);
            // try with sample
            Console.WriteLine(Settings["Roundoff"]);
        }
        



    }
}
