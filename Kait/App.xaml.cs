using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
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
            //Log console ouput to a file
            logOutputToFile();
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

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message + "\n Application will be terminated", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine(e.Exception.Message);
            Console.WriteLine(e.Exception.StackTrace);
            e.Handled = true;
            MainWindow.Close();

        }

        // Save all console out to file
        // Forked from @JaniekBuysrogge's answer at SO

        private void logOutputToFile()
        {
            FileStream logFileStream = new FileStream("logs/genLog.txt", FileMode.Append);
            var logstreamwriter = new StreamWriter(logFileStream);
            FileStream errFileStream = new FileStream("logs/errLog.txt", FileMode.Append);
            var errstreamwriter = new StreamWriter(errFileStream);
            logstreamwriter.AutoFlush = true;
            Console.SetOut(logstreamwriter);
            Console.SetError(errstreamwriter);
        }
    }
}
