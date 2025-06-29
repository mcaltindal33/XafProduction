using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Satınalma Sipariş Kalemi (XPO)")]
    public class PurchaseOrderItem : XPObject
    {
        public PurchaseOrderItem(Session session) : base(session) { }

        private PurchaseOrder _purchaseOrder;
        [Association("PurchaseOrder-OrderItems")]
        [Browsable(false)]
        public PurchaseOrder PurchaseOrder
        {
            get => _purchaseOrder;
            set => SetPropertyValue(nameof(PurchaseOrder), ref _purchaseOrder, value);
        }

        private Material _material; // Malzeme nesnesine referans
        [XafDisplayName("Malzeme/Ürün")]
        public Material Material
        {
            get => _material;
            set {
                if(SetPropertyValue(nameof(Material), ref _material, value))
                {
                    if(!IsLoading && Material != null)
                    {
                        ItemName = Material.MaterialName;
                        Unit = Material.BaseUnit;
                        // UnitPrice = Material.UnitCost; // Malzemenin maliyetini birim fiyata çekebilir
                        if (PurchaseOrder != null) PurchaseOrder.UpdateTotalAmount();
                    }
                }
            }
        }

        private string _itemName;
        [Size(255)]
        [XafDisplayName("Kalem Adı (Malzemeden gelir)")]
        public string ItemName
        {
            get => _itemName;
            set => SetPropertyValue(nameof(ItemName), ref _itemName, value);
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
                    if (!IsLoading && PurchaseOrder != null) PurchaseOrder.UpdateTotalAmount();
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
                     if (!IsLoading && PurchaseOrder != null) PurchaseOrder.UpdateTotalAmount();
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
                    if (!IsLoading && PurchaseOrder != null) PurchaseOrder.UpdateTotalAmount();
                }
            }
        }

        [NonPersistent]
        [XafDisplayName("Satır Tutarı (KDV'siz)")]
        public decimal LineTotalCalculated
        {
            get { return (decimal)Quantity * UnitPrice; }
        }

        [NonPersistent]
        [XafDisplayName("Satır KDV Tutarı")]
        public decimal LineVatAmountCalculated
        {
            get { return LineTotalCalculated * (VatRate / 100); }
        }

        [NonPersistent]
        [XafDisplayName("Satır Toplamı (KDV Dahil)")]
        public decimal LineTotalWithVatCalculated
        {
            get { return LineTotalCalculated + LineVatAmountCalculated; }
        }

        [Association("PurchaseOrderItem-StockTransactions")]
        [XafDisplayName("İlişkili Stok Hareketleri")]
        public XPCollection<StockTransaction> StockTransactions => GetCollection<StockTransaction>(nameof(StockTransactions));


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Unit = "Adet";
            VatRate = 18;
            Quantity = 1;
        }
    }
}
