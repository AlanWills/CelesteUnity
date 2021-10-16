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

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = EditorGUILayout.ObjectField("Value", Parameter.Value, typeof(GameObject), false) as GameObject;
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
