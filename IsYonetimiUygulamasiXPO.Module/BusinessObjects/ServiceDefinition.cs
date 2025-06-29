using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Hizmet Yönetimi")]
    [DisplayName("Hizmet Tanımı (XPO)")]
    public class ServiceDefinition : XPObject
    {
        public ServiceDefinition(Session session) : base(session) { }

        private string _serviceCode;
        [Size(50)]
        [Indexed(Unique = true)]
        [XafDisplayName("Hizmet Kodu")]
        public string ServiceCode { get => _serviceCode; set => SetPropertyValue(nameof(ServiceCode), ref _serviceCode, value); }

        private string _serviceName;
        [XafDisplayName("Hizmet Adı")]
        public string ServiceName { get => _serviceName; set => SetPropertyValue(nameof(ServiceName), ref _serviceName, value); }

        private string _description;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Açıklama")]
        public string Description { get => _description; set => SetPropertyValue(nameof(Description), ref _description, value); }

        private string _category;
        [XafDisplayName("Kategori")]
        public string Category { get => _category; set => SetPropertyValue(nameof(Category), ref _category, value); }

        private decimal _unitPrice;
        [XafDisplayName("Birim Fiyat")]
        public decimal UnitPrice { get => _unitPrice; set => SetPropertyValue(nameof(UnitPrice), ref _unitPrice, value); }

        private double _duration;
        [XafDisplayName("Süre")] // Hizmetin tipik süresi
        public double Duration { get => _duration; set => SetPropertyValue(nameof(Duration), ref _duration, value); }

        private DurationUnitType _durationUnit;
        [XafDisplayName("Süre Birimi")]
        public DurationUnitType DurationUnit { get => _durationUnit; set => SetPropertyValue(nameof(DurationUnit), ref _durationUnit, value); }

        [Association("ServiceDefinition-ContractItems")]
        [XafDisplayName("Kullanıldığı Sözleşme Kalemleri")]
        public XPCollection<ServiceContractItem> ServiceContractItems => GetCollection<ServiceContractItem>(nameof(ServiceContractItems));

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DurationUnit = DurationUnitType.Hour;
        }
    }

    public enum DurationUnitType
    {
        [Display(Name = "Saat")] Hour = 0,
        [Display(Name = "Gün")] Day = 1,
        [Display(Name = "Ay")] Month = 2,
        [Display(Name = "Yıl")] Year = 3
    }
}
