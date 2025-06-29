using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System; // Convert için
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Stok Sayım Kalemi (XPO)")]
    public class StockCountItem : XPObject
    {
        public StockCountItem(Session session) : base(session) { }

        private StockCount _stockCount;
        [Association("StockCount-CountItems")]
        [Browsable(false)]
        public StockCount StockCount { get => _stockCount; set => SetPropertyValue(nameof(StockCount), ref _stockCount, value); }

        private Material _material;
        [Association("Material-StockCountItems")]
        [XafDisplayName("Malzeme")]
        public Material Material {
            get => _material;
            set {
                if(SetPropertyValue(nameof(Material), ref _material, value))
                {
                    if(!IsLoading && Material != null)
                    {
                        Unit = Material.BaseUnit;
                        // Sistem miktarını burada da çekebiliriz, ama StockCount.LoadSystemQuantities daha toplu yapar.
                    }
                }
            }
        }

        private double _systemQuantity;
        [XafDisplayName("Sistem Miktarı")] // Sayım anındaki teorik miktar (Depo bazlı)
        public double SystemQuantity { get => _systemQuantity; set => SetPropertyValue(nameof(SystemQuantity), ref _systemQuantity, value); }

        private double _countedQuantity;
        [XafDisplayName("Sayılan Miktar")]
        public double CountedQuantity { get => _countedQuantity; set => SetPropertyValue(nameof(CountedQuantity), ref _countedQuantity, value); }

        [PersistentAlias("CountedQuantity - SystemQuantity")]
        [XafDisplayName("Fark")]
        public double Difference => Convert.ToDouble(EvaluateAlias(nameof(Difference)));

        private string _unit;
        [XafDisplayName("Birim")]
        public string Unit { get => _unit; set => SetPropertyValue(nameof(Unit), ref _unit, value); }

        private bool _isAdjusted;
        [XafDisplayName("Düzeltme Yapıldı")] // Fark stok hareketlerine yansıtıldı mı?
        public bool IsAdjusted { get => _isAdjusted; set => SetPropertyValue(nameof(IsAdjusted), ref _isAdjusted, value); }

        // Bu sayım farkı için oluşturulan stok hareketi (eğer varsa)
        [Association("StockCountItem-StockTransaction"), Aggregated]
        [XafDisplayName("Düzeltme Stok Hareketi")]
        public StockTransaction AdjustmentStockTransaction {
            get => GetPropertyValue<StockTransaction>(nameof(AdjustmentStockTransaction));
            set => SetPropertyValue(nameof(AdjustmentStockTransaction), value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CountedQuantity = 0;
            IsAdjusted = false;
        }
    }
}
