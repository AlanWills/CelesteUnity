using Celeste.Shop.Catalogue;
using Celeste.Shop.Objects;
using System.Threading.Tasks;

namespace Celeste.Shop.Purchasing.Impls
{
    public class EditorIAPPurchaser : IIAPPurchaser
    {
        public EditorIAPPurchaser(IAPCatalogue iapCatalogue)
        {
            foreach (IAP iap in iapCatalogue)
            {
                iap.LocalisedPriceString = "£0.99";
                iap.LocalisedTitle = iap.name;
                iap.LocalisedDescription = $"{iap.name} Description";
                iap.ISOCurrencyCode = "GBP";
                iap.LocalisedPrice = 0.99f;
            }
        }

        public async Task<PurchaseResult> PurchaseIAP(IAP iap)
        {
            await Task.Yield();

            return new PurchaseResult
            {
                itemCode = iap.IAPCode,
                success = true
            };
        }
    }
}