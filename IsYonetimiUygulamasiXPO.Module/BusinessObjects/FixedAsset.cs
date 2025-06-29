using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Demirbaş Yönetimi")]
    [DisplayName("Demirbaş (XPO)")]
    public class FixedAsset : XPObject
    {
        public FixedAsset(Session session) : base(session) { }

        private string _assetCode;
        [Size(50)]
        [Indexed(Unique = true)]
        [XafDisplayName("Demirbaş Kodu")]
        public string AssetCode { get => _assetCode; set => SetPropertyValue(nameof(AssetCode), ref _assetCode, value); }

        private string _assetName;
        [XafDisplayName("Demirbaş Adı")]
        public string AssetName { get => _assetName; set => SetPropertyValue(nameof(AssetName), ref _assetName, value); }

        private string _description;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Açıklama")]
        public string Description { get => _description; set => SetPropertyValue(nameof(Description), ref _description, value); }

        private string _category;
        [XafDisplayName("Kategori")]
        public string Category { get => _category; set => SetPropertyValue(nameof(Category), ref _category, value); }

        private DateTime _acquisitionDate;
        [XafDisplayName("Alım Tarihi")]
        public DateTime AcquisitionDate { get => _acquisitionDate; set => SetPropertyValue(nameof(AcquisitionDate), ref _acquisitionDate, value); }

        private decimal _acquisitionCost;
        [XafDisplayName("Alım Maliyeti")]
        public decimal AcquisitionCost { get => _acquisitionCost; set => SetPropertyValue(nameof(AcquisitionCost), ref _acquisitionCost, value); }

        private int _usefulLifeYears;
        [XafDisplayName("Kullanım Ömrü (Yıl)")]
        public int UsefulLifeYears { get => _usefulLifeYears; set => SetPropertyValue(nameof(UsefulLifeYears), ref _usefulLifeYears, value); }

        private DepreciationMethodType _depreciationMethod;
        [XafDisplayName("Amortisman Metodu")]
        public DepreciationMethodType DepreciationMethod { get => _depreciationMethod; set => SetPropertyValue(nameof(DepreciationMethod), ref _depreciationMethod, value); }

        // Yıllık Amortisman Tutarı (Normal Amortisman için basit hesaplama)
        [PersistentAlias("IIF(UsefulLifeYears > 0, AcquisitionCost / UsefulLifeYears, 0)")]
        [XafDisplayName("Yıllık Amortisman Tutarı")]
        public decimal AnnualDepreciationAmount => Convert.ToDecimal(EvaluateAlias(nameof(AnnualDepreciationAmount)));

        private decimal _accumulatedDepreciation;
        [XafDisplayName("Birikmiş Amortisman")]
        // Bu alan genellikle periyodik bir işlemle (örn: yıl sonu) güncellenir.
        // Basitlik adına manuel girilebilir veya bir Action ile hesaplatılabilir.
        public decimal AccumulatedDepreciation { get => _accumulatedDepreciation; set => SetPropertyValue(nameof(AccumulatedDepreciation), ref _accumulatedDepreciation, value); }

        [PersistentAlias("AcquisitionCost - AccumulatedDepreciation")]
        [XafDisplayName("Net Defter Değeri")]
        public decimal NetBookValue => Convert.ToDecimal(EvaluateAlias(nameof(NetBookValue)));

        private string _location;
        [XafDisplayName("Lokasyon")]
        public string Location { get => _location; set => SetPropertyValue(nameof(Location), ref _location, value); }

        private Employee _responsibleEmployee;
        [Association("Employee-FixedAssetsResponsible")]
        [XafDisplayName("Sorumlu Personel")]
        public Employee ResponsibleEmployee { get => _responsibleEmployee; set => SetPropertyValue(nameof(ResponsibleEmployee), ref _responsibleEmployee, value); }

        private FixedAssetStatus _status;
        [XafDisplayName("Durum")]
        public FixedAssetStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }

        public override void AfterConstruction() {
            base.AfterConstruction();
            AcquisitionDate = DateTime.Now;
            DepreciationMethod = DepreciationMethodType.StraightLine;
            Status = FixedAssetStatus.InUse;
            UsefulLifeYears = 5; // Varsayılan
        }
    }

    public enum DepreciationMethodType
    {
        [Display(Name = "Normal Amortisman (Doğrusal)")] StraightLine = 0,
        [Display(Name = "Azalan Bakiyeler")] DecliningBalance = 1,
        // Diğer metodlar eklenebilir
    }

    public enum FixedAssetStatus
    {
        [Display(Name = "Kullanımda")] InUse = 0,
        [Display(Name = "Depoda")] InStorage = 1,
        [Display(Name = "Bakımda")] UnderMaintenance = 2,
        [Display(Name = "Hurda")] Scrapped = 3,
        [Display(Name = "Satıldı")] Sold = 4
    }
}
