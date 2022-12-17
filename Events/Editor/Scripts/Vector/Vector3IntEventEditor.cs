using Celeste.Events;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events
{
    [CustomEditor(typeof(Vector3IntEvent))]
    public class Vector3IntEventEditor : ParameterisedEventEditor<Vector3Int, Vector3IntEvent>
    {
        protected override Vector3Int DrawArgument(Vector3Int argument)
        {
            return EditorGUILayout.Vector3IntField(GUIContent.none, argument);
        }
    }
}
