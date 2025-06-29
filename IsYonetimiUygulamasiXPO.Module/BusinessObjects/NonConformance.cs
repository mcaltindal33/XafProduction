using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Kalite Yönetimi")]
    [DisplayName("Uygunsuzluk Kaydı (XPO)")]
    public class NonConformance : XPObject
    {
        public NonConformance(Session session) : base(session) { }

        private string _ncNumber;
        [Size(20)]
        [Indexed(Unique = true)] // Otomatik numara için Controller eklenecek
        [XafDisplayName("Uygunsuzluk No")]
        public string NcNumber { get => _ncNumber; set => SetPropertyValue(nameof(NcNumber), ref _ncNumber, value); }

        private DateTime _detectionDate;
        [XafDisplayName("Tespit Tarihi")]
        public DateTime DetectionDate { get => _detectionDate; set => SetPropertyValue(nameof(DetectionDate), ref _detectionDate, value); }

        private QualityCheck _relatedQualityCheck;
        [Association("QualityCheck-NonConformances")]
        [XafDisplayName("İlgili Kalite Kontrol Kaydı")]
        public QualityCheck RelatedQualityCheck { get => _relatedQualityCheck; set => SetPropertyValue(nameof(RelatedQualityCheck), ref _relatedQualityCheck, value); }

        private string _description;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Uygunsuzluk Açıklaması")]
        public string Description { get => _description; set => SetPropertyValue(nameof(Description), ref _description, value); }

        private string _rootCauseAnalysis;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Kök Neden Analizi")]
        public string RootCauseAnalysis { get => _rootCauseAnalysis; set => SetPropertyValue(nameof(RootCauseAnalysis), ref _rootCauseAnalysis, value); }

        private string _correctiveAction;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Düzeltici Faaliyet")]
        public string CorrectiveAction { get => _correctiveAction; set => SetPropertyValue(nameof(CorrectiveAction), ref _correctiveAction, value); }

        private string _preventiveAction;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Önleyici Faaliyet")]
        public string PreventiveAction { get => _preventiveAction; set => SetPropertyValue(nameof(PreventiveAction), ref _preventiveAction, value); }

        private Employee _responsiblePerson;
        [Association("Employee-NonConformancesResponsible")]
        [XafDisplayName("Sorumlu Kişi")]
        public Employee ResponsiblePerson { get => _responsiblePerson; set => SetPropertyValue(nameof(ResponsiblePerson), ref _responsiblePerson, value); }

        private DateTime? _dueDate;
        [XafDisplayName("Termin Tarihi")]
        public DateTime? DueDate { get => _dueDate; set => SetPropertyValue(nameof(DueDate), ref _dueDate, value); }

        private NonConformanceStatus _status;
        [XafDisplayName("Durum")]
        public NonConformanceStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        public override void AfterConstruction() {
            base.AfterConstruction();
            DetectionDate = DateTime.Now;
            Status = NonConformanceStatus.Open;
        }
    }

    public enum NonConformanceStatus
    {
        [Display(Name = "Açık")] Open = 0,
        [Display(Name = "İnceleniyor")] InProgress = 1,
        [Display(Name = "Çözüldü")] Resolved = 2,
        [Display(Name = "Doğrulandı ve Kapatıldı")] ClosedVerified = 3,
        [Display(Name = "İptal Edildi")] Cancelled = 4
    }
}
