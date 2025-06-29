using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Ürün Ağacı Kalemi (XPO)")]
    public class BomItem : XPObject
    {
        public BomItem(Session session) : base(session) { }

        private BillOfMaterials _billOfMaterials;
        [Association("BOM-BomItems")]
        [Browsable(false)]
        public BillOfMaterials BillOfMaterials
        {
            get => _billOfMaterials;
            set => SetPropertyValue(nameof(BillOfMaterials), ref _billOfMaterials, value);
        }

        private Material _material;
        [Association("Material-BomItems")]
        [XafDisplayName("Malzeme/Yarı Mamul")]
        public Material Material
        {
            get => _material;
            set {
                if(SetPropertyValue(nameof(Material), ref _material, value))
                {
                    if(!IsLoading && Material != null)
                    {
                        // Malzeme seçildiğinde birimini otomatik çekebiliriz
                        Unit = Material.BaseUnit;
                    }
                }
            }
        }

        private double _quantity;
        [XafDisplayName("Miktar")]
        public double Quantity { get => _quantity; set => SetPropertyValue(nameof(Quantity), ref _quantity, value); }

        private string _unit;
        [XafDisplayName("Birim")]
        public string Unit { get => _unit; set => SetPropertyValue(nameof(Unit), ref _unit, value); }

        private double _scrapPercentage;
        [XafDisplayName("Fire Oranı (%)")]
        public double ScrapPercentage { get => _scrapPercentage; set => SetPropertyValue(nameof(ScrapPercentage), ref _scrapPercentage, value); }

        private string _notes;
        [XafDisplayName("Notlar")]
        public string Notes { get => _notes; set => SetPropertyValue(nameof(Notes), ref _notes, value); }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Quantity = 1;
        }
    }
}
