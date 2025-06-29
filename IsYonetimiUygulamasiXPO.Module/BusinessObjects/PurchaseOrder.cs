using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Satın Alma")]
    [DisplayName("Satınalma Siparişi (XPO)")]
    public class PurchaseOrder : XPObject
    {
        public PurchaseOrder(Session session) : base(session) { }

        private string _orderNumber;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Sipariş Numarası")]
        public string OrderNumber
        {
            get => _orderNumber;
            set => SetPropertyValue(nameof(OrderNumber), ref _orderNumber, value);
        }

        private DateTime _orderDate;
        [XafDisplayName("Sipariş Tarihi")]
        public DateTime OrderDate
        {
            get => _orderDate;
            set => SetPropertyValue(nameof(OrderDate), ref _orderDate, value);
        }

        private Supplier _supplier;
        [Association("Supplier-PurchaseOrders")]
        [XafDisplayName("Tedarikçi")]
        public Supplier Supplier
        {
            get => _supplier;
            set => SetPropertyValue(nameof(Supplier), ref _supplier, value);
        }

        private DateTime _expectedDeliveryDate;
        [XafDisplayName("Beklenen Teslim Tarihi")]
        public DateTime ExpectedDeliveryDate
        {
            get => _expectedDeliveryDate;
            set => SetPropertyValue(nameof(ExpectedDeliveryDate), ref _expectedDeliveryDate, value);
        }

        private decimal _totalAmount;
        [XafDisplayName("Toplam Tutar")]
        public decimal TotalAmount
        {
            get => _totalAmount;
            set => SetPropertyValue(nameof(TotalAmount), ref _totalAmount, value);
        }

        private string _currency;
        [Size(3)]
        [XafDisplayName("Para Birimi")]
        public string Currency
        {
            get => _currency;
            set => SetPropertyValue(nameof(Currency), ref _currency, value);
        }

        private PurchaseOrderStatus _status;
        [XafDisplayName("Sipariş Durumu")]
        public PurchaseOrderStatus Status
        {
            get => _status;
            set => SetPropertyValue(nameof(Status), ref _status, value);
        }

        private string _notes;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Notlar")]
        public string Notes
        {
            get => _notes;
            set => SetPropertyValue(nameof(Notes), ref _notes, value);
        }

        [Association("PurchaseOrder-OrderItems"), Aggregated]
        [XafDisplayName("Sipariş Kalemleri")]
        public XPCollection<PurchaseOrderItem> OrderItems
        {
            get {
                XPCollection<PurchaseOrderItem> items = GetCollection<PurchaseOrderItem>(nameof(OrderItems));
                items.ListChanged += OrderItems_ListChanged;
                return items;
            }
        }

        private void OrderItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            UpdateTotalAmount();
        }

        public void UpdateTotalAmount()
        {
            if (IsLoading || IsSaving) return;
            decimal currentTotal = 0;
            foreach (PurchaseOrderItem item in OrderItems)
            {
                currentTotal += item.LineTotalWithVatCalculated; // Kalemdeki hesaplanmış alanı kullan
            }
            TotalAmount = currentTotal;
        }

        [Association("PurchaseOrder-StockTransactions")]
        [XafDisplayName("İlişkili Stok Hareketleri")]
        public XPCollection<StockTransaction> StockTransactions => GetCollection<StockTransaction>(nameof(StockTransactions));

        [Association("PurchaseOrder-Invoices")]
        [XafDisplayName("İlişkili Alış Faturaları")]
        public XPCollection<Invoice> Invoices => GetCollection<Invoice>(nameof(Invoices));


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            OrderDate = DateTime.Now;
            ExpectedDeliveryDate = DateTime.Now.AddDays(7);
            Currency = "TRY";
            Status = PurchaseOrderStatus.Draft;
        }

        protected override void OnSaving()
        {
            UpdateTotalAmount(); // Kaydetmeden önce son bir kez toplamı güncelle
            base.OnSaving();
        }
    }

    public enum PurchaseOrderStatus {
        [Display(Name="Taslak")] Draft=0,
        [Display(Name="Onay Bekliyor")] PendingApproval=1,
        [Display(Name="Onaylandı")] Approved = 2,
        [Display(Name="Sipariş Verildi")] Ordered = 3,
        [Display(Name="Kısmen Teslim Alındı")] PartiallyReceived = 4,
        [Display(Name="Tamamı Teslim Alındı")] FullyReceived=5,
        [Display(Name="İptal Edildi")] Cancelled=6
    }
}
