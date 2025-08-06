using Celeste.Narrative;
using CelesteEditor.Objects;
using UnityEditor;

namespace CelesteEditor.Narrative
{
    [CustomEditor(typeof(NarrativeNodePrefab))]
    public class NarrativeNodePrefabEditor : AssetWrapperScriptableObjectEditor<NarrativeNode>
    {
    }
}