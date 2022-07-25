using Celeste.Logic;
using Celeste.Logic.Catalogue;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Logic.Catalogue
{
    [CustomEditor(typeof(ConditionCatalogue))]
    public class ConditionCatalogueEditor : IIndexableItemsEditor<Condition>
    {
    }
}
