using Celeste.Shop.Objects;
using System.Threading.Tasks;

namespace Celeste.Shop
{
    public enum PurchaseFailedReason
    {
        PurchasingUnavailable,
        ExistingPurchasePending,
        ProductUnavailable,
        SignatureInvalid,
        UserCancelled,
        PaymentDeclined,
        DuplicateTransaction,
        Unknown,
        NotEnoughCurrency
    }

    public struct PurchaseResult
    {
        public bool success;
        public string itemCode;
        public PurchaseFailedReason reason;
        public string errorMessage;
    }

    public interface IIAPPurchaser
    {
        Task<PurchaseResult> PurchaseIAP(IAP iap);

        protected static PurchaseResult MakePurchasingDisabledResult(IAP iap)
        {
            return new PurchaseResult()
            {
                itemCode = iap.IAPCode,
                success = false,
                reason = PurchaseFailedReason.PurchasingUnavailable,
                errorMessage = "Purchasing is currently unavailable or disabled and so this purchase failed."
            };
        }
    }
}