using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Sevkiyat Kalemi (XPO)")]
    public class ShipmentItem : XPObject
    {
        public ShipmentItem(Session session) : base(session) { }

        private Shipment _shipment;
        [Association("Shipment-ShipmentItems")]
        [Browsable(false)]
        public Shipment Shipment
        {
            get => _shipment;
            set => SetPropertyValue(nameof(Shipment), ref _shipment, value);
        }

        private Material _material;
        [Association("Material-ShipmentItems")]
        [XafDisplayName("Malzeme/Ürün")]
        public Material Material
        {
            get => _material;
            set {
                if (SetPropertyValue(nameof(Material), ref _material, value))
                {
                    if (!IsLoading && Material != null)
                    {
                        Description = Material.MaterialName;
                        Unit = Material.BaseUnit;
                    }
                }
            }
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
            set => SetPropertyValue(nameof(Quantity), ref _quantity, value);
        }

        private string _unit;
        [Size(50)]
        [XafDisplayName("Birim")]
        public string Unit
        {
            get => _unit;
            set => SetPropertyValue(nameof(Unit), ref _unit, value);
        }

        private Warehouse _warehouse;
        [Association("Warehouse-ShipmentItems")]
        [XafDisplayName("Depo")]
        public Warehouse Warehouse
        {
            get => _warehouse;
            set => SetPropertyValue(nameof(Warehouse), ref _warehouse, value);
        }

        [Association("ShipmentItem-StockTransactions"), Aggregated] // Bir sevkiyat kalemi bir stok hareketi oluşturur (çıkış)
        [XafDisplayName("İlişkili Stok Hareketi")]
        public StockTransaction StockTransaction
        {
            // Tek bir stok hareketi olacağı için XPCollection değil, tekil referans.
            // Bu, sevkiyat onaylandığında veya "Sevk Edildi" durumuna geldiğinde oluşturulmalı.
            // Şimdilik sadece property olarak tanımlayalım. Otomatik oluşturma Controller ile yapılır.
            get => GetPropertyValue<StockTransaction>(nameof(StockTransaction));
            set => SetPropertyValue(nameof(StockTransaction), value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Quantity = 1;
        }
    }
}
