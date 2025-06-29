using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Üretim Operasyon Kaydı (XPO)")]
    public class ProductionOrderOperationLog : XPObject
    {
        public ProductionOrderOperationLog(Session session) : base(session) { }

        private ProductionOrder _productionOrder;
        [Association("PO-OperationLogs")]
        [Browsable(false)]
        public ProductionOrder ProductionOrder { get => _productionOrder; set => SetPropertyValue(nameof(ProductionOrder), ref _productionOrder, value); }

        private RoutingOperation _routingOperation;
        [Association("RoutingOperation-ProductionOrderOperationLogs")]
        [XafDisplayName("Rota Operasyonu")]
        public RoutingOperation RoutingOperation {
            get => _routingOperation;
            set {
                if(SetPropertyValue(nameof(RoutingOperation), ref _routingOperation, value))
                {
                    if(!IsLoading && RoutingOperation != null)
                    {
                        OperationName = RoutingOperation.OperationName;
                        Workstation = RoutingOperation.Workstation;
                    }
                }
            }
        }

        private string _operationName;
        [XafDisplayName("Operasyon Adı")]
        public string OperationName { get => _operationName; set => SetPropertyValue(nameof(OperationName), ref _operationName, value); }

        private Workstation _workstation;
        [Association("Workstation-ProductionOrderOperationLogs")]
        [XafDisplayName("İş İstasyonu")]
        public Workstation Workstation { get => _workstation; set => SetPropertyValue(nameof(Workstation), ref _workstation, value); }

        private DateTime? _startTime;
        [XafDisplayName("Başlangıç Zamanı")]
        public DateTime? StartTime { get => _startTime; set => SetPropertyValue(nameof(StartTime), ref _startTime, value); }

        private DateTime? _endTime;
        [XafDisplayName("Bitiş Zamanı")]
        public DateTime? EndTime { get => _endTime; set => SetPropertyValue(nameof(EndTime), ref _endTime, value); }

        private double _quantityCompleted;
        [XafDisplayName("Tamamlanan Miktar")]
        public double QuantityCompleted { get => _quantityCompleted; set => SetPropertyValue(nameof(QuantityCompleted), ref _quantityCompleted, value); }

        private double _quantityScrapped;
        [XafDisplayName("Fire Miktarı")]
        public double QuantityScrapped { get => _quantityScrapped; set => SetPropertyValue(nameof(QuantityScrapped), ref _quantityScrapped, value); }

        private OperationLogStatus _status;
        [XafDisplayName("Durum")]
        public OperationLogStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        private string _operatorName; // Veya Employee nesnesi
        [XafDisplayName("Operatör")]
        public string OperatorName { get => _operatorName; set => SetPropertyValue(nameof(OperatorName), ref _operatorName, value); }

        public override void AfterConstruction() { base.AfterConstruction(); Status = OperationLogStatus.Pending; }
    }

    public enum OperationLogStatus
    {
        [Display(Name = "Beklemede")] Pending = 0,
        [Display(Name = "Devam Ediyor")] InProgress = 1,
        [Display(Name = "Tamamlandı")] Completed = 2,
        [Display(Name = "Durduruldu")] Paused = 3
    }
}
