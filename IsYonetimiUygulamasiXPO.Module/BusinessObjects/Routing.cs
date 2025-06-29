using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("İmalat Yönetimi")]
    [DisplayName("Üretim Rotası (XPO)")]
    public class Routing : XPObject
    {
        public Routing(Session session) : base(session) { }

        private string _routingCode;
        [Size(50)]
        [Indexed(Unique = true)]
        [XafDisplayName("Rota Kodu")]
        public string RoutingCode { get => _routingCode; set => SetPropertyValue(nameof(RoutingCode), ref _routingCode, value); }

        private Material _product;
        [Association("Material-RoutingProduct")]
        [XafDisplayName("Üretilecek Ürün/Mamul")]
        public Material Product { get => _product; set => SetPropertyValue(nameof(Product), ref _product, value); }

        private string _description;
        [XafDisplayName("Açıklama")]
        public string Description { get => _description; set => SetPropertyValue(nameof(Description), ref _description, value); }

        private string _version;
        [XafDisplayName("Versiyon")]
        public string Version { get => _version; set => SetPropertyValue(nameof(Version), ref _version, value); }

        private bool _isActive;
        [XafDisplayName("Aktif Rota")]
        public bool IsActive { get => _isActive; set => SetPropertyValue(nameof(IsActive), ref _isActive, value); }

        [Association("Routing-Operations"), Aggregated]
        [XafDisplayName("Rota Operasyonları")]
        public XPCollection<RoutingOperation> Operations => GetCollection<RoutingOperation>(nameof(Operations));

        [Association("Routing-ProductionOrders")]
        [XafDisplayName("Kullanıldığı Üretim Emirleri")]
        public XPCollection<ProductionOrder> ProductionOrders => GetCollection<ProductionOrder>(nameof(ProductionOrders));

        public override void AfterConstruction() { base.AfterConstruction(); IsActive = true; Version = "1.0"; }
    }
}
