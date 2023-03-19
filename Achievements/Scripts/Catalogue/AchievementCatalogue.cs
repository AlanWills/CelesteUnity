using UnityEngine;
using Celeste.Objects;
using Celeste.Achievements.Objects;

namespace Celeste.Achievements.Catalogue
{
    [CreateAssetMenu(fileName = nameof(AchievementCatalogue), menuName = "Celeste/Achievements/Achievement Catalogue")]
    public class AchievementCatalogue : ListScriptableObject<Achievement>
    {
    }
}