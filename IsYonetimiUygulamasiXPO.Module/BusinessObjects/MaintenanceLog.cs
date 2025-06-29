using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Teknik Servis")]
    [DisplayName("Bakım Kaydı (XPO)")]
    public class MaintenanceLog : XPObject
    {
        public MaintenanceLog(Session session) : base(session) { }

        private Equipment _equipment;
        [Association("Equipment-MaintenanceLogs")]
        [XafDisplayName("Ekipman")]
        public Equipment Equipment { get => _equipment; set => SetPropertyValue(nameof(Equipment), ref _equipment, value); }

        private MaintenancePlan _maintenancePlan;
        [Association("MaintenancePlan-MaintenanceLogs")]
        [XafDisplayName("İlişkili Bakım Planı")]
        public MaintenancePlan MaintenancePlan { get => _maintenancePlan; set => SetPropertyValue(nameof(MaintenancePlan), ref _maintenancePlan, value); }

        private ServiceTicket _serviceTicket;
        [Association("ServiceTicket-MaintenanceLogs")]
        [XafDisplayName("İlişkili Servis Talebi")] // Eğer bir servis talebi üzerinden yapıldıysa
        public ServiceTicket ServiceTicket { get => _serviceTicket; set => SetPropertyValue(nameof(ServiceTicket), ref _serviceTicket, value); }

        private DateTime _maintenanceDate;
        [XafDisplayName("Bakım Tarihi")]
        public DateTime MaintenanceDate { get => _maintenanceDate; set => SetPropertyValue(nameof(MaintenanceDate), ref _maintenanceDate, value); }

        private string _workPerformed;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Yapılan İşlemler")]
        public string WorkPerformed { get => _workPerformed; set => SetPropertyValue(nameof(WorkPerformed), ref _workPerformed, value); }

        private Employee _technician; // Employee nesnesine referans
        [XafDisplayName("Teknisyen")]
        public Employee Technician { get => _technician; set => SetPropertyValue(nameof(Technician), ref _technician, value); }

        private string _materialsUsed;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Kullanılan Malzemeler")] // Daha sonra XPCollection<MaterialUsage> gibi bir yapıya dönüşebilir
        public string MaterialsUsed { get => _materialsUsed; set => SetPropertyValue(nameof(MaterialsUsed), ref _materialsUsed, value); }

        private decimal _cost;
        [XafDisplayName("Maliyet")]
        public decimal Cost { get => _cost; set => SetPropertyValue(nameof(Cost), ref _cost, value); }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            MaintenanceDate = DateTime.Now;
            // Eğer MaintenancePlan seçiliyse, Ekipmanı oradan alabiliriz.
            if (MaintenancePlan != null)
            {
                Equipment = MaintenancePlan.Equipment;
            }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            // Bakım yapıldıktan sonra ilgili MaintenancePlan'ın LastMaintenanceDate'ini güncelleyebiliriz.
            if (MaintenancePlan != null && MaintenancePlan.LastMaintenanceDate < MaintenanceDate)
            {
                MaintenancePlan.LastMaintenanceDate = MaintenanceDate;
                // MaintenancePlan.CalculateNextDueDate(); // Bu zaten OnChanged ile tetikleniyor olacak.
                // Session.CommitChanges(); // Eğer ayrı bir UnitOfWork içinde değilse. XAF bunu yönetir.
            }
        }
    }
}
