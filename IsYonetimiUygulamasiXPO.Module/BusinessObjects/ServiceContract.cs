using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Hizmet Yönetimi")]
    [DisplayName("Hizmet Sözleşmesi (XPO)")]
    public class ServiceContract : XPObject
    {
        public ServiceContract(Session session) : base(session) { }

        private string _contractNumber;
        [Size(20)]
        [Indexed(Unique = true)] // Otomatik numara için Controller eklenecek
        [XafDisplayName("Sözleşme Numarası")]
        public string ContractNumber { get => _contractNumber; set => SetPropertyValue(nameof(ContractNumber), ref _contractNumber, value); }

        private Customer _customer;
        [Association("Customer-ServiceContracts")]
        [XafDisplayName("Müşteri")]
        public Customer Customer { get => _customer; set => SetPropertyValue(nameof(Customer), ref _customer, value); }

        private DateTime _startDate;
        [XafDisplayName("Başlangıç Tarihi")]
        public DateTime StartDate { get => _startDate; set => SetPropertyValue(nameof(StartDate), ref _startDate, value); }

        private DateTime _endDate;
        [XafDisplayName("Bitiş Tarihi")]
        public DateTime EndDate { get => _endDate; set => SetPropertyValue(nameof(EndDate), ref _endDate, value); }

        private decimal _contractAmount;
        [XafDisplayName("Sözleşme Tutarı")]
        // Bu alan, ContractItems'dan hesaplanabilir veya manuel girilebilir.
        // PersistentAlias ile: [PersistentAlias("ContractItems.Sum(AgreedPrice * Quantity)")]
        public decimal ContractAmount { get => _contractAmount; set => SetPropertyValue(nameof(ContractAmount), ref _contractAmount, value); }

        private string _description;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Açıklama")]
        public string Description { get => _description; set => SetPropertyValue(nameof(Description), ref _description, value); }

        private ContractStatus _status;
        [XafDisplayName("Durum")]
        public ContractStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        [Association("ServiceContract-ContractItems"), Aggregated]
        [XafDisplayName("Sözleşme Kapsamındaki Hizmetler")]
        public XPCollection<ServiceContractItem> ContractItems => GetCollection<ServiceContractItem>(nameof(ContractItems));

        public override void AfterConstruction() {
            base.AfterConstruction();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddYears(1);
            Status = ContractStatus.Active;
        }
    }

    public enum ContractStatus
    {
        [Display(Name = "Taslak")] Draft = 0,
        [Display(Name = "Aktif")] Active = 1,
        [Display(Name = "Süresi Doldu")] Expired = 2,
        [Display(Name = "İptal Edildi")] Cancelled = 3
    }
}
