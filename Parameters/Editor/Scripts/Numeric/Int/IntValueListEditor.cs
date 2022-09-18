using Celeste.Parameters;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Parameters
{
    [CustomEditor(typeof(IntValueList))]
    public class IntValueListEditor : IIndexableItemsEditor<IntValue>
    {
    }
}
