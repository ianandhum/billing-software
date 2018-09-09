using Provider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kait.ViewModel.Primitive
{
    public class PurchaseViewModel: Support.NotifyUIBase
    {
        public PurchaseViewModel()
        {
            Purchase = new Purchase();
        }
        public PurchaseViewModel(Purchase purchase)
        {
            Purchase = purchase;

        }
        public int PurchaseId
        {
            get { return Purchase.PurchaseId; }
            set
            {
                Purchase.PurchaseId = value;
                RaisePropertyChanged("PurchaseId");
            }
        }
        public int VendorId
        {
            get { return Purchase.VendorId; }
            set {
                Purchase.VendorId = value;
                RaisePropertyChanged("VendorId");
            }
        }

        public DateTime IssueDate
        {
            get { return Purchase.IssueDate; }
            set
            {
                Purchase.IssueDate = value;
                RaisePropertyChanged("IssueDate");
            }
        }
        
        public bool IsPaid
        {
            get { return Purchase.IsPaid; }
            set
            {
                Purchase.IsPaid = value;
                RaisePropertyChanged("IsPaid");
            }
        }

        public Decimal SubTotal
        {
            get { return Purchase.SubTotal; }
            set
            {
                Purchase.SubTotal = value;
                RaisePropertyChanged("SubTotal");
            }
        }

        public Decimal TotalTax
        {
            get { return Purchase.TotalTax; }
            set
            {
                Purchase.TotalTax = value;
                RaisePropertyChanged("TotalTax");
            }
        }

        public Decimal Total
        {
            get { return Purchase.Total; }
            set
            {
                Purchase.Total = value;
                RaisePropertyChanged("Total");
            }
        }

        public string InternalNote
        {
            get { return Purchase.InternalNote; }
            set
            {
                Purchase.InternalNote = value;
                RaisePropertyChanged("InternalNote");
            }
        }

        public string PurchaseNote
        {
            get { return Purchase.PurchaseNote; }
            set
            {
                Purchase.PurchaseNote = value;
                RaisePropertyChanged("PurchaseNote");
            }
        }

        public bool Deleted
        {
            get { return Purchase.Deleted; }
            set
            {
                Purchase.Deleted = value;
                RaisePropertyChanged("Deleted");
            }
        }

        public Decimal Discount
        {
            get { return Purchase.Discount; }
            set
            {
                Purchase.Discount = value;
                RaisePropertyChanged("Discount");
            }
        }

        public Decimal RoundedOff
        {
            get { return Purchase.RoundedOff; }
            set
            {
                Purchase.RoundedOff = value;
                RaisePropertyChanged("RoundedOff");
            }
        }

        
        public bool Cancelled
        {
            get { return Purchase.Cancelled; }
            set
            {
                Purchase.Cancelled = value;
                RaisePropertyChanged("Cancelled");
            }
        }

        private Purchase Purchase { get; set; }

        public Purchase GetPurchase() {
            return Purchase;
        }

    }

}
