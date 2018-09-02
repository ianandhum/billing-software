using Provider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kait.ViewModel.Primitive
{
    public class InvoiceViewModel: Support.NotifyUIBase
    {
        public InvoiceViewModel()
        {
            Invoice = new Invoice();
        }
        public InvoiceViewModel(Invoice invoice)
        {
            Invoice = invoice;
        }
        public int ClientId
        {
            get { return Invoice.ClientId; }
            set {
                Invoice.ClientId = value;
                RaisePropertyChanged("ClientId");
            }
        }

        public DateTime IssueDate
        {
            get { return Invoice.IssueDate; }
            set
            {
                Invoice.IssueDate = value;
                RaisePropertyChanged("IssueDate");
            }
        }
        public  DateTime DueDate
        {
            get { return Invoice.DueDate; }
            set
            {
                Invoice.DueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        public bool IsPaid
        {
            get { return Invoice.IsPaid; }
            set
            {
                Invoice.IsPaid = value;
                RaisePropertyChanged("IsPaid");
            }
        }

        public Decimal SubTotal
        {
            get { return Invoice.SubTotal; }
            set
            {
                Invoice.SubTotal = value;
                RaisePropertyChanged("SubTotal");
            }
        }

        public Decimal TotalTax
        {
            get { return Invoice.TotalTax; }
            set
            {
                Invoice.TotalTax = value;
                RaisePropertyChanged("TotalTax");
            }
        }

        public Decimal Total
        {
            get { return Invoice.Total; }
            set
            {
                Invoice.Total = value;
                RaisePropertyChanged("Total");
            }
        }

        public string InternalNote
        {
            get { return Invoice.InternalNote; }
            set
            {
                Invoice.InternalNote = value;
                RaisePropertyChanged("InternalNote");
            }
        }

        public string InvoiceNote
        {
            get { return Invoice.InvoiceNote; }
            set
            {
                Invoice.InvoiceNote = value;
                RaisePropertyChanged("InvoiceNote");
            }
        }

        public bool Deleted
        {
            get { return Invoice.Deleted; }
            set
            {
                Invoice.Deleted = value;
                RaisePropertyChanged("Deleted");
            }
        }

        public Decimal Discount
        {
            get { return Invoice.Discount; }
            set
            {
                Invoice.Discount = value;
                RaisePropertyChanged("Discount");
            }
        }

        public Decimal RoundedOff
        {
            get { return Invoice.RoundedOff; }
            set
            {
                Invoice.RoundedOff = value;
                RaisePropertyChanged("RoundedOff");
            }
        }

        public Decimal ShippingCharge
        {
            get { return Invoice.ShippingCharge; }
            set
            {
                Invoice.ShippingCharge = value;
                RaisePropertyChanged("ShippingCharge");
            }
        }

        public bool Cancelled
        {
            get { return Invoice.Cancelled; }
            set
            {
                Invoice.Cancelled = value;
                RaisePropertyChanged("Cancelled");
            }
        }

        private Invoice Invoice { get; set; }

        public Invoice GetInvoice() {
            return Invoice;
        }

    }

}
