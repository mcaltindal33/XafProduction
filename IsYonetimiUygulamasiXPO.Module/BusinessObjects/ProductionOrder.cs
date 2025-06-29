using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Üretim Yönetimi")]
    [DisplayName("Üretim Emri (XPO)")]
    public class ProductionOrder : XPObject
    {
        public ProductionOrder(Session session) : base(session) { }

        private string _orderNumber;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Üretim Emri No")]
        public string OrderNumber { get => _orderNumber; set => SetPropertyValue(nameof(OrderNumber), ref _orderNumber, value); }

        private Material _product;
        [Association("Material-ProductionOrderProduct")]
        [XafDisplayName("Üretilecek Ürün")]
        public Material Product {
            get => _product;
            set {
                if(SetPropertyValue(nameof(Product), ref _product, value))
                {
                    if(!IsLoading && Product != null)
                    {
                        Unit = Product.BaseUnit;
                        // İlgili BOM ve Rota'yı otomatik çekebiliriz
                        // BillOfMaterials = Session.FindObject<BillOfMaterials>(CriteriaOperator.Parse("Product.Oid = ? And IsActive = true", Product.Oid));
                        // Routing = Session.FindObject<Routing>(CriteriaOperator.Parse("Product.Oid = ? And IsActive = true", Product.Oid));
                    }
                }
            }
        }

        private double _quantityToProduce;
        [XafDisplayName("Üretilecek Miktar")]
        public double QuantityToProduce { get => _quantityToProduce; set => SetPropertyValue(nameof(QuantityToProduce), ref _quantityToProduce, value); }

        private string _unit;
        [XafDisplayName("Birim")]
        public string Unit { get => _unit; set => SetPropertyValue(nameof(Unit), ref _unit, value); }

        private DateTime _plannedStartDate;
        [XafDisplayName("Planlanan Başlangıç")]
        public DateTime PlannedStartDate { get => _plannedStartDate; set => SetPropertyValue(nameof(PlannedStartDate), ref _plannedStartDate, value); }

        private DateTime _plannedEndDate;
        [XafDisplayName("Planlanan Bitiş")]
        public DateTime PlannedEndDate { get => _plannedEndDate; set => SetPropertyValue(nameof(PlannedEndDate), ref _plannedEndDate, value); }

        private DateTime? _actualStartDate;
        [XafDisplayName("Fiili Başlangıç")]
        public DateTime? ActualStartDate { get => _actualStartDate; set => SetPropertyValue(nameof(ActualStartDate), ref _actualStartDate, value); }

        private DateTime? _actualEndDate;
        [XafDisplayName("Fiili Bitiş")]
        public DateTime? ActualEndDate { get => _actualEndDate; set => SetPropertyValue(nameof(ActualEndDate), ref _actualEndDate, value); }

        private double _quantityProduced;
        [XafDisplayName("Üretilen Miktar")]
        public double QuantityProduced { get => _quantityProduced; set => SetPropertyValue(nameof(QuantityProduced), ref _quantityProduced, value); }

        private double _quantityScrapped;
        [XafDisplayName("Fire Miktarı")]
        public double QuantityScrapped { get => _quantityScrapped; set => SetPropertyValue(nameof(QuantityScrapped), ref _quantityScrapped, value); }

        private ProductionOrderStatus _status;
        [XafDisplayName("Durum")]
        public ProductionOrderStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        private BillOfMaterials _billOfMaterials;
        [Association("BillOfMaterials-ProductionOrders")]
        [XafDisplayName("Ürün Ağacı (Reçete)")]
        public BillOfMaterials BillOfMaterials { get => _billOfMaterials; set => SetPropertyValue(nameof(BillOfMaterials), ref _billOfMaterials, value); }

        private Routing _routing;
        [Association("Routing-ProductionOrders")]
        [XafDisplayName("Üretim Rotası")]
        public Routing Routing { get => _routing; set => SetPropertyValue(nameof(Routing), ref _routing, value); }

        private string _relatedSalesOrder;
        [XafDisplayName("İlişkili Satış Siparişi")]
        public string RelatedSalesOrder { get => _relatedSalesOrder; set => SetPropertyValue(nameof(RelatedSalesOrder), ref _relatedSalesOrder, value); }

        [Association("PO-MaterialRequirements"), Aggregated]
        [XafDisplayName("Gerekli Malzemeler")]
        public XPCollection<ProductionOrderMaterialRequirement> MaterialRequirements => GetCollection<ProductionOrderMaterialRequirement>(nameof(MaterialRequirements));

        [Association("PO-OperationLogs"), Aggregated]
        [XafDisplayName("Üretim Operasyonları")]
        public XPCollection<ProductionOrderOperationLog> OperationLogs => GetCollection<ProductionOrderOperationLog>(nameof(OperationLogs));

        [Association("ProductionOrder-StockInbounds"), Aggregated] // Üretimden giriş hareketleri
        [XafDisplayName("Stok Giriş Hareketleri (Mamul)")]
        public XPCollection<StockTransaction> StockInbounds => GetCollection<StockTransaction>(nameof(StockInbounds));


        public override void AfterConstruction() { base.AfterConstruction(); PlannedStartDate = DateTime.Now; Status = ProductionOrderStatus.Planned; QuantityToProduce = 1; }
    }

    public enum ProductionOrderStatus {
        [Display(Name="Planlandı")] Planned = 0,
        [Display(Name="Başlatıldı")] Started = 1,
        [Display(Name="Devam Ediyor")] InProgress = 2,
        [Display(Name="Tamamlandı")] Completed = 3,
        [Display(Name="Kısmen Tamamlandı")] PartiallyCompleted = 4,
        [Display(Name="İptal Edildi")] Cancelled = 5,
        [Display(Name="Beklemede")] OnHold = 6
    }
}
