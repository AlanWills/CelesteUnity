using Celeste.Parameters;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Parameters
{
    [CustomEditor(typeof(BoolValueList))]
    public class BoolValueListEditor : IIndexableItemsEditor<BoolValue>
    {
    }
}
