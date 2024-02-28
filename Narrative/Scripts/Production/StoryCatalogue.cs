using Celeste.Objects;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = nameof(StoryCatalogue), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Production/Story Catalogue", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class StoryCatalogue : ListScriptableObject<Story>
    {
        public Story FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}