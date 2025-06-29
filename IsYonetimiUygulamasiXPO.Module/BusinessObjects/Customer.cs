using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
// using DevExpress.Persistent.Validation; // Validation modülü eklenince aktifleşir

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Müşteri Yönetimi")]
    [DisplayName("Müşteri (XPO)")]
    public class Customer : XPObject
    {
        public Customer(Session session) : base(session) { }

        private string _customerCode;
        [Size(20)]
        // [RuleRequiredField("RuleRequiredField for Customer.CustomerCode", DefaultContexts.Save)]
        // [RuleUniqueValue("CustomerCode_IsUnique_XPO", DefaultContexts.Save)]
        [Indexed(Unique = true)]
        [XafDisplayName("Müşteri Kodu"), ToolTip("Müşteri kodunu giriniz")]
        public string CustomerCode
        {
            get => _customerCode;
            set => SetPropertyValue(nameof(CustomerCode), ref _customerCode, value);
        }

        private string _companyName;
        [Size(255)]
        // [RuleRequiredField("RuleRequiredField for Customer.CompanyName", DefaultContexts.Save)]
        [XafDisplayName("Firma Adı"), ToolTip("Firma adını giriniz")]
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

        private string _address;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Adres")]
        public string Address
        {
            get => _address;
            set => SetPropertyValue(nameof(Address), ref _address, value);
        }

        [Association("Customer-Leads")]
        [XafDisplayName("Potansiyel Müşterileri (Bu Müşteriye Dönüşenler)")]
        public XPCollection<Lead> ConvertedLeads => GetCollection<Lead>(nameof(ConvertedLeads));

        [Association("Customer-SalesOpportunities")]
        [XafDisplayName("Satış Fırsatları")]
        public XPCollection<SalesOpportunity> SalesOpportunities => GetCollection<SalesOpportunity>(nameof(SalesOpportunities));

        [Association("Customer-Offers")]
        [XafDisplayName("Teklifler")]
        public XPCollection<Offer> Offers => GetCollection<Offer>(nameof(Offers));

        [Association("Customer-Shipments")]
        [XafDisplayName("Sevkiyatlar")]
        public XPCollection<Shipment> Shipments => GetCollection<Shipment>(nameof(Shipments));

        [Association("Customer-Invoices")]
        [XafDisplayName("Faturalar")]
        public XPCollection<Invoice> Invoices => GetCollection<Invoice>(nameof(Invoices));

        [Association("Customer-Equipments")]
        [XafDisplayName("Ekipmanları")]
        public XPCollection<Equipment> Equipments => GetCollection<Equipment>(nameof(Equipments));

        [Association("Customer-ServiceTickets")]
        [XafDisplayName("Servis Talepleri")]
        public XPCollection<ServiceTicket> ServiceTickets => GetCollection<ServiceTicket>(nameof(ServiceTickets));

        [Association("Customer-ServiceContracts")]
        [XafDisplayName("Hizmet Sözleşmeleri")]
        public XPCollection<ServiceContract> ServiceContracts => GetCollection<ServiceContract>(nameof(ServiceContracts));


        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}
