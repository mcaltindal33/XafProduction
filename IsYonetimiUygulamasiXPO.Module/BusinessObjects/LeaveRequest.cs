using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("İnsan Kaynakları")]
    [DisplayName("İzin Talebi (XPO)")]
    public class LeaveRequest : XPObject
    {
        public LeaveRequest(Session session) : base(session) { }

        private Employee _employee;
        [Association("Employee-LeaveRequests")]
        [XafDisplayName("Personel")]
        public Employee Employee { get => _employee; set => SetPropertyValue(nameof(Employee), ref _employee, value); }

        private string _leaveType;
        [XafDisplayName("İzin Tipi")] // Örn: Yıllık İzin, Hastalık İzni, Mazeret İzni vb.
        public string LeaveType { get => _leaveType; set => SetPropertyValue(nameof(LeaveType), ref _leaveType, value); }

        private DateTime _startDate;
        [XafDisplayName("Başlangıç Tarihi")]
        public DateTime StartDate { get => _startDate; set => SetPropertyValue(nameof(StartDate), ref _startDate, value); }

        private DateTime _endDate;
        [XafDisplayName("Bitiş Tarihi")]
        public DateTime EndDate { get => _endDate; set => SetPropertyValue(nameof(EndDate), ref _endDate, value); }

        // TimeSpan.TotalDays kullanmak daha doğru olabilir, ancak PersistentAlias basit bir çıkarma yapar.
        // Eğer iş günleri vs. hesabı gerekiyorsa NonPersistent bir property ve getter'da hesaplama daha iyi olur.
        [PersistentAlias("EndDate - StartDate")] // Bu TimeSpan döndürür, XAF bunu nasıl gösterir kontrol edilmeli.
                                                 // Daha iyisi: AddDays(1) ile gün sayısı.
        [XafDisplayName("İzin Süresi (Gün)")]
        public TimeSpan DurationTimeSpan => (TimeSpan)EvaluateAlias(nameof(DurationTimeSpan));

        [NonPersistent]
        [XafDisplayName("İzin Süresi (Gün Sayısı)")]
        public int DurationInDays => DurationTimeSpan.Days + 1;


        private string _reason;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Açıklama")]
        public string Reason { get => _reason; set => SetPropertyValue(nameof(Reason), ref _reason, value); }

        private DateTime _requestDate;
        [XafDisplayName("Talep Tarihi")]
        public DateTime RequestDate { get => _requestDate; set => SetPropertyValue(nameof(RequestDate), ref _requestDate, value); }

        private ApprovalStatus _status;
        [XafDisplayName("Onay Durumu")]
        public ApprovalStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        private Employee _approvedBy;
        [XafDisplayName("Onaylayan Yetkili")]
        public Employee ApprovedBy { get => _approvedBy; set => SetPropertyValue(nameof(ApprovedBy), ref _approvedBy, value); }

        private DateTime? _approvalDate;
        [XafDisplayName("Onay Tarihi")]
        public DateTime? ApprovalDate { get => _approvalDate; set => SetPropertyValue(nameof(ApprovalDate), ref _approvalDate, value); }

        public override void AfterConstruction() { base.AfterConstruction(); RequestDate = DateTime.Now; Status = ApprovalStatus.Pending; StartDate = DateTime.Today; EndDate = DateTime.Today.AddDays(1); }
    }

    public enum ApprovalStatus
    {
        [Display(Name = "Beklemede")] Pending = 0,
        [Display(Name = "Onaylandı")] Approved = 1,
        [Display(Name = "Reddedildi")] Rejected = 2,
        [Display(Name = "İptal Edildi")] Cancelled = 3
    }
}
