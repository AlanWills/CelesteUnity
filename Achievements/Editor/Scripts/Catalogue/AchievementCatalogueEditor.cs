using Celeste.Achievements.Catalogue;
using UnityEditor;
using CelesteEditor.DataStructures;
using Celeste.Achievements.Objects;

namespace CelesteEditor.Achievements.Catalogue
{
    [CustomEditor(typeof(AchievementCatalogue))]
    public class AchievementCatalogueEditor : IIndexableItemsEditor<Achievement>
    {
    }
}