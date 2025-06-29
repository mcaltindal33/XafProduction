using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Depo Yönetimi")]
    [DisplayName("Stok Sayımı (XPO)")]
    public class StockCount : XPObject
    {
        public StockCount(Session session) : base(session) { }

        private string _countNumber;
        [Size(20)]
        [Indexed(Unique = true)] // Otomatik numara için Controller eklenecek
        [XafDisplayName("Sayım Numarası")]
        public string CountNumber { get => _countNumber; set => SetPropertyValue(nameof(CountNumber), ref _countNumber, value); }

        private DateTime _countDate;
        [XafDisplayName("Sayım Tarihi")]
        public DateTime CountDate { get => _countDate; set => SetPropertyValue(nameof(CountDate), ref _countDate, value); }

        private Warehouse _warehouse;
        [Association("Warehouse-StockCounts")]
        [XafDisplayName("Depo")]
        public Warehouse Warehouse { get => _warehouse; set => SetPropertyValue(nameof(Warehouse), ref _warehouse, value); }

        private Employee _countedBy;
        [Association("Employee-StockCountsBy")]
        [XafDisplayName("Sayan Kişi")]
        public Employee CountedBy { get => _countedBy; set => SetPropertyValue(nameof(CountedBy), ref _countedBy, value); }

        private StockCountStatus _status;
        [XafDisplayName("Durum")]
        public StockCountStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        private string _notes;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Notlar")]
        public string Notes { get => _notes; set => SetPropertyValue(nameof(Notes), ref _notes, value); }

        [Association("StockCount-CountItems"), Aggregated]
        [XafDisplayName("Sayım Kalemleri")]
        public XPCollection<StockCountItem> CountItems => GetCollection<StockCountItem>(nameof(CountItems));

        public override void AfterConstruction() {
            base.AfterConstruction();
            CountDate = DateTime.Now;
            Status = StockCountStatus.Planned;
        }

        // Sayım kalemleri yüklendikten veya değiştirildikten sonra çağrılacak bir Action (Controller ile)
        // Bu Action, CountItems'taki her bir Material için SystemQuantity'yi o anki stoktan çekebilir.
        public void LoadSystemQuantities()
        {
            if (Warehouse == null) return;
            foreach(var item in CountItems)
            {
                if(item.Material != null)
                {
                    // Depo bazlı stok miktarını almak için daha karmaşık bir sorgu gerekebilir.
                    // Şimdilik Material.TotalStockQuantity'yi (tüm depolardaki toplam) alalım,
                    // Bu, depo bazlı sayım için doğru olmayacaktır.
                    // Doğrusu: item.Material.StockTransactions.Where(st => st.Warehouse == this.Warehouse).Sum(st => st.Quantity);
                    // Ancak bu PersistentAlias içinde daha zor ifade edilir, bu yüzden bir metodla yapıyoruz.

                    double qtyInWarehouse = 0;
                    var transactionsInWarehouse = new XPQuery<StockTransaction>(Session)
                        .Where(st => st.Material == item.Material && st.Warehouse == this.Warehouse);

                    if(transactionsInWarehouse.Any())
                    {
                        qtyInWarehouse = transactionsInWarehouse.Sum(st => st.Quantity);
                    }
                    item.SystemQuantity = qtyInWarehouse;
                }
            }
        }
    }

    public enum StockCountStatus
    {
        [Display(Name = "Planlandı")] Planned = 0,
        [Display(Name = "Devam Ediyor")] InProgress = 1,
        [Display(Name = "Tamamlandı")] Completed = 2, // Sayım bitti, farklar hesaplandı
        [Display(Name = "Onaylandı")] Approved = 3, // Farklar stok hareketlerine işlendi
        [Display(Name = "İptal Edildi")] Cancelled = 4
    }
}
