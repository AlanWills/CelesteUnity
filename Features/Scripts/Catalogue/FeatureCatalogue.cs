using Celeste.Objects;
using UnityEngine;

namespace Celeste.Features
{
    [CreateAssetMenu(fileName = nameof(FeatureCatalogue), menuName = "Celeste/Features/Feature Catalogue")]
    public class FeatureCatalogue : ArrayScriptableObject<Feature>
    {
        public Feature FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}
