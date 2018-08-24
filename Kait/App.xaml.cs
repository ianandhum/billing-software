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

        public void InitailizeDataContext(object sender, StartupEventArgs e)
        {
            Thread StartAsyncEF6Migration = new Thread(CreateContext);
            StartAsyncEF6Migration.Start();
        }
        public void CreateContext()
        {
            Dispatcher.BeginInvoke(new Action(ActionCreateContext));
        }
        public void ActionCreateContext()
        {
            DataProvider = new DataContext();

            //mockup queryies for startup Migration
            var Result = DataProvider.Products.Count();


        }




    }
}
