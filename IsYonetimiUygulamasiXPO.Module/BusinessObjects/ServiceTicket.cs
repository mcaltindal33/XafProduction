using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DevExpress.Persistent.BaseImpl.Xpo; // PermissionPolicyUser için

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Teknik Servis")]
    [DisplayName("Servis Talebi/İş Emri (XPO)")]
    public class ServiceTicket : XPObject
    {
        public ServiceTicket(Session session) : base(session) { }

        private string _ticketNumber;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Servis Talep No")]
        public string TicketNumber { get => _ticketNumber; set => SetPropertyValue(nameof(TicketNumber), ref _ticketNumber, value); }

        private DateTime _reportedDate;
        [XafDisplayName("Bildirim Tarihi")]
        public DateTime ReportedDate { get => _reportedDate; set => SetPropertyValue(nameof(ReportedDate), ref _reportedDate, value); }

        private Customer _customer;
        [Association("Customer-ServiceTickets")]
        [XafDisplayName("Müşteri")]
        public Customer Customer { get => _customer; set => SetPropertyValue(nameof(Customer), ref _customer, value); }

        private Equipment _equipment;
        [Association("Equipment-ServiceTickets")]
        [XafDisplayName("Ekipman")]
        public Equipment Equipment { get => _equipment; set => SetPropertyValue(nameof(Equipment), ref _equipment, value); }

        private string _issueDescription;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Sorun Açıklaması")]
        public string IssueDescription { get => _issueDescription; set => SetPropertyValue(nameof(IssueDescription), ref _issueDescription, value); }

        private ServiceTicketType _ticketType;
        [XafDisplayName("Talep Tipi")]
        public ServiceTicketType TicketType { get => _ticketType; set => SetPropertyValue(nameof(TicketType), ref _ticketType, value); }

        private ServiceTicketPriority _priority;
        [XafDisplayName("Öncelik")]
        public ServiceTicketPriority Priority { get => _priority; set => SetPropertyValue(nameof(Priority), ref _priority, value); }

        private ServiceTicketStatus _status;
        [XafDisplayName("Durum")]
        public ServiceTicketStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        private PermissionPolicyUser _assignedTechnician; // XPO User
        [XafDisplayName("Atanan Teknisyen")]
        public PermissionPolicyUser AssignedTechnician { get => _assignedTechnician; set => SetPropertyValue(nameof(AssignedTechnician), ref _assignedTechnician, value); }

        private DateTime? _scheduledDate;
        [XafDisplayName("Planlanan Servis Tarihi")]
        public DateTime? ScheduledDate { get => _scheduledDate; set => SetPropertyValue(nameof(ScheduledDate), ref _scheduledDate, value); }

        private DateTime? _completionDate;
        [XafDisplayName("Tamamlanma Tarihi")]
        public DateTime? CompletionDate { get => _completionDate; set => SetPropertyValue(nameof(CompletionDate), ref _completionDate, value); }

        // ServiceReport ile bire-bir ilişki için. ServiceReport'ta da ServiceTicket'a referans olacak.
        // Eğer ServiceReport silindiğinde ServiceTicket kalmalıysa Aggregated kullanmayız.
        // ServiceTicket silindiğinde ServiceReport silinsin istiyorsak ServiceReport'taki Association'da Aggregated olur.
        // Ya da ServiceReport'u ServiceTicket'ın bir parçası gibi yönetmek için burada Aggregated kullanılabilir.
        private ServiceReport _serviceReport;
        [Association("ServiceTicket-ServiceReportOneToOne"), Aggregated] // Bir talep bir rapor (genellikle)
        [XafDisplayName("Servis Raporu")]
        public ServiceReport ServiceReport
        {
            get => _serviceReport;
            set => SetPropertyValue(nameof(ServiceReport), ref _serviceReport, value);
        }

        [Association("ServiceTicket-MaintenanceLogs")]
        [XafDisplayName("İlişkili Bakım Kayıtları")]
        public XPCollection<MaintenanceLog> MaintenanceLogs => GetCollection<MaintenanceLog>(nameof(MaintenanceLogs));


        public override void AfterConstruction() {
            base.AfterConstruction();
            ReportedDate = DateTime.Now;
            TicketType = ServiceTicketType.Repair;
            Priority = ServiceTicketPriority.Medium;
            Status = ServiceTicketStatus.Open;
        }
    }

    public enum ServiceTicketType
    {
        [Display(Name = "Arıza Onarım")] Repair = 0,
        [Display(Name = "Periyodik Bakım")] Maintenance = 1,
        [Display(Name = "Kurulum")] Installation = 2,
        [Display(Name = "Danışmanlık")] Consultation = 3,
        [Display(Name = "Diğer")] Other = 4
    }

    public enum ServiceTicketPriority
    {
        [Display(Name = "Düşük")] Low = 0,
        [Display(Name = "Orta")] Medium = 1,
        [Display(Name = "Yüksek")] High = 2,
        [Display(Name = "Acil")] Urgent = 3
    }

    public enum ServiceTicketStatus
    {
        [Display(Name = "Açık")] Open = 0,
        [Display(Name = "Atandı")] Assigned = 1,
        [Display(Name = "Devam Ediyor")] InProgress = 2,
        [Display(Name = "Beklemede (Müşteri/Parça)")] OnHold = 3,
        [Display(Name = "Çözüldü")] Resolved = 4,
        [Display(Name = "Kapatıldı")] Closed = 5,
        [Display(Name = "İptal Edildi")] Cancelled = 6
    }
}
