using Celeste.Parameters;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Parameters
{
    [CustomEditor(typeof(FloatValueList))]
    public class FloatValueListEditor : IIndexableItemsEditor<FloatValue>
    {
    }
}
