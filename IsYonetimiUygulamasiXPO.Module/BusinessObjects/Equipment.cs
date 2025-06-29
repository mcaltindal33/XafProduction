using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Teknik Servis")]
    [DisplayName("Makine/Ekipman (XPO)")]
    public class Equipment : XPObject
    {
        public Equipment(Session session) : base(session) { }

        private string _equipmentCode;
        [Size(50)]
        [Indexed(Unique = true)]
        [XafDisplayName("Ekipman Kodu")]
        public string EquipmentCode { get => _equipmentCode; set => SetPropertyValue(nameof(EquipmentCode), ref _equipmentCode, value); }

        private string _equipmentName;
        [Size(255)]
        [XafDisplayName("Ekipman Adı")]
        public string EquipmentName { get => _equipmentName; set => SetPropertyValue(nameof(EquipmentName), ref _equipmentName, value); }

        private string _serialNumber;
        [Indexed(Unique = true)]
        [XafDisplayName("Seri Numarası")]
        public string SerialNumber { get => _serialNumber; set => SetPropertyValue(nameof(SerialNumber), ref _serialNumber, value); }

        private string _model;
        [XafDisplayName("Model")]
        public string Model { get => _model; set => SetPropertyValue(nameof(Model), ref _model, value); }

        private string _manufacturer;
        [XafDisplayName("Marka")]
        public string Manufacturer { get => _manufacturer; set => SetPropertyValue(nameof(Manufacturer), ref _manufacturer, value); }

        private string _category;
        [XafDisplayName("Kategori")]
        public string Category { get => _category; set => SetPropertyValue(nameof(Category), ref _category, value); }

        private Customer _customer;
        [Association("Customer-Equipments")]
        [XafDisplayName("Müşteri")]
        public Customer Customer { get => _customer; set => SetPropertyValue(nameof(Customer), ref _customer, value); }

        private DateTime? _installationDate;
        [XafDisplayName("Kurulum Tarihi")]
        public DateTime? InstallationDate { get => _installationDate; set => SetPropertyValue(nameof(InstallationDate), ref _installationDate, value); }

        private DateTime? _warrantyEndDate;
        [XafDisplayName("Garanti Bitiş Tarihi")]
        public DateTime? WarrantyEndDate { get => _warrantyEndDate; set => SetPropertyValue(nameof(WarrantyEndDate), ref _warrantyEndDate, value); }

        private string _location;
        [XafDisplayName("Lokasyon")]
        public string Location { get => _location; set => SetPropertyValue(nameof(Location), ref _location, value); }

        private EquipmentStatus _status;
        [XafDisplayName("Durum")]
        public EquipmentStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        private string _notes;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Notlar")]
        public string Notes { get => _notes; set => SetPropertyValue(nameof(Notes), ref _notes, value); }

        [Association("Equipment-ServiceTickets")]
        [XafDisplayName("Servis Talepleri")]
        public XPCollection<ServiceTicket> ServiceTickets => GetCollection<ServiceTicket>(nameof(ServiceTickets));

        [Association("Equipment-MaintenancePlans")]
        [XafDisplayName("Bakım Planları")]
        public XPCollection<MaintenancePlan> MaintenancePlans => GetCollection<MaintenancePlan>(nameof(MaintenancePlans));

        [Association("Equipment-MaintenanceLogs")]
        [XafDisplayName("Bakım Kayıtları")]
        public XPCollection<MaintenanceLog> MaintenanceLogs => GetCollection<MaintenanceLog>(nameof(MaintenanceLogs));

        public override void AfterConstruction() { base.AfterConstruction(); Status = EquipmentStatus.Active; }
    }

    public enum EquipmentStatus
    {
        [Display(Name = "Aktif")] Active = 0,
        [Display(Name = "Pasif")] Inactive = 1,
        [Display(Name = "Bakımda")] UnderMaintenance = 2,
        [Display(Name = "Arızalı")] OutOfOrder = 3,
        [Display(Name = "Hurda")] Scrapped = 4
    }
}
