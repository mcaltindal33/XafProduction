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
    [DisplayName("Teklif (XPO)")]
    public class Offer : XPObject
    {
        public Offer(Session session) : base(session) { }

        private string _offerNumber;
        [Size(20)]
        [Indexed(Unique = true)]
        [XafDisplayName("Teklif Numarası")]
        public string OfferNumber
        {
            get => _offerNumber;
            set => SetPropertyValue(nameof(OfferNumber), ref _offerNumber, value);
        }

        private DateTime _offerDate;
        [XafDisplayName("Teklif Tarihi")]
        public DateTime OfferDate
        {
            get => _offerDate;
            set => SetPropertyValue(nameof(OfferDate), ref _offerDate, value);
        }

        private Customer _customer;
        [Association("Customer-Offers")]
        [XafDisplayName("Müşteri")]
        public Customer Customer
        {
            get => _customer;
            set => SetPropertyValue(nameof(Customer), ref _customer, value);
        }

        private Lead _lead;
        [Association("Lead-Offers")]
        [XafDisplayName("Potansiyel Müşteri")]
        public Lead Lead
        {
            get => _lead;
            set => SetPropertyValue(nameof(Lead), ref _lead, value);
        }

        private SalesOpportunity _salesOpportunity;
        [Association("SalesOpportunity-Offers")]
        [XafDisplayName("Satış Fırsatı")]
        public SalesOpportunity SalesOpportunity
        {
            get => _salesOpportunity;
            set => SetPropertyValue(nameof(SalesOpportunity), ref _salesOpportunity, value);
        }

        private DateTime _validUntilDate;
        [XafDisplayName("Geçerlilik Tarihi")]
        public DateTime ValidUntilDate
        {
            get => _validUntilDate;
            set => SetPropertyValue(nameof(ValidUntilDate), ref _validUntilDate, value);
        }

        private decimal _totalAmount;
        [XafDisplayName("Toplam Tutar")]
        public decimal TotalAmount // Bu alan Controller veya OnChanged ile güncellenecek
        {
            get => _totalAmount;
            set => SetPropertyValue(nameof(TotalAmount), ref _totalAmount, value);
        }

        private string _currency;
        [Size(3)]
        [XafDisplayName("Para Birimi")]
        public string Currency
        {
            get => _currency;
            set => SetPropertyValue(nameof(Currency), ref _currency, value);
        }

        private OfferStatus _status;
        [XafDisplayName("Teklif Durumu")]
        public OfferStatus Status
        {
            get => _status;
            set => SetPropertyValue(nameof(Status), ref _status, value);
        }

        private PermissionPolicyUser _preparedBy; // XPO User
        [XafDisplayName("Hazırlayan")]
        public PermissionPolicyUser PreparedBy
        {
            get => _preparedBy;
            set => SetPropertyValue(nameof(PreparedBy), ref _preparedBy, value);
        }

        private string _notes;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Açıklamalar / Notlar")]
        public string Notes
        {
            get => _notes;
            set => SetPropertyValue(nameof(Notes), ref _notes, value);
        }

        [Association("Offer-OfferItems"), Aggregated]
        [XafDisplayName("Teklif Kalemleri")]
        public XPCollection<OfferItem> OfferItems
        {
            get {
                XPCollection<OfferItem> items = GetCollection<OfferItem>(nameof(OfferItems));
                items.ListChanged += OfferItems_ListChanged; // Toplamı güncellemek için
                return items;
            }
        }

        private void OfferItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            // Bir kalem eklendiğinde, silindiğinde veya bir kalemin kendisi değiştiğinde (örn: miktarı)
            // bu olay tetiklenir. Ancak bir kalemin içindeki property değiştiğinde bu olay doğrudan tetiklenmez.
            // OfferItem içindeki property'lerin set metodlarında Offer.UpdateTotalAmount çağrılmalı.
            if (e.ListChangedType == ListChangedType.ItemAdded ||
                e.ListChangedType == ListChangedType.ItemDeleted ||
                e.ListChangedType == ListChangedType.Reset) // Reset tüm koleksiyon değiştiğinde
            {
                UpdateTotalAmount();
            }
        }

        public void UpdateTotalAmount()
        {
            if (IsLoading || IsSaving) return;

            decimal currentTotal = 0;
            foreach (OfferItem item in OfferItems)
            {
                currentTotal += item.LineTotalWithVat;
            }
            TotalAmount = currentTotal;
            // OnChanged(nameof(TotalAmount)); // Eğer TotalAmount bir PersistentAlias değilse ve UI'ın hemen güncellenmesi isteniyorsa.
                                            // Bizim senaryomuzda TotalAmount normal bir property, bu yüzden SetPropertyValue bunu halleder.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            OfferDate = DateTime.Now;
            ValidUntilDate = DateTime.Now.AddDays(30);
            Currency = "TRY";
            Status = OfferStatus.Draft;
            // Hazırlayan kullanıcıyı otomatik atama (SecuritySystem XPO ile uyumlu olmalı)
            // if(SecuritySystem.CurrentUser != null && Session.IsObjectFitForCriteria<PermissionPolicyUser>(SecuritySystem.CurrentUser, CriteriaOperator.Parse("Oid = ?", SecuritySystem.CurrentUserId)))
            //    PreparedBy = Session.GetObjectByKey<PermissionPolicyUser>(SecuritySystem.CurrentUserId);
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            // OfferItems koleksiyonu yüklendiğinde ListChanged olayına abone olalım
            // Bu, nesne yüklendikten sonra koleksiyon değişikliklerini de yakalamak için.
            // Ancak GetCollection içinde abone olmak daha yaygın.
            // OfferItems.ListChanged += OfferItems_ListChanged;
        }

        protected override void OnSaving()
        {
            UpdateTotalAmount(); // Kaydetmeden önce son bir kez toplamı güncelle
            base.OnSaving();
        }
    }

    public enum OfferStatus
    {
        [Display(Name = "Taslak")] Draft = 0,
        [Display(Name = "Gönderildi")] Sent = 1,
        [Display(Name = "Kabul Edildi")] Accepted = 2,
        [Display(Name = "Reddedildi")] Rejected = 3,
        [Display(Name = "Revize Edildi")] Revised = 4,
        [Display(Name = "İptal Edildi")] Cancelled = 5
    }
}
