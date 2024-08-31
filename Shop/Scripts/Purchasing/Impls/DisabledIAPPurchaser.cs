using Celeste.Shop.Objects;
using System.Threading.Tasks;

namespace Celeste.Shop.Purchasing.Impls
{
    public class DisabledIAPPurchaser : IIAPPurchaser
    {
        public Task<PurchaseResult> PurchaseIAP(IAP iap)
        {
            return Task.FromResult(IIAPPurchaser.MakePurchasingDisabledResult(iap));
        }
    }
}