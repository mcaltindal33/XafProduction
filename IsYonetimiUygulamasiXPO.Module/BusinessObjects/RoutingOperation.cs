using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Rota Operasyonu (XPO)")]
    public class RoutingOperation : XPObject
    {
        public RoutingOperation(Session session) : base(session) { }

        private Routing _routing;
        [Association("Routing-Operations")]
        [Browsable(false)]
        public Routing Routing { get => _routing; set => SetPropertyValue(nameof(Routing), ref _routing, value); }

        private int _sequence;
        [XafDisplayName("Sıra No")]
        public int Sequence { get => _sequence; set => SetPropertyValue(nameof(Sequence), ref _sequence, value); }

        private string _operationName;
        [Size(255)]
        [XafDisplayName("Operasyon Adı")]
        public string OperationName { get => _operationName; set => SetPropertyValue(nameof(OperationName), ref _operationName, value); }

        private string _description;
        [XafDisplayName("Açıklama")]
        public string Description { get => _description; set => SetPropertyValue(nameof(Description), ref _description, value); }

        private Workstation _workstation;
        [Association("Workstation-RoutingOperations")]
        [XafDisplayName("İş İstasyonu")]
        public Workstation Workstation { get => _workstation; set => SetPropertyValue(nameof(Workstation), ref _workstation, value); }

        private int _setupTime;
        [XafDisplayName("Hazırlık Süresi (dk)")]
        public int SetupTime { get => _setupTime; set => SetPropertyValue(nameof(SetupTime), ref _setupTime, value); }

        private double _processingTimePerUnit;
        [XafDisplayName("İşlem Süresi (dk/birim)")]
        public double ProcessingTimePerUnit { get => _processingTimePerUnit; set => SetPropertyValue(nameof(ProcessingTimePerUnit), ref _processingTimePerUnit, value); }

        private int _waitTime;
        [XafDisplayName("Bekleme Süresi (dk)")]
        public int WaitTime { get => _waitTime; set => SetPropertyValue(nameof(WaitTime), ref _waitTime, value); }

        [Association("RoutingOperation-ProductionOrderOperationLogs")]
        [XafDisplayName("Üretim Operasyon Kayıtları")]
        public XPCollection<ProductionOrderOperationLog> ProductionOrderOperationLogs => GetCollection<ProductionOrderOperationLog>(nameof(ProductionOrderOperationLogs));

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Sequence = (Routing?.Operations.Count + 1) ?? 1;
        }
    }
}
