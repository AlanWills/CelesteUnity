using Celeste.Options;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Options
{
    [CustomEditor(typeof(IntOptionList))]
    public class IntOptionListEditor : IIndexableItemsEditor<IntOption>
    {
    }
}
