using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Finans Yönetimi")]
    [DisplayName("Fatura (XPO)")]
    public class Invoice : XPObject
    {
        public Invoice(Session session) : base(session) { }

        private string _invoiceNumber;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Fatura Numarası")]
        public string InvoiceNumber { get => _invoiceNumber; set => SetPropertyValue(nameof(InvoiceNumber), ref _invoiceNumber, value); }

        private DateTime _invoiceDate;
        [XafDisplayName("Fatura Tarihi")]
        public DateTime InvoiceDate { get => _invoiceDate; set => SetPropertyValue(nameof(InvoiceDate), ref _invoiceDate, value); }

        private Customer _customer; // Satış faturası ise
        [Association("Customer-Invoices")]
        [XafDisplayName("Müşteri")]
        public Customer Customer { get => _customer; set => SetPropertyValue(nameof(Customer), ref _customer, value); }

        private Supplier _supplier; // Alış faturası ise
        [Association("Supplier-Invoices")]
        [XafDisplayName("Tedarikçi")]
        public Supplier Supplier { get => _supplier; set => SetPropertyValue(nameof(Supplier), ref _supplier, value); }

        private DateTime _dueDate;
        [XafDisplayName("Vade Tarihi")]
        public DateTime DueDate { get => _dueDate; set => SetPropertyValue(nameof(DueDate), ref _dueDate, value); }

        // InvoiceItems koleksiyonu üzerinden PersistentAlias ile hesaplamalar
        [PersistentAlias("InvoiceItems.Sum(LineAmountCalculated)")] // LineAmountCalculated InvoiceItem'da tanımlı
        [XafDisplayName("Ara Toplam (İndirimli, KDV'siz)")]
        public decimal SubTotalAfterDiscount => Convert.ToDecimal(EvaluateAlias(nameof(SubTotalAfterDiscount)));

        [PersistentAlias("InvoiceItems.Sum(UnitPrice * Quantity * (DiscountRate/100))")] // Her bir kalemdeki indirim tutarı
        [XafDisplayName("Toplam İndirim Tutarı")]
        public decimal TotalDiscountAmount => Convert.ToDecimal(EvaluateAlias(nameof(TotalDiscountAmount)));

        [PersistentAlias("InvoiceItems.Sum(LineVatAmountCalculated)")] // LineVatAmountCalculated InvoiceItem'da tanımlı
        [XafDisplayName("Toplam KDV")]
        public decimal TotalVat => Convert.ToDecimal(EvaluateAlias(nameof(TotalVat)));

        [PersistentAlias("InvoiceItems.Sum(LineTotalWithVatCalculated)")] // LineTotalWithVatCalculated InvoiceItem'da tanımlı
        [XafDisplayName("Genel Toplam (KDV Dahil)")]
        public decimal GrandTotal => Convert.ToDecimal(EvaluateAlias(nameof(GrandTotal)));

        private string _currency;
        [Size(3)]
        [XafDisplayName("Para Birimi")]
        public string Currency { get => _currency; set => SetPropertyValue(nameof(Currency), ref _currency, value); }

        private InvoiceType _type;
        [XafDisplayName("Fatura Tipi")]
        public InvoiceType Type { get => _type; set => SetPropertyValue(nameof(Type), ref _type, value); }

        private PaymentStatus _paymentStatus;
        [XafDisplayName("Ödeme Durumu")]
        public PaymentStatus PaymentStatus { get => _paymentStatus; set => SetPropertyValue(nameof(PaymentStatus), ref _paymentStatus, value); }

        private string _notes;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Notlar")]
        public string Notes { get => _notes; set => SetPropertyValue(nameof(Notes), ref _notes, value); }

        [Association("Invoice-InvoiceItems"), Aggregated]
        [XafDisplayName("Fatura Kalemleri")]
        public XPCollection<InvoiceItem> InvoiceItems => GetCollection<InvoiceItem>(nameof(InvoiceItems));

        private Shipment _relatedShipment; // Satış Faturası için
        [Association("Shipment-Invoices")]
        [XafDisplayName("İlişkili Sevkiyat")]
        public Shipment RelatedShipment { get => _relatedShipment; set => SetPropertyValue(nameof(RelatedShipment), ref _relatedShipment, value); }

        private PurchaseOrder _relatedPurchaseOrder; // Alış Faturası için
        [Association("PurchaseOrder-Invoices")]
        [XafDisplayName("İlişkili Satınalma Siparişi")]
        public PurchaseOrder RelatedPurchaseOrder { get => _relatedPurchaseOrder; set => SetPropertyValue(nameof(RelatedPurchaseOrder), ref _relatedPurchaseOrder, value); }

        public override void AfterConstruction() {
            base.AfterConstruction();
            InvoiceDate = DateTime.Now;
            DueDate = DateTime.Now.AddDays(30);
            Currency = "TRY";
            Type = InvoiceType.Sales;
            PaymentStatus = PaymentStatus.Unpaid;
        }
    }

    public enum InvoiceType {
        [Display(Name="Satış Faturası")] Sales,
        [Display(Name="Alış Faturası")] Purchase,
        [Display(Name="Satış İade Faturası")] SalesReturn,
        [Display(Name="Alış İade Faturası")] PurchaseReturn
    }
    public enum PaymentStatus {
        [Display(Name="Ödenmedi")] Unpaid,
        [Display(Name="Kısmen Ödendi")] PartiallyPaid,
        [Display(Name="Ödendi")] Paid,
        [Display(Name="Vadesi Geçti")] Overdue
    }
}
