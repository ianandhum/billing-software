using System;
using Provider.Data;
using Kait.Support;

namespace Kait.ViewModel.Primitive
{
    public class InvoiceProductsViewModel:NotifyUIBase
    {
        public InvoiceProductsViewModel()
        {
            InvoiceProducts = new InvoiceProducts();
        }
        public InvoiceProductsViewModel(InvoiceProducts invoiceProducts)
        {
            InvoiceProducts = invoiceProducts;
        }
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

        public int ProductId
        {
            get
            {
                return InvoiceProducts.ProductId;
            }
            set
            {
                InvoiceProducts.ProductId = value;
                RaisePropertyChanged("ProductId");
            }
        }

        public String Name
        {
            get
            {
                return InvoiceProducts.Name;
            }
            set
            {
                InvoiceProducts.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public String Description
        {
            get
            {
                return InvoiceProducts.Description;
            }
            set
            {
                InvoiceProducts.Description = value;
                RaisePropertyChanged("Description");
            }
        }
        public Measure MU
        {
            get
            {
                return InvoiceProducts.MU;
            }
            set
            {
                InvoiceProducts.MU = value;
                RaisePropertyChanged("MU");
            }
        }
        public decimal Price
        {
            get
            {
                return InvoiceProducts.Price;
            }
            set
            {
                InvoiceProducts.Price = value;
                RaisePropertyChanged("Price");
            }
        }

        public decimal Quantity
        {
            get
            {
                return InvoiceProducts.Quantity;
            }
            set
            {
                InvoiceProducts.Quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }
        public bool InclusiveTax
        {
            get
            {
                return InvoiceProducts.InclusiveTax;
            }
            set
            {
                InvoiceProducts.InclusiveTax = value;
                RaisePropertyChanged("InclusiveTax");
            }
        }
        
        public decimal TotalNoTax
        {
            get
            {
                return InvoiceProducts.TotalNoTax;
            }
            set
            {
                InvoiceProducts.TotalNoTax = value;
                RaisePropertyChanged("TotalNoTax");
            }
        }
        public decimal TotalTax
        {
            get
            {
                return InvoiceProducts.TotalTax;
            }
            set
            {
                InvoiceProducts.TotalTax = value;
                RaisePropertyChanged("TotalTax");
            }
        }
        public decimal Total
        {
            get
            {
                return InvoiceProducts.Total;
            }
            set
            {
                InvoiceProducts.Total = value;
                RaisePropertyChanged("Total");
            }
        }
        public bool IsDiscount
        {
            get
            {
                return InvoiceProducts.IsDiscount;
            }
            set
            {
                InvoiceProducts.IsDiscount = value;
                RaisePropertyChanged("IsDiscount");
            }
        }
        public Decimal DiscountPercent
        {
            get
            {
                return InvoiceProducts.DiscountPercent;
            }
            set
            {
                InvoiceProducts.DiscountPercent = value;
                RaisePropertyChanged("DiscountPercent");
            }
        }
        public String HSN
        {
            get
            {
                return InvoiceProducts.HSN;
            }
            set
            {
                InvoiceProducts.HSN = value;
                RaisePropertyChanged("HSN");
            }
        }
        public decimal CESSPercent
        {
            get
            {
                return InvoiceProducts.CESSPercent;
            }
            set
            {
                InvoiceProducts.CESSPercent = value;
                RaisePropertyChanged("CESSPercent");
            }

        }
        public Taxes Tax
        {
            get
            {
                return InvoiceProducts.Tax;
            }
            set
            {
                InvoiceProducts.Tax = value;
                RaisePropertyChanged("Tax");
            }

        }
        private InvoiceProducts InvoiceProducts { get; set; }

        public InvoiceProducts GetInvoiceProducts()
        {
            return InvoiceProducts;
        }
    }
}
