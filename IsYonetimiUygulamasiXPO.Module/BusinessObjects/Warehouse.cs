using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Depo Yönetimi")]
    [DisplayName("Depo (XPO)")]
    public class Warehouse : XPObject
    {
        public Warehouse(Session session) : base(session) { }

        private string _warehouseCode;
        [Size(50)]
        [Indexed(Unique = true)]
        [XafDisplayName("Depo Kodu")]
        public string WarehouseCode
        {
            get => _warehouseCode;
            set => SetPropertyValue(nameof(WarehouseCode), ref _warehouseCode, value);
        }

        private string _warehouseName;
        [Size(255)]
        [XafDisplayName("Depo Adı")]
        public string WarehouseName
        {
            get => _warehouseName;
            set => SetPropertyValue(nameof(WarehouseName), ref _warehouseName, value);
        }

        private string _address;
        [XafDisplayName("Adres")]
        public string Address
        {
            get => _address;
            set => SetPropertyValue(nameof(Address), ref _address, value);
        }

        private string _responsiblePerson;
        [XafDisplayName("Yetkili Kişi")]
        public string ResponsiblePerson
        {
            get => _responsiblePerson;
            set => SetPropertyValue(nameof(ResponsiblePerson), ref _responsiblePerson, value);
        }

        private bool _isActive;
        [XafDisplayName("Aktif")]
        public bool IsActive
        {
            get => _isActive;
            set => SetPropertyValue(nameof(IsActive), ref _isActive, value);
        }

        [Association("Warehouse-StockTransactions")]
        [XafDisplayName("Stok Hareketleri")]
        public XPCollection<StockTransaction> StockTransactions => GetCollection<StockTransaction>(nameof(StockTransactions));

        [Association("Warehouse-StockCounts")]
        [XafDisplayName("Stok Sayımları")]
        public XPCollection<StockCount> StockCounts => GetCollection<StockCount>(nameof(StockCounts));

        [Association("Warehouse-ProductionOrderMaterialRequirements")] // Malzeme ihtiyaçlarının hangi depodan karşılanacağı
        [XafDisplayName("Üretim Malzeme İhtiyaçları")]
        public XPCollection<ProductionOrderMaterialRequirement> ProductionOrderMaterialRequirements => GetCollection<ProductionOrderMaterialRequirement>(nameof(ProductionOrderMaterialRequirements));

        [Association("Warehouse-ShipmentItems")] // Sevkiyat kalemlerinin hangi depodan çıktığı
        [XafDisplayName("Sevkiyat Kalemleri")]
        public XPCollection<ShipmentItem> ShipmentItems => GetCollection<ShipmentItem>(nameof(ShipmentItems));


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            IsActive = true;
        }
    }
}
