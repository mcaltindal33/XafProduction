using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
// using DevExpress.Persistent.Validation;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Satış Pazarlama")]
    [DisplayName("Potansiyel Müşteri (XPO)")]
    public class Lead : XPObject
    {
        public Lead(Session session) : base(session) { }

        private string _leadName;
        [Size(100)]
        // [RuleRequiredField("RuleRequiredField for Lead.LeadName", DefaultContexts.Save)]
        [XafDisplayName("Potansiyel Müşteri Adı")]
        public string LeadName
        {
            get => _leadName;
            set => SetPropertyValue(nameof(LeadName), ref _leadName, value);
        }

        private string _companyName;
        [Size(100)]
        [XafDisplayName("Firma Adı")]
        public string CompanyName
        {
            get => _companyName;
            set => SetPropertyValue(nameof(CompanyName), ref _companyName, value);
        }

        private string _contactPerson;
        [Size(100)]
        [XafDisplayName("Yetkili Kişi")]
        public string ContactPerson
        {
            get => _contactPerson;
            set => SetPropertyValue(nameof(ContactPerson), ref _contactPerson, value);
        }

        private string _phoneNumber;
        [Size(20)]
        [XafDisplayName("Telefon")]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetPropertyValue(nameof(PhoneNumber), ref _phoneNumber, value);
        }

        private string _email;
        [Size(100)]
        [XafDisplayName("E-posta")]
        public string Email
        {
            get => _email;
            set => SetPropertyValue(nameof(Email), ref _email, value);
        }

        private string _source;
        [XafDisplayName("Kaynak")]
        public string Source
        {
            get => _source;
            set => SetPropertyValue(nameof(Source), ref _source, value);
        }

        private LeadStatus _status;
        [XafDisplayName("Durum")]
        public LeadStatus Status
        {
            get => _status;
            set => SetPropertyValue(nameof(Status), ref _status, value);
        }

        private string _description;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Açıklama")]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        private DateTime _createdDate;
        [XafDisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedDate
        {
            get => _createdDate;
            set => SetPropertyValue(nameof(CreatedDate), ref _createdDate, value);
        }

        private bool _isConvertedToCustomer;
        [XafDisplayName("Müşteriye Dönüştürüldü")]
        public bool IsConvertedToCustomer
        {
            get => _isConvertedToCustomer;
            set => SetPropertyValue(nameof(IsConvertedToCustomer), ref _isConvertedToCustomer, value);
        }

        private Customer _convertedCustomer;
        [Association("Customer-Leads")] // Customer'daki XPCollection ile eşleşir
        [XafDisplayName("Dönüştürülen Müşteri")]
        public Customer ConvertedCustomer
        {
            get => _convertedCustomer;
            set => SetPropertyValue(nameof(ConvertedCustomer), ref _convertedCustomer, value);
        }

        [Association("Lead-SalesOpportunities")]
        [XafDisplayName("Satış Fırsatları")]
        public XPCollection<SalesOpportunity> SalesOpportunities => GetCollection<SalesOpportunity>(nameof(SalesOpportunities));

        [Association("Lead-Offers")]
        [XafDisplayName("Teklifler")]
        public XPCollection<Offer> Offers => GetCollection<Offer>(nameof(Offers));


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = DateTime.Now;
            Status = LeadStatus.New;
        }
    }

    public enum LeadStatus
    {
        [Display(Name = "Yeni")] New = 0,
        [Display(Name = "İletişime Geçildi")] Contacted = 1,
        [Display(Name = "Değerlendiriliyor")] Qualified = 2,
        [Display(Name = "Teklif Verildi")] ProposalSent = 3,
        [Display(Name = "Kazanılamadı")] Lost = 4,
        [Display(Name = "Müşteriye Dönüştü")] Converted = 5
    }
}
