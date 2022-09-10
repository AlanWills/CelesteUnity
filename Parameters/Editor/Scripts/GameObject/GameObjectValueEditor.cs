using Celeste.Parameters;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters
{
    [CustomEditor(typeof(GameObjectValue))]
    public class GameObjectValueEditor : ParameterValueEditor<GameObjectValue>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = EditorGUILayout.ObjectField("Value", Parameter.Value, typeof(GameObject), false) as GameObject;
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
