using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Depo Yönetimi")]
    [DisplayName("Stok Hareketi (XPO)")]
    public class StockTransaction : XPObject
    {
        public StockTransaction(Session session) : base(session) { }

        private DateTime _transactionDate;
        [XafDisplayName("Hareket Tarihi")]
        public DateTime TransactionDate
        {
            get => _transactionDate;
            set => SetPropertyValue(nameof(TransactionDate), ref _transactionDate, value);
        }

        private Material _material;
        [Association("Material-StockTransactions")]
        [XafDisplayName("Malzeme/Ürün")]
        public Material Material
        {
            get => _material;
            set => SetPropertyValue(nameof(Material), ref _material, value);
        }

        private Warehouse _warehouse;
        [Association("Warehouse-StockTransactions")]
        [XafDisplayName("Depo")]
        public Warehouse Warehouse
        {
            get => _warehouse;
            set => SetPropertyValue(nameof(Warehouse), ref _warehouse, value);
        }

        private double _quantity;
        [XafDisplayName("Miktar")] // Pozitif: Giriş, Negatif: Çıkış
        public double Quantity
        {
            get => _quantity;
            set {
                if (SetPropertyValue(nameof(Quantity), ref _quantity, value))
                {
                    Material?.OnChanged(nameof(Material.TotalStockQuantity));
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

        private StockTransactionType _transactionType;
        [XafDisplayName("Hareket Tipi")]
        public StockTransactionType TransactionType
        {
            get => _transactionType;
            set => SetPropertyValue(nameof(TransactionType), ref _transactionType, value);
        }

        private string _referenceDocumentNumber;
        [XafDisplayName("İlişkili Belge No")]
        public string ReferenceDocumentNumber
        {
            get => _referenceDocumentNumber;
            set => SetPropertyValue(nameof(ReferenceDocumentNumber), ref _referenceDocumentNumber, value);
        }

        private PurchaseOrderItem _purchaseOrderItem;
        [Association("PurchaseOrderItem-StockTransactions")]
        [XafDisplayName("İlişkili Satınalma Sip. Kalemi")]
        public PurchaseOrderItem PurchaseOrderItem
        {
            get => _purchaseOrderItem;
            set => SetPropertyValue(nameof(PurchaseOrderItem), ref _purchaseOrderItem, value);
        }

        private ShipmentItem _shipmentItem;
        [Association("ShipmentItem-StockTransactions")]
        [XafDisplayName("İlişkili Sevkiyat Kalemi")]
        public ShipmentItem ShipmentItem
        {
            get => _shipmentItem;
            set => SetPropertyValue(nameof(ShipmentItem), ref _shipmentItem, value);
        }

        private ProductionOrder _productionOrderInbound; // Üretimden Giriş için
        [Association("ProductionOrder-StockInbounds")]
        [XafDisplayName("Üretim Emri (Giriş)")]
        public ProductionOrder ProductionOrderInbound
        {
            get => _productionOrderInbound;
            set => SetPropertyValue(nameof(ProductionOrderInbound), ref _productionOrderInbound, value);
        }

        private ProductionOrderMaterialRequirement _productionOrderOutboundMaterial; // Üretime Sarf için
        [Association("ProductionOrderMaterialRequirement-StockOutbounds")]
        [XafDisplayName("Üretim Malzeme İhtiyacı (Sarf)")]
        public ProductionOrderMaterialRequirement ProductionOrderOutboundMaterial
        {
            get => _productionOrderOutboundMaterial;
            set => SetPropertyValue(nameof(ProductionOrderOutboundMaterial), ref _productionOrderOutboundMaterial, value);
        }

        private StockCountItem _stockCountItem; // Sayım farkı için
        [Association("StockCountItem-StockTransaction")]
        [XafDisplayName("Stok Sayım Kalemi (Fark)")]
        public StockCountItem StockCountItem
        {
            get => _stockCountItem;
            set => SetPropertyValue(nameof(StockCountItem), ref _stockCountItem, value);
        }


        private string _description;
        [XafDisplayName("Açıklama")]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TransactionDate = DateTime.Now;
        }
    }

    public enum StockTransactionType
    {
        [Display(Name = "Giriş (Satın Alma)")] PurchaseInbound = 0,
        [Display(Name = "Çıkış (Satış/Sevkiyat)")] SalesOutbound = 1,
        [Display(Name = "Giriş (Üretimden)")] ProductionInbound = 2,
        [Display(Name = "Çıkış (Üretime Sarf)")] ProductionOutbound = 3,
        [Display(Name = "Depolar Arası Transfer (Çıkış)")] TransferOutbound = 4,
        [Display(Name = "Depolar Arası Transfer (Giriş)")] TransferInbound = 5,
        [Display(Name = "Sayım Düzeltme (Artış)")] AdjustmentIncrease = 6,
        [Display(Name = "Sayım Düzeltme (Azalış)")] AdjustmentDecrease = 7,
        [Display(Name = "İade Girişi")] ReturnInbound = 8,
        [Display(Name = "İade Çıkışı")] ReturnOutbound = 9,
        [Display(Name = "Diğer Giriş")] OtherInbound = 10,
        [Display(Name = "Diğer Çıkış")] OtherOutbound = 11
    }
}
