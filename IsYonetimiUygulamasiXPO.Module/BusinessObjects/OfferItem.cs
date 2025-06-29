using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Teklif Kalemi (XPO)")]
    public class OfferItem : XPObject
    {
        public OfferItem(Session session) : base(session) { }

        private Offer _offer;
        [Association("Offer-OfferItems")]
        [Browsable(false)]
        public Offer Offer
        {
            get => _offer;
            // OfferItems koleksiyonu Aggregated olduğu için Offer set edildiğinde
            // bu item Offer'ın OfferItems koleksiyonuna otomatik eklenir/çıkarılır.
            set => SetPropertyValue(nameof(Offer), ref _offer, value);
        }

        private string _productName;
        [Size(255)]
        [XafDisplayName("Ürün/Hizmet Adı")]
        public string ProductName
        {
            get => _productName;
            set => SetPropertyValue(nameof(ProductName), ref _productName, value);
        }

        private string _description;
        [XafDisplayName("Açıklama")]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        private double _quantity;
        [XafDisplayName("Miktar")]
        public double Quantity
        {
            get => _quantity;
            set {
                if(SetPropertyValue(nameof(Quantity), ref _quantity, value)) {
                    // PersistentAlias kullanmadığımız için ana nesneyi bilgilendirmeliyiz
                    if (!IsLoading && Offer != null) Offer.UpdateTotalAmount();
                }
            }
        }

        private string _unit;
        [Size(50)]
        [XafDisplayName("Birim")]
        public string Unit
        {
            get => _unit;
            set => SetPropertyValue(nameof(Unit), ref _unit, value);
        }

        private decimal _unitPrice;
        [XafDisplayName("Birim Fiyat")]
        public decimal UnitPrice
        {
            get => _unitPrice;
            set {
                if(SetPropertyValue(nameof(UnitPrice), ref _unitPrice, value)) {
                    if (!IsLoading && Offer != null) Offer.UpdateTotalAmount();
                }
            }
        }

        private decimal _discountRate;
        [XafDisplayName("İndirim Oranı (%)")]
        public decimal DiscountRate
        {
            get => _discountRate;
            set {
                if(SetPropertyValue(nameof(DiscountRate), ref _discountRate, value)) {
                    if (!IsLoading && Offer != null) Offer.UpdateTotalAmount();
                }
            }
        }

        private decimal _vatRate;
        [XafDisplayName("KDV Oranı (%)")]
        public decimal VatRate
        {
            get => _vatRate;
            set {
                if(SetPropertyValue(nameof(VatRate), ref _vatRate, value)) {
                    if (!IsLoading && Offer != null) Offer.UpdateTotalAmount();
                }
            }
        }

        // PersistentAlias yerine manuel hesaplama yapalım, Offer.UpdateTotalAmount tarafından kullanılacak.
        [NonPersistent] // Bu alan veritabanında saklanmayacak
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
        public decimal LineTotalWithVat
        {
            get
            {
                return LineAmountCalculated + LineVatAmountCalculated;
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Unit = "Adet";
            VatRate = 18;
            Quantity = 1;
        }

        // Bir OfferItem silindiğinde Offer'ın toplamını güncellemek için.
        protected override void OnDeleting()
        {
            if (Offer != null)
            {
                // Bu item silinmeden hemen önce Offer'a haber verelim ki
                // Offer, bu item hariç diğerleriyle toplamını güncelleyebilsin.
                // Ya da Offer.OfferItems.ListChanged olayı bunu zaten yakalar.
                // En temizi Offer.OfferItems.ListChanged olayının ItemDeleted durumunu işlemesi.
            }
            base.OnDeleting();
        }
    }
}
