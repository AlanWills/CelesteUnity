using Celeste.Debug.Menus;
using Celeste.Parameters;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Advertising.Debug
{
    [CreateAssetMenu(
        fileName = nameof(AdsDebugMenu), 
        menuName = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM + "Debug/Ads Debug Menu",
        order = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM_PRIORITY)]
    public class AdsDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private BoolValue adsTestMode;
        [SerializeField] private AdRecord adRecord;

        #endregion

        protected override void OnDrawMenu()
        {
            adsTestMode.Value = GUILayout.Toggle(adsTestMode.Value, "Ads Test Mode");
            adRecord.AdsEnabled = GUILayout.Toggle(adRecord.AdsEnabled, "Ads Enabled");

            for (int i = 0, n = adRecord.NumAdPlacements; i < n; i++)
            {
                AdPlacement adPlacement = adRecord.GetAdPlacement(i);

                GUILayout.Space(10);
                GUILayout.Label($"{adPlacement.PlacementId} ({adPlacement.PlacementType})", GUI.skin.label.New().Bold());

                using (var indent = new GUIIndentScope())
                {
                    GUILayout.Space(5);
                    adPlacement.IsLoaded = GUILayout.Toggle(adPlacement.IsLoaded, adPlacement.PlacementId);

                    using (var horizontal = new GUILayout.HorizontalScope())
                    {
                        using (var enabled = new GUIEnabledScope(!adPlacement.IsLoaded))
                        {
                            if (GUILayout.Button("Load"))
                            {
                                adRecord.LoadAdPlacement(adPlacement);
                            }
                        }

                        using (var enabled = new GUIEnabledScope(adPlacement.IsLoaded))
                        {
                            if (GUILayout.Button("Play"))
                            {
                                adRecord.PlayAdPlacement(adPlacement, (result) => { });
                            }
                        }
                    }
                }
            }
        }
    }
}
