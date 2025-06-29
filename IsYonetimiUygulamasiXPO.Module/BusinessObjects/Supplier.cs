using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
// using DevExpress.Persistent.Validation;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Satın Alma")]
    [DisplayName("Tedarikçi (XPO)")]
    public class Supplier : XPObject
    {
        public Supplier(Session session) : base(session) { }

        private string _supplierCode;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Tedarikçi Kodu")]
        public string SupplierCode
        {
            get => _supplierCode;
            set => SetPropertyValue(nameof(SupplierCode), ref _supplierCode, value);
        }

        private string _supplierName;
        [Size(255)]
        [XafDisplayName("Tedarikçi Adı")]
        public string SupplierName
        {
            get => _supplierName;
            set => SetPropertyValue(nameof(SupplierName), ref _supplierName, value);
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

        private string _taxNumber;
        [Size(11)]
        [XafDisplayName("Vergi Numarası")]
        public string TaxNumber
        {
            get => _taxNumber;
            set => SetPropertyValue(nameof(TaxNumber), ref _taxNumber, value);
        }

        private string _notes;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Notlar")]
        public string Notes
        {
            get => _notes;
            set => SetPropertyValue(nameof(Notes), ref _notes, value);
        }

        [Association("Supplier-PurchaseOrders")]
        [XafDisplayName("Satınalma Siparişleri")]
        public XPCollection<PurchaseOrder> PurchaseOrders => GetCollection<PurchaseOrder>(nameof(PurchaseOrders));

        [Association("Supplier-Invoices")]
        [XafDisplayName("Alış Faturaları")] // Supplier'a kesilen faturalar
        public XPCollection<Invoice> Invoices => GetCollection<Invoice>(nameof(Invoices));
    }
}
