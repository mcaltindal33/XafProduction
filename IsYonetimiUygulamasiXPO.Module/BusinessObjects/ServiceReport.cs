using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Teknik Servis")]
    [DisplayName("Servis Raporu (XPO)")]
    public class ServiceReport : XPObject
    {
        public ServiceReport(Session session) : base(session) { }

        private string _reportNumber;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Rapor Numarası")]
        public string ReportNumber { get => _reportNumber; set => SetPropertyValue(nameof(ReportNumber), ref _reportNumber, value); }

        private ServiceTicket _serviceTicket;
        // ServiceTicket'taki Association ile eşleşmeli.
        // Eğer ServiceTicket silindiğinde bu raporun da silinmesini istiyorsak,
        // ServiceTicket tarafındaki Association'da Aggregated olmalı.
        // Ya da burada Association'ı tanımlayıp, ServiceTicket'taki property'i bu Association'a bağlarız.
        [Association("ServiceTicket-ServiceReportOneToOne")]
        [XafDisplayName("İlişkili Servis Talebi")]
        public ServiceTicket ServiceTicket
        {
            get => _serviceTicket;
            set => SetPropertyValue(nameof(ServiceTicket), ref _serviceTicket, value);
        }

        private DateTime _reportDate;
        [XafDisplayName("Rapor Tarihi")]
        public DateTime ReportDate { get => _reportDate; set => SetPropertyValue(nameof(ReportDate), ref _reportDate, value); }

        private string _technicianNotes;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Yapılan İşlemler / Teknisyen Notları")]
        public string TechnicianNotes { get => _technicianNotes; set => SetPropertyValue(nameof(TechnicianNotes), ref _technicianNotes, value); }

        private string _usedMaterials;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Kullanılan Malzemeler")] // Daha sonra XPCollection<MaterialUsage> gibi bir yapıya dönüşebilir
        public string UsedMaterials { get => _usedMaterials; set => SetPropertyValue(nameof(UsedMaterials), ref _usedMaterials, value); }

        private double _totalHoursWorked;
        [XafDisplayName("Toplam Çalışma Süresi (saat)")]
        public double TotalHoursWorked { get => _totalHoursWorked; set => SetPropertyValue(nameof(TotalHoursWorked), ref _totalHoursWorked, value); }

        private bool _customerApproved;
        [XafDisplayName("Müşteri Onayı")]
        public bool CustomerApproved { get => _customerApproved; set => SetPropertyValue(nameof(CustomerApproved), ref _customerApproved, value); }

        private string _customerApproverName;
        [XafDisplayName("Müşteri Adı Soyadı (Onaylayan)")]
        public string CustomerApproverName { get => _customerApproverName; set => SetPropertyValue(nameof(CustomerApproverName), ref _customerApproverName, value); }

        public override void AfterConstruction() { base.AfterConstruction(); ReportDate = DateTime.Now; }
    }
}
