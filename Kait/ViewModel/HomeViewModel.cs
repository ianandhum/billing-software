using Kait.Support;
using Provider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kait.ViewModel
{
    public class HomeViewModel:NotifyUIBase
    {

        public HomeViewModel()
        {
            Meta = new MetaData(App.DataProvider);
        }

        public  MetaData Meta { get; set; }
    }
}
