using Celeste.Objects;
using UnityEngine;

namespace Celeste.Advertising
{
    [CreateAssetMenu(fileName = nameof(AdPlacementCatalogue), menuName = "Celeste/Advertising/Ad Placement Catalogue")]
    public class AdPlacementCatalogue : ListScriptableObject<AdPlacement>
    {
        public AdPlacement FindPlacementById(string placementId)
        {
            return FindItem(x => string.CompareOrdinal(placementId, x.PlacementId) == 0);
        }
    }
}
