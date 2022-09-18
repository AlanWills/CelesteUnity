using Celeste.Parameters;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Parameters
{
    [CustomEditor(typeof(StringValueList))]
    public class StringValueListEditor : IIndexableItemsEditor<StringValue>
    {
    }
}
