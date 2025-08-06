using Celeste.FSM;
using Celeste.FSM.Prefabs;
using CelesteEditor.Objects;
using UnityEditor;

namespace CelesteEditor.FSM.Prefabs
{
    [CustomEditor(typeof(FSMNodePrefab))]
    public class FSMNodePrefabEditor : AssetWrapperScriptableObjectEditor<FSMNode>
    {
    }
}