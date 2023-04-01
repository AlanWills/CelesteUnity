using Celeste.Options;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Options
{
    [CustomEditor(typeof(FloatOptionList))]
    public class FloatOptionListEditor : IIndexableItemsEditor<FloatOption>
    {
    }
}
