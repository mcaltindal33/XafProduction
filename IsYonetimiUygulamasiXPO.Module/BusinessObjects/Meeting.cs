using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.Xpo; // Event için
using DevExpress.Xpo;
using System.ComponentModel;
using System.Collections.Generic; // IList için

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Organizasyon")]
    [DisplayName("Toplantı (XPO)")]
    // Event sınıfı zaten XPObject'tan (veya XPCustomObject'tan) türemiştir.
    public class Meeting : Event
    {
        public Meeting(Session session) : base(session) { }

        // Event sınıfında Subject, Location, StartOn, EndOn, AllDay, Description, Label, Status, Reminder gibi alanlar zaten var.
        // Bunları kullanabilir veya XafDisplayName ile etiketlerini değiştirebiliriz.
        // Veya ek alanlar ekleyebiliriz.

        // Örneğin, Event.Subject yerine daha spesifik bir alan adı kullanmak istersek:
        private string _meetingTitle;
        [XafDisplayName("Toplantı Başlığı")] // Event.Subject yerine bu görünebilir (Model Editor'den ayarlanabilir)
        public string MeetingTitle
        {
            get => _meetingTitle;
            set => SetPropertyValue(nameof(MeetingTitle), ref _meetingTitle, value);
        }

        private string _agenda;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Gündem")]
        public string Agenda
        {
            get => _agenda;
            set => SetPropertyValue(nameof(Agenda), ref _agenda, value);
        }

        private string _minutes; // Toplantı tutanağı
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Toplantı Notları/Tutanağı")]
        public string Minutes
        {
            get => _minutes;
            set => SetPropertyValue(nameof(Minutes), ref _minutes, value);
        }

        // Event nesnesinde Resources özelliği zaten var. Bu, katılımcıları (örn: Employee, Contact)
        // veya toplantı odası gibi kaynakları temsil etmek için kullanılabilir.
        // Eğer Employee nesnelerini doğrudan katılımcı olarak saklamak istiyorsak:
        [Association("Meeting-Attendees")]
        [XafDisplayName("Katılımcı Personeller")]
        public XPCollection<Employee> AttendeesEmployee => GetCollection<Employee>(nameof(AttendeesEmployee));


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Varsayılan değerler
            Label = 0; // Renk etiketi (0-10)
            Status = 1; // AppointmentStatus (0: Free, 1: Tentative, 2: Busy, 3: OutOfOffice, 4: WorkingElsewhere)
            // StartOn ve EndOn kullanıcı tarafından girilmeli veya varsayılan atanabilir.
            StartOn = System.DateTime.Now.AddHours(1);
            EndOn = System.DateTime.Now.AddHours(2);
        }

        // Event.Subject'ı MeetingTitle ile senkronize tutmak istersek (opsiyonel):
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == nameof(MeetingTitle))
                {
                    Subject = MeetingTitle; // Event'in Subject'ini güncelle
                }
                else if (propertyName == nameof(Subject) && MeetingTitle != Subject)
                {
                    MeetingTitle = Subject; // Eğer Subject dışarıdan değişirse MeetingTitle'ı güncelle
                }
            }
        }
    }
}
