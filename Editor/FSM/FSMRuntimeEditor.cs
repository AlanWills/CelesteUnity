using Celeste.FSM;
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

namespace CelesteEditor.FSM
{
    [CustomEditor(typeof(FSMRuntime))]
    public class FSMRuntimeEditor : SceneGraphEditor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            FSMRuntime fsmRuntime = (target as FSMRuntime);
            fsmRuntime.graph = EditorGUILayout.ObjectField(fsmRuntime.graph, typeof(FSMGraph), false) as FSMGraph;

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(fsmRuntime);
            }

            if (fsmRuntime.graph != null && GUILayout.Button("Open graph", GUILayout.Height(40)))
            {
                NodeEditorWindow.Open(fsmRuntime.graph);
            }

            CelesteEditorGUILayout.HorizontalLine();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("lateUpdate"));
            EditorGUILayout.LabelField(string.Format("Current Node: {0}", fsmRuntime.CurrentNode != null ? fsmRuntime.CurrentNode.name : "null"));

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
