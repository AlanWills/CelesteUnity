using UnityEngine;
using Celeste.Objects;
using Celeste.Achievements.Objects;

namespace Celeste.Achievements.Catalogue
{
    [CreateAssetMenu(
        fileName = nameof(AchievementCatalogue), 
        menuName = CelesteMenuItemConstants.ACHIEVEMENTS_MENU_ITEM + "Achievement Catalogue", 
        order = CelesteMenuItemConstants.ACHIEVEMENTS_MENU_ITEM_PRIORITY)]
    public class AchievementCatalogue : ListScriptableObject<Achievement>
    {
    }
}