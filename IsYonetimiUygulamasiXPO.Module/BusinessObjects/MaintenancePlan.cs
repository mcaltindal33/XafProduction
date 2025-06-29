using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Teknik Servis")]
    [DisplayName("Periyodik Bakım Planı (XPO)")]
    public class MaintenancePlan : XPObject
    {
        public MaintenancePlan(Session session) : base(session) { }

        private string _planName;
        [Size(100)] // EF Core'da 50 idi, XPO için biraz daha uzun olabilir.
        [XafDisplayName("Plan Adı")]
        public string PlanName { get => _planName; set => SetPropertyValue(nameof(PlanName), ref _planName, value); }

        private Equipment _equipment;
        [Association("Equipment-MaintenancePlans")]
        [XafDisplayName("Ekipman")]
        public Equipment Equipment { get => _equipment; set => SetPropertyValue(nameof(Equipment), ref _equipment, value); }

        private string _maintenanceType;
        [XafDisplayName("Bakım Tipi")] // Örn: Yağ Değişimi, Filtre Kontrolü
        public string MaintenanceType { get => _maintenanceType; set => SetPropertyValue(nameof(MaintenanceType), ref _maintenanceType, value); }

        private MaintenanceFrequency _frequency;
        [XafDisplayName("Sıklık")]
        public MaintenanceFrequency Frequency { get => _frequency; set => SetPropertyValue(nameof(Frequency), ref _frequency, value); }

        private int _frequencyValue;
        [XafDisplayName("Sıklık Değeri")] // Frequency'e göre (örn: Frequency = Ay ise Değer = 3 -> 3 Ayda bir)
        public int FrequencyValue { get => _frequencyValue; set => SetPropertyValue(nameof(FrequencyValue), ref _frequencyValue, value); }

        private DateTime? _lastMaintenanceDate;
        [XafDisplayName("Son Bakım Tarihi")]
        public DateTime? LastMaintenanceDate { get => _lastMaintenanceDate; set => SetPropertyValue(nameof(LastMaintenanceDate), ref _lastMaintenanceDate, value); }

        private DateTime? _nextDueDate;
        [XafDisplayName("Bir Sonraki Bakım Tarihi")]
        // Bu alan bir Controller veya OnChanged metodu ile otomatik hesaplanabilir.
        // Örneğin LastMaintenanceDate veya Frequency değiştiğinde.
        public DateTime? NextDueDate { get => _nextDueDate; set => SetPropertyValue(nameof(NextDueDate), ref _nextDueDate, value); }

        private string _description;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Açıklama")]
        public string Description { get => _description; set => SetPropertyValue(nameof(Description), ref _description, value); }

        private bool _isActive;
        [XafDisplayName("Aktif Plan")]
        public bool IsActive { get => _isActive; set => SetPropertyValue(nameof(IsActive), ref _isActive, value); }

        [Association("MaintenancePlan-MaintenanceLogs")]
        [XafDisplayName("Bu Plana Ait Bakım Kayıtları")]
        public XPCollection<MaintenanceLog> MaintenanceLogs => GetCollection<MaintenanceLog>(nameof(MaintenanceLogs));

        public override void AfterConstruction() { base.AfterConstruction(); FrequencyValue = 1; IsActive = true; }

        // NextDueDate'yi hesaplamak için bir metod (Controller'dan veya OnChanged'den çağrılabilir)
        public void CalculateNextDueDate()
        {
            if (LastMaintenanceDate.HasValue && IsActive)
            {
                DateTime newDueDate = LastMaintenanceDate.Value;
                switch (Frequency)
                {
                    case MaintenanceFrequency.Daily:
                        newDueDate = newDueDate.AddDays(FrequencyValue);
                        break;
                    case MaintenanceFrequency.Weekly:
                        newDueDate = newDueDate.AddDays(7 * FrequencyValue);
                        break;
                    case MaintenanceFrequency.Monthly:
                        newDueDate = newDueDate.AddMonths(FrequencyValue);
                        break;
                    case MaintenanceFrequency.Yearly:
                        newDueDate = newDueDate.AddYears(FrequencyValue);
                        break;
                    // ByOperationHours ve ByUsageCount için farklı bir mantık gerekir (sayaç takibi)
                }
                NextDueDate = newDueDate;
            }
            else
            {
                NextDueDate = null;
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
            {
                if (propertyName == nameof(LastMaintenanceDate) || propertyName == nameof(Frequency) || propertyName == nameof(FrequencyValue) || propertyName == nameof(IsActive))
                {
                    CalculateNextDueDate();
                }
            }
        }
    }

    public enum MaintenanceFrequency
    {
        [Display(Name = "Günlük")] Daily = 0,
        [Display(Name = "Haftalık")] Weekly = 1,
        [Display(Name = "Aylık")] Monthly = 2,
        [Display(Name = "Yıllık")] Yearly = 3,
        [Display(Name = "Çalışma Saatine Göre")] ByOperationHours = 4, // Bu tipler için sayaç takibi gerekir
        [Display(Name = "Kullanıma Göre")] ByUsageCount = 5 // Bu tipler için sayaç takibi gerekir
    }
}
