using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Model; // ModelDefault için

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("İnsan Kaynakları")]
    [DisplayName("Puantaj Kaydı (XPO)")]
    public class TimesheetEntry : XPObject
    {
        public TimesheetEntry(Session session) : base(session) { }

        private Employee _employee;
        [Association("Employee-TimesheetEntries")]
        [XafDisplayName("Personel")]
        public Employee Employee { get => _employee; set => SetPropertyValue(nameof(Employee), ref _employee, value); }

        private DateTime _entryDate;
        [XafDisplayName("Tarih")]
        public DateTime EntryDate { get => _entryDate; set => SetPropertyValue(nameof(EntryDate), ref _entryDate, value); }

        private DateTime? _clockInTime;
        [XafDisplayName("Giriş Saati")]
        [ModelDefault("DisplayFormat", "{0:HH:mm}")]
        [ModelDefault("EditMaskType", DevExpress.ExpressApp.Editors.EditMaskType.DateTime)] // EditMaskType
        [ModelDefault("EditMask", "HH:mm")]
        public DateTime? ClockInTime { get => _clockInTime; set => SetPropertyValue(nameof(ClockInTime), ref _clockInTime, value); }

        private DateTime? _clockOutTime;
        [XafDisplayName("Çıkış Saati")]
        [ModelDefault("DisplayFormat", "{0:HH:mm}")]
        [ModelDefault("EditMaskType", DevExpress.ExpressApp.Editors.EditMaskType.DateTime)] // EditMaskType
        [ModelDefault("EditMask", "HH:mm")]
        public DateTime? ClockOutTime { get => _clockOutTime; set => SetPropertyValue(nameof(ClockOutTime), ref _clockOutTime, value); }

        // PersistentAlias ile saat farkını hesaplama
        // Dikkat: ClockInTime ve ClockOutTime nullable olduğu için null kontrolü gerekir.
        // XPO'da IIF ve IsNullOrEmpty gibi fonksiyonlar PersistentAlias içinde kullanılabilir.
        [PersistentAlias("IIF(ClockOutTime Is Null Or ClockInTime Is Null, 0.0, TotalHours(ClockOutTime - ClockInTime))")]
        [XafDisplayName("Çalışma Süresi (Saat)")]
        public double HoursWorked => Convert.ToDouble(EvaluateAlias(nameof(HoursWorked)));

        private double _overtimeHours;
        [XafDisplayName("Mesai Süresi (Saat)")]
        public double OvertimeHours { get => _overtimeHours; set => SetPropertyValue(nameof(OvertimeHours), ref _overtimeHours, value); }

        private string _entryType; // Normal, Resmi Tatil, İzinli vb.
        [XafDisplayName("Kayıt Tipi")]
        public string EntryType { get => _entryType; set => SetPropertyValue(nameof(EntryType), ref _entryType, value); }

        public override void AfterConstruction() {
            base.AfterConstruction();
            EntryType = "Normal";
            EntryDate = DateTime.Today;
        }
    }
}
