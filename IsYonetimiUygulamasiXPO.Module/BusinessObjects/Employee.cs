using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl.Xpo; // PermissionPolicyUser için
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("İnsan Kaynakları")]
    [DisplayName("Personel (XPO)")]
    public class Employee : XPObject
    {
        public Employee(Session session) : base(session) { }

        private string _employeeNumber;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Personel Numarası")]
        public string EmployeeNumber { get => _employeeNumber; set => SetPropertyValue(nameof(EmployeeNumber), ref _employeeNumber, value); }

        private string _firstName;
        [XafDisplayName("Adı")]
        public string FirstName { get => _firstName; set => SetPropertyValue(nameof(FirstName), ref _firstName, value); }

        private string _lastName;
        [XafDisplayName("Soyadı")]
        public string LastName { get => _lastName; set => SetPropertyValue(nameof(LastName), ref _lastName, value); }

        [PersistentAlias("Concat(FirstName, ' ', LastName)")]
        [XafDisplayName("Tam Adı")]
        public string FullName => (string)EvaluateAlias(nameof(FullName));

        private string _nationalId;
        [Size(11)]
        [XafDisplayName("TC Kimlik Numarası")]
        public string NationalId { get => _nationalId; set => SetPropertyValue(nameof(NationalId), ref _nationalId, value); }

        private DateTime? _dateOfBirth;
        [XafDisplayName("Doğum Tarihi")]
        public DateTime? DateOfBirth { get => _dateOfBirth; set => SetPropertyValue(nameof(DateOfBirth), ref _dateOfBirth, value); }

        private Gender _gender;
        [XafDisplayName("Cinsiyet")]
        public Gender Gender { get => _gender; set => SetPropertyValue(nameof(Gender), ref _gender, value); }

        private string _departmentName;
        [XafDisplayName("Departman")]
        public string DepartmentName { get => _departmentName; set => SetPropertyValue(nameof(DepartmentName), ref _departmentName, value); }

        private string _positionTitle;
        [XafDisplayName("Pozisyon/Unvan")]
        public string PositionTitle { get => _positionTitle; set => SetPropertyValue(nameof(PositionTitle), ref _positionTitle, value); }

        private DateTime? _hireDate;
        [XafDisplayName("İşe Başlama Tarihi")]
        public DateTime? HireDate { get => _hireDate; set => SetPropertyValue(nameof(HireDate), ref _hireDate, value); }

        private DateTime? _terminationDate;
        [XafDisplayName("İşten Ayrılma Tarihi")]
        public DateTime? TerminationDate { get => _terminationDate; set => SetPropertyValue(nameof(TerminationDate), ref _terminationDate, value); }

        private EmployeeStatus _status;
        [XafDisplayName("Durumu")]
        public EmployeeStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        private string _phoneNumber;
        [Size(20)]
        [XafDisplayName("Telefon")]
        public string PhoneNumber { get => _phoneNumber; set => SetPropertyValue(nameof(PhoneNumber), ref _phoneNumber, value); }

        private string _email;
        [Size(100)]
        [XafDisplayName("E-posta")]
        public string Email { get => _email; set => SetPropertyValue(nameof(Email), ref _email, value); }

        private string _address;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Adres")]
        public string Address { get => _address; set => SetPropertyValue(nameof(Address), ref _address, value); }

        private PermissionPolicyUser _systemUser;
        [XafDisplayName("Sistem Kullanıcısı")]
        public PermissionPolicyUser SystemUser
        {
            get => _systemUser;
            set => SetPropertyValue(nameof(SystemUser), ref _systemUser, value);
        }

        [Association("Employee-LeaveRequests")]
        [XafDisplayName("İzin Talepleri")]
        public XPCollection<LeaveRequest> LeaveRequests => GetCollection<LeaveRequest>(nameof(LeaveRequests));

        [Association("Employee-TimesheetEntries")]
        [XafDisplayName("Puantaj Kayıtları")]
        public XPCollection<TimesheetEntry> TimesheetEntries => GetCollection<TimesheetEntry>(nameof(TimesheetEntries));

        [Association("Employee-FixedAssetsResponsible")] // Sorumlu olduğu demirbaşlar
        [XafDisplayName("Sorumlu Olduğu Demirbaşlar")]
        public XPCollection<FixedAsset> ResponsibleFixedAssets => GetCollection<FixedAsset>(nameof(ResponsibleFixedAssets));

        [Association("Employee-QualityChecksBy")] // Yaptığı kalite kontrolleri
        [XafDisplayName("Yaptığı Kalite Kontrolleri")]
        public XPCollection<QualityCheck> QualityChecksPerformed => GetCollection<QualityCheck>(nameof(QualityChecksPerformed));

        [Association("Employee-NonConformancesResponsible")] // Sorumlu olduğu uygunsuzluklar
        [XafDisplayName("Sorumlu Olduğu Uygunsuzluklar")]
        public XPCollection<NonConformance> ResponsibleNonConformances => GetCollection<NonConformance>(nameof(ResponsibleNonConformances));

        // Meeting (Event) nesnesindeki Resources (katılımcılar) ile ilişki kurulacaksa,
        // Event nesnesinde Employee tipinde bir Resource tanımı yapılabilir veya Meeting nesnesinde direkt Employee koleksiyonu tutulabilir.
        // Şimdilik Meeting nesnesinde Employee koleksiyonu tutacağımızı varsayalım.
        [Association("Meeting-Attendees")]
        [XafDisplayName("Katıldığı Toplantılar")]
        public XPCollection<Meeting> MeetingsAttended => GetCollection<Meeting>(nameof(MeetingsAttended));

        [Association("Employee-StockCountsBy")] // Yaptığı stok sayımları
        [XafDisplayName("Yaptığı Stok Sayımları")]
        public XPCollection<StockCount> StockCountsPerformed => GetCollection<StockCount>(nameof(StockCountsPerformed));

        [Association("MaintenanceLog-Technician")] // Yaptığı bakımlar
        [XafDisplayName("Yaptığı Bakımlar")]
        public XPCollection<MaintenanceLog> MaintenanceLogsPerformed => GetCollection<MaintenanceLog>(nameof(MaintenanceLogsPerformed));


        public override void AfterConstruction() { base.AfterConstruction(); Status = EmployeeStatus.Active; }
    }

    public enum Gender
    {
        [Display(Name = "Belirtilmemiş")] Unspecified = 0,
        [Display(Name = "Erkek")] Male = 1,
        [Display(Name = "Kadın")] Female = 2
    }
    public enum EmployeeStatus
    {
        [Display(Name = "Aktif")] Active = 0,
        [Display(Name = "İzinli")] OnLeave = 1,
        [Display(Name = "Ayrıldı")] Terminated = 2
    }
}
