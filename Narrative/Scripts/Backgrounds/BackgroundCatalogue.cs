using Celeste.Objects;
using UnityEngine;

namespace Celeste.Narrative.Backgrounds
{
    [CreateAssetMenu(fileName = nameof(BackgroundCatalogue), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Backgrounds/Background Catalogue", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class BackgroundCatalogue : ArrayScriptableObject<Background>
    {
        public Background FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}