using Kait.ViewModel.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kait.BusinessLogic
{
    public abstract class ComputeInvoice
    {
        
        //Invoice Fields
        //Total
        //TotalNoTax
        //TotalTax
        //ShippingCharge
        //Discount-- If DiscounAll is enabled
        //RoundedOff
        //InvoiceProducts
        //InvoicePayments
        //Client Resolution
        private Decimal Total;
        private Decimal TotalTax;
        private Decimal RoundedOff;
        private Decimal TaxRate;
        public void AddToTotal(Decimal value)
        {
            Total += value;

        }
        public void DeductFromTotal(Decimal value)
        {
            Total -= value;
        }
        public void SetTaxRate(Decimal rate)
        {
            TaxRate = rate;
        }
        public Decimal GetTotal()
        {
            return Total;
        }
        public Decimal GetTax()
        {
            TotalTax = Total * TaxRate / 100;
            return TotalTax;
        }
        public Decimal GetRoundedOffAmt()
        {
            RoundedOff = Total - (Decimal.Floor(Total));
            return RoundedOff;
        }
        
    }
}
