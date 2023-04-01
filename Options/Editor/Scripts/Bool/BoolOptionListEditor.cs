using Celeste.Options;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Options
{
    [CustomEditor(typeof(BoolOptionList))]
    public class BoolOptionListEditor : IIndexableItemsEditor<BoolOption>
    {
    }
}
