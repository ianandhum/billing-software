using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Kait.View.Pages;
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
           
        }
        public static int SetConfig(string key,string value,bool saveInstant=false)
        {

            var settingKey=DataProvider.Settings.SingleOrDefault(v => v.Key == key);
            if (settingKey != default(Settings))
            {
                settingKey.Value = value;
                Settings[key] = value;
            }
            return (saveInstant)?DataProvider.SaveChanges():0;
            
        }
        public static string GetConfig(string key)
        {

            string value;
            if (!Settings.TryGetValue(key, out value))
            {
                return null;
            }
            return value;
        }


    }
}
