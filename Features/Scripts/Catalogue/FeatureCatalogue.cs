using Celeste.Objects;
using UnityEngine;

namespace Celeste.Features
{
    [CreateAssetMenu(fileName = nameof(FeatureCatalogue), menuName = CelesteMenuItemConstants.FEATURES_MENU_ITEM + "Feature Catalogue", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class FeatureCatalogue : ArrayScriptableObject<Feature>
    {
        public Feature FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}
