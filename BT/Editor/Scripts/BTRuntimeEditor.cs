using Celeste.BT;
using CelesteEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.BT
{
    [CustomEditor(typeof(BTRuntime))]
    public class BTRuntimeEditor : SceneGraphEditor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            BTRuntime btRuntime = target as BTRuntime;
            btRuntime.graph = EditorGUILayout.ObjectField(btRuntime.graph, typeof(BTGraph), false) as BTGraph;

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(btRuntime);
            }

            if (btRuntime.graph != null && GUILayout.Button("Open Graph", GUILayout.Height(40)))
            {
                NodeEditorWindow.Open(btRuntime.graph);
            }

            CelesteEditorGUILayout.HorizontalLine();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
