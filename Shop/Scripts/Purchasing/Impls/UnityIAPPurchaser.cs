#if USE_UNITY_PURCHASING
using Celeste.Shop.Catalogue;
using Celeste.Shop.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

namespace Celeste.Shop.Purchasing.Impls
{
    public class UnityIAPPurchaser : IIAPPurchaser, IDetailedStoreListener
    {
        #region Properties and Fields

        private IAPCatalogue iapCatalogue;
        private IStoreController controller;
        private IExtensionProvider extensions;
        private bool currentPurchaseComplete;
        private PurchaseResult purchaseResult;

        #endregion

        public UnityIAPPurchaser(IAPCatalogue _iapCatalogue)
        {
            iapCatalogue = _iapCatalogue;

            StandardPurchasingModule standardPurchasingModule = StandardPurchasingModule.Instance();
            var builder = ConfigurationBuilder.Instance(standardPurchasingModule);

            foreach (IAP iap in _iapCatalogue)
            {
                builder.AddProduct(iap.IAPCode, iap.ProductType, new IDs()
            {
                { iap.GoogleIAPCode, GooglePlay.Name },
                { iap.AppleIAPCode, AppleAppStore.Name }
            });
            }
            
            UnityPurchasing.Initialize(this, builder);
        }

        #region IIAPPurchaser

        public async Task<PurchaseResult> PurchaseIAP(IAP iap)
        {
            if (controller != null)
            {
                UnityEngine.Debug.Assert(!currentPurchaseComplete, $"We're attempting to begin a purchase, but it looks like we haven't finished cleaning one up...");
                currentPurchaseComplete = false;
                purchaseResult = new PurchaseResult();

                controller.InitiatePurchase(iap.IAPCode);

                while (currentPurchaseComplete == false)
                {
                    await Task.Yield();
                }

                return purchaseResult;
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Failed to purchase IAP {iap.name} due to Unity Purchasing not being initialized.");
                return IIAPPurchaser.MakePurchasingDisabledResult(iap);
            }
        }

        #endregion

        #region IDetailedStoreListener Impl

        /// <summary>
        /// Called when Unity IAP is ready to make purchases.
        /// </summary>
        void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.controller = controller;
            this.extensions = extensions;

            UnityEngine.Debug.Assert(iapCatalogue != null, $"Initialized the {nameof(UnityIAPPurchaser)} with a null IAP catalogue.  IAPs will likely be non-functional.");
            if (iapCatalogue != null && controller.products.all != null)
            {
                Dictionary<string, Product> productLookup = new Dictionary<string, Product>(controller.products.all.Length);

                foreach (Product product in controller.products.all)
                {
                    productLookup[product.definition.id] = product;
                }

                foreach (IAP iap in iapCatalogue)
                {
                    if (productLookup.TryGetValue(iap.IAPCode, out Product product))
                    {
                        iap.LocalisedPriceString = product.metadata.localizedPriceString;
                        iap.LocalisedTitle = product.metadata.localizedTitle;
                        iap.LocalisedDescription = product.metadata.localizedDescription;
                        iap.ISOCurrencyCode = product.metadata.isoCurrencyCode;
                        iap.LocalisedPrice = decimal.ToSingle(product.metadata.localizedPrice);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"Failed to find Product information for iap {iap.IAPCode}.");
                    }
                }
            }
        }

        /// <summary>
        /// Called when Unity IAP encounters an unrecoverable initialization error.
        ///
        /// Note that this will not be called if Internet is unavailable; Unity IAP
        /// will attempt initialization until it becomes available.
        /// </summary>
        void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
        {
            UnityEngine.Debug.LogError($"Failed to initialize {nameof(UnityIAPPurchaser)} because of error: {error}.");
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
        {
            UnityEngine.Debug.LogError($"Failed to initialize {nameof(UnityIAPPurchaser)} because of error: {error} and message: {message}.");
        }

        /// <summary>
        /// Called when a purchase completes.
        ///
        /// May be called at any time after OnInitialized().
        /// </summary>
        PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs e)
        {
            bool validPurchase = true; // Presume valid for platforms with no R.V.

            // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS
            // Prepare the validator with the secrets we prepared in the Editor
            // obfuscation window.
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                AppleTangle.Data(), Application.identifier);

            try
            {
                // On Google Play, result has a single product ID.
                // On Apple stores, receipts contain multiple products.
                var result = validator.Validate(e.purchasedProduct.receipt);
                // For informational purposes, we list the receipt(s)
                UnityEngine.Debug.Log("Receipt is valid. Contents:");
                foreach (IPurchaseReceipt productReceipt in result)
                {
                    UnityEngine.Debug.Log(productReceipt.productID);
                    UnityEngine.Debug.Log(productReceipt.purchaseDate);
                    UnityEngine.Debug.Log(productReceipt.transactionID);
                }
            }
            catch (IAPSecurityException)
            {
                UnityEngine.Debug.Log("Invalid receipt, not unlocking content.");
                validPurchase = false;
            }
#endif

            if (validPurchase)
            {
                CleanupSuccessfulPurchase(e.purchasedProduct.definition.id);
            }

            return PurchaseProcessingResult.Complete;
        }

        /// <summary>
        /// Called when a purchase fails.
        /// IStoreListener.OnPurchaseFailed is deprecated,
        /// use IDetailedStoreListener.OnPurchaseFailed instead.
        /// </summary>
        void IStoreListener.OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            CleanupUnsuccessfulPurchase(i.definition.id, $"Purchasing of {i.definition.id} failed in {nameof(UnityIAPPurchaser)}.", p);
        }

        /// <summary>
        /// Called when a purchase fails.
        /// </summary>
        void IDetailedStoreListener.OnPurchaseFailed(Product i, PurchaseFailureDescription p)
        {
            CleanupUnsuccessfulPurchase(i.definition.id, p.message, p.reason);
        }

        #endregion

        #region Utility

        private void CleanupSuccessfulPurchase(string iapCode)
        {
            currentPurchaseComplete = true;
            purchaseResult = new PurchaseResult()
            {
                itemCode = iapCode,
                success = true
            };
        }

        private void CleanupUnsuccessfulPurchase(string iapCode, string failureMessage, PurchaseFailureReason purchaseFailureReason)
        {
            currentPurchaseComplete = true;
            purchaseResult = new PurchaseResult()
            {
                itemCode = iapCode,
                success = false,
                errorMessage = failureMessage,
                reason = ToPurchaseFailedReason(purchaseFailureReason)
            };
        }

        private static PurchaseFailedReason ToPurchaseFailedReason(PurchaseFailureReason purchaseFailureReason)
        {
            return (PurchaseFailedReason)purchaseFailureReason;
        }

        #endregion
    }
}
#endif