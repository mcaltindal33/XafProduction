using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("İmalat Yönetimi")]
    [DisplayName("Ürün Ağacı (Reçete, XPO)")]
    public class BillOfMaterials : XPObject
    {
        public BillOfMaterials(Session session) : base(session) { }

        private string _bomCode;
        [Size(50)]
        [Indexed(Unique = true)]
        [XafDisplayName("Ürün Ağacı Kodu")]
        public string BomCode
        {
            get => _bomCode;
            set => SetPropertyValue(nameof(BomCode), ref _bomCode, value);
        }

        private Material _product;
        [Association("Material-BillOfMaterialsProduct")]
        [XafDisplayName("Üretilecek Ürün/Mamul")]
        public Material Product
        {
            get => _product;
            set {
                if (SetPropertyValue(nameof(Product), ref _product, value))
                {
                    if(!IsLoading && Product != null)
                    {
                        // Ürün seçildiğinde birimini otomatik çekebiliriz
                        Unit = Product.BaseUnit;
                    }
                }
            }
        }

        private string _description;
        [XafDisplayName("Açıklama")]
        public string Description { get => _description; set => SetPropertyValue(nameof(Description), ref _description, value); }

        private double _quantity;
        [XafDisplayName("Üretim Miktarı (Bu reçete kaç adet ürün için)")]
        public double Quantity { get => _quantity; set => SetPropertyValue(nameof(Quantity), ref _quantity, value); }

        private string _unit;
        [XafDisplayName("Birim")]
        public string Unit { get => _unit; set => SetPropertyValue(nameof(Unit), ref _unit, value); }

        private string _version;
        [XafDisplayName("Versiyon")]
        public string Version { get => _version; set => SetPropertyValue(nameof(Version), ref _version, value); }

        private bool _isActive;
        [XafDisplayName("Aktif Reçete")]
        public bool IsActive { get => _isActive; set => SetPropertyValue(nameof(IsActive), ref _isActive, value); }

        [Association("BOM-BomItems"), Aggregated]
        [XafDisplayName("Reçete Malzemeleri")]
        public XPCollection<BomItem> BomItems => GetCollection<BomItem>(nameof(BomItems));

        [Association("BillOfMaterials-ProductionOrders")]
        [XafDisplayName("Kullanıldığı Üretim Emirleri")]
        public XPCollection<ProductionOrder> ProductionOrders => GetCollection<ProductionOrder>(nameof(ProductionOrders));

        public override void AfterConstruction() { base.AfterConstruction(); Quantity = 1; IsActive = true; Version = "1.0"; }
    }
}
