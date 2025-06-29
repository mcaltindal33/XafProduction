using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Kalite Yönetimi")]
    [DisplayName("Kalite Kontrol Kaydı (XPO)")]
    public class QualityCheck : XPObject
    {
        public QualityCheck(Session session) : base(session) { }

        private string _checkNumber;
        [Size(20)]
        [Indexed(Unique = true)] // Otomatik numara için Controller eklenecek
        [XafDisplayName("Kontrol No")]
        public string CheckNumber { get => _checkNumber; set => SetPropertyValue(nameof(CheckNumber), ref _checkNumber, value); }

        private DateTime _checkDate;
        [XafDisplayName("Kontrol Tarihi")]
        public DateTime CheckDate { get => _checkDate; set => SetPropertyValue(nameof(CheckDate), ref _checkDate, value); }

        private Material _material;
        [Association("Material-QualityChecks")]
        [XafDisplayName("Kontrol Edilen Ürün/Malzeme")]
        public Material Material { get => _material; set => SetPropertyValue(nameof(Material), ref _material, value); }

        private ProductionOrderOperationLog _productionOperation;
        // ProductionOrderOperationLog'da QualityChecks koleksiyonu tanımlanabilir.
        [XafDisplayName("Kontrol Edilen Süreç/Operasyon")]
        public ProductionOrderOperationLog ProductionOperation { get => _productionOperation; set => SetPropertyValue(nameof(ProductionOperation), ref _productionOperation, value); }

        private string _checkpoint;
        [XafDisplayName("Kontrol Noktası/Aşaması")] // Giriş Kontrol, Proses Kontrol, Son Kontrol
        public string Checkpoint { get => _checkpoint; set => SetPropertyValue(nameof(Checkpoint), ref _checkpoint, value); }

        private Employee _checkedBy;
        [Association("Employee-QualityChecksBy")]
        [XafDisplayName("Kontrolü Yapan")]
        public Employee CheckedBy { get => _checkedBy; set => SetPropertyValue(nameof(CheckedBy), ref _checkedBy, value); }

        private QualityCheckResult _result;
        [XafDisplayName("Sonuç")] // Uygun, Şartlı Kabul, Red
        public QualityCheckResult Result { get => _result; set => SetPropertyValue(nameof(Result), ref _result, value); }

        private string _remarks;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Açıklamalar")]
        public string Remarks { get => _remarks; set => SetPropertyValue(nameof(Remarks), ref _remarks, value); }

        [Association("QualityCheck-NonConformances")]
        [XafDisplayName("İlişkili Uygunsuzluklar")]
        public XPCollection<NonConformance> NonConformances => GetCollection<NonConformance>(nameof(NonConformances));

        public override void AfterConstruction() {
            base.AfterConstruction();
            CheckDate = DateTime.Now;
            Result = QualityCheckResult.Pending;
        }
    }
    public enum QualityCheckResult {
        [Display(Name="Beklemede")] Pending,
        [Display(Name="Uygun")] Pass,
        [Display(Name="Şartlı Kabul")] ConditionalPass,
        [Display(Name="Red")] Fail
    }
}
