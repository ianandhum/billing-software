using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Support;
using Provider.Data;
namespace Kait.ViewModel
{
    public class AddedInvoiceProductsVM:NotifyUIBase
    {
        private int _SlNo { get; set; }
        public int SlNo
        {
            get
            {
                return _SlNo;
            }
            set
            {
                _SlNo =value;
                RaisePropertyChanged("SlNo");
                
            }
        }
        public InvoiceProducts InvoiceProducts { get; set; }
    }
}
