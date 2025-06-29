using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace IsYonetimiUygulamasiXPO.Module.BusinessObjects
{
    [DisplayName("Sözleşme Kapsamındaki Hizmet (XPO)")]
    public class ServiceContractItem : XPObject
    {
        public ServiceContractItem(Session session) : base(session) { }

        private ServiceContract _serviceContract;
        [Association("ServiceContract-ContractItems")]
        [Browsable(false)]
        public ServiceContract ServiceContract { get => _serviceContract; set => SetPropertyValue(nameof(ServiceContract), ref _serviceContract, value); }

        private ServiceDefinition _serviceDefinition;
        [Association("ServiceDefinition-ContractItems")]
        [XafDisplayName("Hizmet")]
        public ServiceDefinition ServiceDefinition {
            get => _serviceDefinition;
            set {
                if(SetPropertyValue(nameof(ServiceDefinition), ref _serviceDefinition, value))
                {
                    if(!IsLoading && ServiceDefinition != null)
                    {
                        // Hizmet seçildiğinde anlaşılan fiyatı, hizmet tanımındaki fiyattan alabiliriz.
                        AgreedPrice = ServiceDefinition.UnitPrice;
                    }
                }
            }
        }

        private int _quantity;
        [XafDisplayName("Adet/Miktar")]
        public int Quantity { get => _quantity; set => SetPropertyValue(nameof(Quantity), ref _quantity, value); }

        private decimal _agreedPrice;
        [XafDisplayName("Anlaşılan Fiyat")]
        public decimal AgreedPrice { get => _agreedPrice; set => SetPropertyValue(nameof(AgreedPrice), ref _agreedPrice, value); }

        // Sözleşme toplamını etkileyecekse, bu property'nin OnChanged'inde ServiceContract.UpdateTotals() gibi bir metod çağrılabilir.
        // Veya ServiceContract.ContractAmount PersistentAlias ile hesaplanabilir.

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Quantity = 1;
        }
    }
}
