using UnityEditor;
using CelesteEditor.DataStructures;
using Celeste.Achievements.Objects;

namespace CelesteEditor.Achievements.Catalogue
{
    [CustomEditor(typeof(Achievement))]
    public class AchievementCatalogueEditor : IIndexableItemsEditor<Achievement>
    {
    }
}