using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Üretim Yönetimi")]
    [DisplayName("İş İstasyonu (XPO)")]
    public class Workstation : XPObject
    {
        public Workstation(Session session) : base(session) { }

        private string _workstationCode;
        [Size(50)]
        [Indexed(Unique = true)]
        [XafDisplayName("İş İstasyonu Kodu")]
        public string WorkstationCode
        {
            get => _workstationCode;
            set => SetPropertyValue(nameof(WorkstationCode), ref _workstationCode, value);
        }

        private string _workstationName;
        [Size(255)]
        [XafDisplayName("İş İstasyonu Adı")]
        public string WorkstationName
        {
            get => _workstationName;
            set => SetPropertyValue(nameof(WorkstationName), ref _workstationName, value);
        }

        private string _description;
        [XafDisplayName("Açıklama")]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        private string _department;
        [XafDisplayName("Departman")]
        public string Department
        {
            get => _department;
            set => SetPropertyValue(nameof(Department), ref _department, value);
        }

        private bool _isActive;
        [XafDisplayName("Aktif")]
        public bool IsActive
        {
            get => _isActive;
            set => SetPropertyValue(nameof(IsActive), ref _isActive, value);
        }

        [Association("Workstation-RoutingOperations")]
        [XafDisplayName("Rota Operasyonları")]
        public XPCollection<RoutingOperation> RoutingOperations => GetCollection<RoutingOperation>(nameof(RoutingOperations));

        [Association("Workstation-ProductionOrderOperationLogs")]
        [XafDisplayName("Üretim Operasyon Kayıtları")]
        public XPCollection<ProductionOrderOperationLog> ProductionOrderOperationLogs => GetCollection<ProductionOrderOperationLog>(nameof(ProductionOrderOperationLogs));


        public override void AfterConstruction() { base.AfterConstruction(); IsActive = true; }
    }
}
