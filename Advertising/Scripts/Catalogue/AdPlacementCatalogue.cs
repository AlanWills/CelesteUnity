using Celeste.Objects;
using UnityEngine;

namespace Celeste.Advertising
{
    [CreateAssetMenu(
        fileName = nameof(AdPlacementCatalogue), 
        menuName = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM + "Ad Placement Catalogue",
        order = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM_PRIORITY)]
    public class AdPlacementCatalogue : ListScriptableObject<AdPlacement>
    {
        public AdPlacement FindPlacementById(string placementId)
        {
            return FindItem(x => string.CompareOrdinal(placementId, x.PlacementId) == 0);
        }
    }
}
