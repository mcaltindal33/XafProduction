using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
using System; // Convert için

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Fatura Kalemi (XPO)")]
    public class InvoiceItem : XPObject
    {
        public InvoiceItem(Session session) : base(session) { }

        private Invoice _invoice;
        [Association("Invoice-InvoiceItems")]
        [Browsable(false)]
        public Invoice Invoice { get => _invoice; set => SetPropertyValue(nameof(Invoice), ref _invoice, value); }

        private Material _material;
        [Association("Material-InvoiceItems")]
        [XafDisplayName("Malzeme/Hizmet")]
        public Material Material {
            get => _material;
            set {
                if(SetPropertyValue(nameof(Material), ref _material, value))
                {
                    if(!IsLoading && Material != null)
                    {
                        Description = Material.MaterialName;
                        Unit = Material.BaseUnit;
                        UnitPrice = Material.UnitPrice; // Malzemenin satış fiyatını çekebilir
                    }
                }
            }
        }

        private string _description;
        [Size(255)]
        [XafDisplayName("Açıklama")]
        public string Description { get => _description; set => SetPropertyValue(nameof(Description), ref _description, value); }

        private double _quantity;
        [XafDisplayName("Miktar")]
        public double Quantity { get => _quantity; set => SetPropertyValue(nameof(Quantity), ref _quantity, value); }

        private string _unit;
        [Size(50)]
        [XafDisplayName("Birim")]
        public string Unit { get => _unit; set => SetPropertyValue(nameof(Unit), ref _unit, value); }

        private decimal _unitPrice;
        [XafDisplayName("Birim Fiyat")]
        public decimal UnitPrice { get => _unitPrice; set => SetPropertyValue(nameof(UnitPrice), ref _unitPrice, value); }

        private decimal _discountRate; // 0-100 arası
        [XafDisplayName("İndirim Oranı (%)")]
        public decimal DiscountRate { get => _discountRate; set => SetPropertyValue(nameof(DiscountRate), ref _discountRate, value); }

        private decimal _vatRate; // 0-100 arası
        [XafDisplayName("KDV Oranı (%)")]
        public decimal VatRate { get => _vatRate; set => SetPropertyValue(nameof(VatRate), ref _vatRate, value); }

        // Bu alan Invoice'daki PersistentAlias'lar tarafından kullanılacak.
        [NonPersistent] // Veritabanında tutulmayacak, anlık hesaplanacak.
        [XafDisplayName("Satır Tutarı (KDV'siz, İndirimli)")]
        public decimal LineAmountCalculated
        {
            get
            {
                return (decimal)Quantity * UnitPrice * (1 - DiscountRate / 100);
            }
        }

        [NonPersistent]
        [XafDisplayName("Satır KDV Tutarı")]
        public decimal LineVatAmountCalculated
        {
            get
            {
                return LineAmountCalculated * (VatRate / 100);
            }
        }

        [NonPersistent]
        [XafDisplayName("Satır Toplamı (KDV Dahil)")]
        public decimal LineTotalWithVatCalculated
        {
            get
            {
                return LineAmountCalculated + LineVatAmountCalculated;
            }
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            Unit = "Adet";
            VatRate = 18;
            Quantity = 1;
        }

        // Fatura toplamlarının güncellenmesi için InvoiceItem'daki değişikliklerde
        // Invoice nesnesinin OnChanged'ını tetiklemek gerekebilir (eğer PersistentAlias kullanılmıyorsa).
        // Biz Invoice'da PersistentAlias kullandığımız için bu genellikle otomatik olur.
        // Ancak, bir koleksiyon değişikliğinde (Item Ekleme/Silme) Invoice'un haberdar olması için
        // Invoice.InvoiceItems getter'ında ListChanged olayına abone olunabilir.
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving && Invoice != null)
            {
                // Eğer Invoice'daki toplamlar PersistentAlias ile değil de manuel hesaplanıyorsa:
                // Invoice.UpdateTotals();
                // Ama PersistentAlias kullandığımız için bu satıra gerek yok.
                // Sadece Invoice nesnesinin kendisinin değiştiğini bildirmek yeterli olabilir (UI güncellemesi için)
                 if (propertyName == nameof(Quantity) || propertyName == nameof(UnitPrice) ||
                     propertyName == nameof(DiscountRate) || propertyName == nameof(VatRate))
                 {
                    Invoice.OnChanged(nameof(Invoice.SubTotalAfterDiscount));
                    Invoice.OnChanged(nameof(Invoice.TotalDiscountAmount));
                    Invoice.OnChanged(nameof(Invoice.TotalVat));
                    Invoice.OnChanged(nameof(Invoice.GrandTotal));
                 }
            }
        }
    }
}
