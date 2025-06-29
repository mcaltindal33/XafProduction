using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Malzeme Yönetimi")]
    [DisplayName("Malzeme/Ürün (XPO)")]
    public class Material : XPObject
    {
        public Material(Session session) : base(session) { }

        private string _materialCode;
        [Size(50)]
        [Indexed(Unique = true)]
        [XafDisplayName("Malzeme Kodu")]
        public string MaterialCode
        {
            get => _materialCode;
            set => SetPropertyValue(nameof(MaterialCode), ref _materialCode, value);
        }

        private string _materialName;
        [Size(255)]
        [XafDisplayName("Malzeme Adı")]
        public string MaterialName
        {
            get => _materialName;
            set => SetPropertyValue(nameof(MaterialName), ref _materialName, value);
        }

        private string _description;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Açıklama")]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        private string _baseUnit;
        [Size(50)]
        [XafDisplayName("Temel Birim")]
        public string BaseUnit
        {
            get => _baseUnit;
            set => SetPropertyValue(nameof(BaseUnit), ref _baseUnit, value);
        }

        private decimal _unitCost;
        [XafDisplayName("Birim Maliyet (Alış)")]
        public decimal UnitCost
        {
            get => _unitCost;
            set => SetPropertyValue(nameof(UnitCost), ref _unitCost, value);
        }

        private decimal _unitPrice;
        [XafDisplayName("Birim Fiyat (Satış)")]
        public decimal UnitPrice
        {
            get => _unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref _unitPrice, value);
        }

        private string _category;
        [XafDisplayName("Kategori")]
        public string Category
        {
            get => _category;
            set => SetPropertyValue(nameof(Category), ref _category, value);
        }

        private bool _trackStock;
        [XafDisplayName("Stok Takibi")]
        public bool TrackStock
        {
            get => _trackStock;
            set => SetPropertyValue(nameof(TrackStock), ref _trackStock, value);
        }

        [XafDisplayName("Toplam Stok Miktarı")]
        [PersistentAlias("StockTransactions.Sum(Quantity)")]
        public double TotalStockQuantity => TrackStock ? Convert.ToDouble(EvaluateAlias(nameof(TotalStockQuantity))) : 0;

        [Association("Material-StockTransactions")]
        public XPCollection<StockTransaction> StockTransactions => GetCollection<StockTransaction>(nameof(StockTransactions));

        [Association("Material-BillOfMaterialsProduct")] // Bu malzemenin üretildiği ürün ağaçları
        [XafDisplayName("Ürün Ağaçları (Mamul Olarak)")]
        public XPCollection<BillOfMaterials> BillOfMaterialsAsProduct => GetCollection<BillOfMaterials>(nameof(BillOfMaterialsAsProduct));

        [Association("Material-BomItems")] // Bu malzemenin kullanıldığı ürün ağacı kalemleri
        [XafDisplayName("Ürün Ağacı Kalemleri (Hammadde/Yarı Mamul Olarak)")]
        public XPCollection<BomItem> BomItems => GetCollection<BomItem>(nameof(BomItems));

        [Association("Material-RoutingProduct")]
        [XafDisplayName("Üretim Rotaları (Mamul Olarak)")]
        public XPCollection<Routing> RoutingsAsProduct => GetCollection<Routing>(nameof(RoutingsAsProduct));

        [Association("Material-ProductionOrderProduct")]
        [XafDisplayName("Üretim Emirleri (Mamul Olarak)")]
        public XPCollection<ProductionOrder> ProductionOrdersAsProduct => GetCollection<ProductionOrder>(nameof(ProductionOrdersAsProduct));

        [Association("Material-ProductionOrderMaterialReq")]
        [XafDisplayName("Üretim Emri Malzeme İhtiyaçları (Hammadde/Yarı Mamul Olarak)")]
        public XPCollection<ProductionOrderMaterialRequirement> ProductionOrderMaterialRequirements => GetCollection<ProductionOrderMaterialRequirement>(nameof(ProductionOrderMaterialRequirements));

        [Association("Material-ShipmentItems")]
        [XafDisplayName("Sevkiyat Kalemleri")]
        public XPCollection<ShipmentItem> ShipmentItems => GetCollection<ShipmentItem>(nameof(ShipmentItems));

        [Association("Material-InvoiceItems")]
        [XafDisplayName("Fatura Kalemleri")]
        public XPCollection<InvoiceItem> InvoiceItems => GetCollection<InvoiceItem>(nameof(InvoiceItems));

        [Association("Material-QualityChecks")]
        [XafDisplayName("Kalite Kontrol Kayıtları")]
        public XPCollection<QualityCheck> QualityChecks => GetCollection<QualityCheck>(nameof(QualityChecks));

        [Association("Material-StockCountItems")]
        [XafDisplayName("Stok Sayım Kalemleri")]
        public XPCollection<StockCountItem> StockCountItems => GetCollection<StockCountItem>(nameof(StockCountItems));

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            BaseUnit = "Adet";
            TrackStock = true;
        }
    }
}
