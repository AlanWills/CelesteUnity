using Celeste.Options;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Options
{
    [CustomEditor(typeof(StringOptionList))]
    public class StringOptionListEditor : IIndexableItemsEditor<StringOption>
    {
    }
}
