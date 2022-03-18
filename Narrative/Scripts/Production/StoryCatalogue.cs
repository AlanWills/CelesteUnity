using Celeste.Objects;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = nameof(StoryCatalogue), menuName = "Celeste/Narrative/Production/Story Catalogue")]
    public class StoryCatalogue : ListScriptableObject<Story>
    {
        public Story FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}