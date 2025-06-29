using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DevExpress.Persistent.BaseImpl.Xpo; // PermissionPolicyUser için

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Satış Pazarlama")]
    [DisplayName("Satış Fırsatı (XPO)")]
    public class SalesOpportunity : XPObject
    {
        public SalesOpportunity(Session session) : base(session) { }

        private string _opportunityCode;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Fırsat Kodu")]
        public string OpportunityCode
        {
            get => _opportunityCode;
            set => SetPropertyValue(nameof(OpportunityCode), ref _opportunityCode, value);
        }

        private string _opportunityName;
        [Size(255)]
        [XafDisplayName("Fırsat Adı")]
        public string OpportunityName
        {
            get => _opportunityName;
            set => SetPropertyValue(nameof(OpportunityName), ref _opportunityName, value);
        }

        private Customer _customer;
        [Association("Customer-SalesOpportunities")]
        [XafDisplayName("Müşteri")]
        public Customer Customer
        {
            get => _customer;
            set => SetPropertyValue(nameof(Customer), ref _customer, value);
        }

        private Lead _lead;
        [Association("Lead-SalesOpportunities")]
        [XafDisplayName("Potansiyel Müşteri")]
        public Lead Lead
        {
            get => _lead;
            set => SetPropertyValue(nameof(Lead), ref _lead, value);
        }

        private DateTime _estimatedCloseDate;
        [XafDisplayName("Tahmini Kapanış Tarihi")]
        public DateTime EstimatedCloseDate
        {
            get => _estimatedCloseDate;
            set => SetPropertyValue(nameof(EstimatedCloseDate), ref _estimatedCloseDate, value);
        }

        private decimal _estimatedAmount;
        [XafDisplayName("Fırsat Tutarı")]
        public decimal EstimatedAmount
        {
            get => _estimatedAmount;
            set => SetPropertyValue(nameof(EstimatedAmount), ref _estimatedAmount, value);
        }

        private int _probability;
        [XafDisplayName("Olasılık (%)")]
        public int Probability
        {
            get => _probability;
            set => SetPropertyValue(nameof(Probability), ref _probability, value);
        }

        private OpportunityStage _stage;
        [XafDisplayName("Aşama")]
        public OpportunityStage Stage
        {
            get => _stage;
            set => SetPropertyValue(nameof(Stage), ref _stage, value);
        }

        private PermissionPolicyUser _salesRepresentative; // XPO User
        [XafDisplayName("Sorumlu Satışçı")]
        public PermissionPolicyUser SalesRepresentative
        {
            get => _salesRepresentative;
            set => SetPropertyValue(nameof(SalesRepresentative), ref _salesRepresentative, value);
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

        [Association("SalesOpportunity-Offers")]
        [XafDisplayName("Teklifler")]
        public XPCollection<Offer> Offers => GetCollection<Offer>(nameof(Offers));

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = DateTime.Now;
            Stage = OpportunityStage.Qualification;
            EstimatedCloseDate = DateTime.Now.AddMonths(1);
        }
    }

    public enum OpportunityStage
    {
        [Display(Name = "Niteleniyor")] Qualification = 0,
        [Display(Name = "İhtiyaç Analizi")] NeedsAnalysis = 1,
        [Display(Name = "Teklif Aşaması")] Proposal = 2,
        [Display(Name = "Müzakere")] Negotiation = 3,
        [Display(Name = "Kazanıldı")] Won = 4,
        [Display(Name = "Kaybedildi")] Lost = 5
    }
}
