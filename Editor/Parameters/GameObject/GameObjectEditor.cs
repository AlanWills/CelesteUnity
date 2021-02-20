using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters
{
    [CustomEditor(typeof(GameObjectValue))]
    public class GameObjectValueEditor : ParameterValueEditor<GameObject>
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Parameter.Value = EditorGUILayout.ObjectField("Value", Parameter.Value, typeof(GameObject), false) as GameObject;
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
