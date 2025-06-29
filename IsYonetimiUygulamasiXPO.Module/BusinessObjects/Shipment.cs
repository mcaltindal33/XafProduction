using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Sevkiyat Yönetimi")]
    [DisplayName("Sevkiyat/İrsaliye (XPO)")]
    public class Shipment : XPObject
    {
        public Shipment(Session session) : base(session) { }

        private string _shipmentNumber;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Sevkiyat/İrsaliye No")]
        public string ShipmentNumber
        {
            get => _shipmentNumber;
            set => SetPropertyValue(nameof(ShipmentNumber), ref _shipmentNumber, value);
        }

        private DateTime _shipmentDate;
        [XafDisplayName("Sevkiyat Tarihi")]
        public DateTime ShipmentDate
        {
            get => _shipmentDate;
            set => SetPropertyValue(nameof(ShipmentDate), ref _shipmentDate, value);
        }

        private Customer _customer;
        [Association("Customer-Shipments")]
        [XafDisplayName("Müşteri")]
        public Customer Customer
        {
            get => _customer;
            set => SetPropertyValue(nameof(Customer), ref _customer, value);
        }

        private Offer _relatedOffer;
        // Offer'da Shipment için bir koleksiyon tanımlamadıysak buradaki Association tek yönlü kalabilir.
        // Eğer Offer'da da Shipments koleksiyonu olacaksa çift yönlü Association tanımlanır.
        [XafDisplayName("İlişkili Satış Teklifi")]
        public Offer RelatedOffer
        {
            get => _relatedOffer;
            set => SetPropertyValue(nameof(RelatedOffer), ref _relatedOffer, value);
        }

        private string _shippingAddress;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Teslimat Adresi")]
        public string ShippingAddress
        {
            get => _shippingAddress;
            set => SetPropertyValue(nameof(ShippingAddress), ref _shippingAddress, value);
        }

        private string _carrier;
        [XafDisplayName("Taşıyıcı Firma")]
        public string Carrier
        {
            get => _carrier;
            set => SetPropertyValue(nameof(Carrier), ref _carrier, value);
        }

        private string _trackingNumber;
        [XafDisplayName("Takip Numarası")]
        public string TrackingNumber
        {
            get => _trackingNumber;
            set => SetPropertyValue(nameof(TrackingNumber), ref _trackingNumber, value);
        }

        private ShipmentStatus _status;
        [XafDisplayName("Durum")]
        public ShipmentStatus Status
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

        [Association("Shipment-ShipmentItems"), Aggregated]
        [XafDisplayName("Sevkiyat Kalemleri")]
        public XPCollection<ShipmentItem> ShipmentItems => GetCollection<ShipmentItem>(nameof(ShipmentItems));

        [Association("Shipment-Invoices")] // Bir sevkiyattan birden fazla fatura kesilebilir (kısmi faturalama)
        [XafDisplayName("İlişkili Faturalar")]
        public XPCollection<Invoice> Invoices => GetCollection<Invoice>(nameof(Invoices));

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ShipmentDate = DateTime.Now;
            Status = ShipmentStatus.Preparing;
        }
    }

    public enum ShipmentStatus {
        [Display(Name="Hazırlanıyor")] Preparing = 0,
        [Display(Name="Sevk Edildi")] Shipped = 1,
        [Display(Name="Teslim Edildi")] Delivered = 2,
        [Display(Name="İptal Edildi")] Cancelled = 3
    }
}
