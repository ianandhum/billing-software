using System;
using Provider.Data;
using Kait.Support;

namespace Kait.ViewModel.Primitive
{
    public class PurchaseProductsViewModel:NotifyUIBase
    {
        public PurchaseProductsViewModel()
        {
            PurchaseProducts = new PurchaseProducts();
        }
        public PurchaseProductsViewModel(PurchaseProducts purchaseProducts)
        {
            PurchaseProducts = purchaseProducts;
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
                return PurchaseProducts.ProductId;
            }
            set
            {
                PurchaseProducts.ProductId = value;
                RaisePropertyChanged("ProductId");
            }
        }

        public String Name
        {
            get
            {
                return PurchaseProducts.Name;
            }
            set
            {
                PurchaseProducts.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public String Description
        {
            get
            {
                return PurchaseProducts.Description;
            }
            set
            {
                PurchaseProducts.Description = value;
                RaisePropertyChanged("Description");
            }
        }
        public Measure MU
        {
            get
            {
                return PurchaseProducts.MU;
            }
            set
            {
                PurchaseProducts.MU = value;
                RaisePropertyChanged("MU");
            }
        }
        public decimal Price
        {
            get
            {
                return PurchaseProducts.Price;
            }
            set
            {
                PurchaseProducts.Price = value;
                RaisePropertyChanged("Price");
            }
        }

        public decimal Quantity
        {
            get
            {
                return PurchaseProducts.Quantity;
            }
            set
            {
                PurchaseProducts.Quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }
        public bool InclusiveTax
        {
            get
            {
                return PurchaseProducts.InclusiveTax;
            }
            set
            {
                PurchaseProducts.InclusiveTax = value;
                RaisePropertyChanged("InclusiveTax");
            }
        }
        
        public decimal TotalNoTax
        {
            get
            {
                return PurchaseProducts.TotalNoTax;
            }
            set
            {
                PurchaseProducts.TotalNoTax = value;
                RaisePropertyChanged("TotalNoTax");
            }
        }
        public decimal TotalTax
        {
            get
            {
                return PurchaseProducts.TotalTax;
            }
            set
            {
                PurchaseProducts.TotalTax = value;
                RaisePropertyChanged("TotalTax");
            }
        }
        public decimal Total
        {
            get
            {
                return PurchaseProducts.Total;
            }
            set
            {
                PurchaseProducts.Total = value;
                RaisePropertyChanged("Total");
            }
        }
        public bool IsDiscount
        {
            get
            {
                return PurchaseProducts.IsDiscount;
            }
            set
            {
                PurchaseProducts.IsDiscount = value;
                RaisePropertyChanged("IsDiscount");
            }
        }
        public Decimal DiscountPercent
        {
            get
            {
                return PurchaseProducts.DiscountPercent;
            }
            set
            {
                PurchaseProducts.DiscountPercent = value;
                RaisePropertyChanged("DiscountPercent");
            }
        }
        public String HSN
        {
            get
            {
                return PurchaseProducts.HSN;
            }
            set
            {
                PurchaseProducts.HSN = value;
                RaisePropertyChanged("HSN");
            }
        }
        public decimal CESSPercent
        {
            get
            {
                return PurchaseProducts.CESSPercent;
            }
            set
            {
                PurchaseProducts.CESSPercent = value;
                RaisePropertyChanged("CESSPercent");
            }

        }
        public Taxes Tax
        {
            get
            {
                return PurchaseProducts.Tax;
            }
            set
            {
                PurchaseProducts.Tax = value;
                RaisePropertyChanged("Tax");
            }

        }
        private PurchaseProducts PurchaseProducts { get; set; }

        public PurchaseProducts GetPurchaseProducts()
        {
            return PurchaseProducts;
        }
    }
}
