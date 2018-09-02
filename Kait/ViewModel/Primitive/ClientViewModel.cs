using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Provider.Data;
namespace Kait.ViewModel
{
    public class ClientViewModel
    {
        public bool DefaultItem { get; set; } 
        public object Client { get; set; }
        public ClientViewModel()
        {
            Client = new Client();
            DefaultItem = false;
        }
        public ClientViewModel(bool type)
        {
            
            DefaultItem = type;
            if (DefaultItem)
            {
                Client = new DefaultItemCL();
            }
        }
        private class DefaultItemCL
        {
            public string Name { get; set; }
            public DefaultItemCL()
            {
                Name = "<Add New>";
            }
        }
    }
}
