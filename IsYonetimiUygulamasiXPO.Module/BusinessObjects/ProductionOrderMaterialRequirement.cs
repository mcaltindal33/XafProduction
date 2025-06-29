using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Üretim Emri Malzeme İhtiyacı (XPO)")]
    public class ProductionOrderMaterialRequirement : XPObject
    {
        public ProductionOrderMaterialRequirement(Session session) : base(session) { }

        private ProductionOrder _productionOrder;
        [Association("PO-MaterialRequirements")]
        [Browsable(false)]
        public ProductionOrder ProductionOrder { get => _productionOrder; set => SetPropertyValue(nameof(ProductionOrder), ref _productionOrder, value); }

        private Material _material;
        [Association("Material-ProductionOrderMaterialReq")]
        [XafDisplayName("Malzeme")]
        public Material Material {
            get => _material;
            set {
                if(SetPropertyValue(nameof(Material), ref _material, value))
                {
                    if(!IsLoading && Material != null)
                    {
                        Unit = Material.BaseUnit;
                    }
                }
            }
        }

        private double _requiredQuantityFromBom;
        [XafDisplayName("Gerekli Miktar (BOM'dan)")]
        public double RequiredQuantityFromBom { get => _requiredQuantityFromBom; set => SetPropertyValue(nameof(RequiredQuantityFromBom), ref _requiredQuantityFromBom, value); }

        private double _plannedQuantity;
        [XafDisplayName("Planlanan Miktar")]
        public double PlannedQuantity { get => _plannedQuantity; set => SetPropertyValue(nameof(PlannedQuantity), ref _plannedQuantity, value); }

        private double _actualQuantityUsed;
        [XafDisplayName("Fiili Kullanılan Miktar")]
        public double ActualQuantityUsed { get => _actualQuantityUsed; set => SetPropertyValue(nameof(ActualQuantityUsed), ref _actualQuantityUsed, value); }

        private string _unit;
        [XafDisplayName("Birim")]
        public string Unit { get => _unit; set => SetPropertyValue(nameof(Unit), ref _unit, value); }

        private Warehouse _warehouse;
        [Association("Warehouse-ProductionOrderMaterialRequirements")]
        [XafDisplayName("Depo")]
        public Warehouse Warehouse { get => _warehouse; set => SetPropertyValue(nameof(Warehouse), ref _warehouse, value); }

        [Association("ProductionOrderMaterialRequirement-StockOutbounds"), Aggregated] // Bu ihtiyaç için yapılan stok çıkışları
        [XafDisplayName("Stok Çıkış Hareketleri (Sarf)")]
        public XPCollection<StockTransaction> StockOutbounds => GetCollection<StockTransaction>(nameof(StockOutbounds));

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            PlannedQuantity = RequiredQuantityFromBom; // Başlangıçta eşit olabilir, fire vb. sonra eklenebilir.
        }
    }
}
