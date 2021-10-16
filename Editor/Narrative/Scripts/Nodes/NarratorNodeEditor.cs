using Celeste.FSM;
using Celeste.Narrative;
using CelesteEditor.FSM.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.Narrative
{
    [CustomNodeEditor(typeof(NarratorNode))]
    public class NarratorNodeEditor : FSMNodeEditor
    {
        #region GUI

        public override void OnCreate()
        {
            base.OnCreate();

            serializedObject.Update();
            serializedObject.FindProperty("character").objectReferenceValue = NodeConstants.instance.Narrator;
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            NarratorNode narratorNode = target as NarratorNode;

            DrawDefaultPortPair();
            
            narratorNode.RawDialogue = EditorGUILayout.TextArea(narratorNode.RawDialogue, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2));

            EditorGUILayout.Space();

            DrawNarratorNodeValues();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNarratorNodeValues()
        {
            string[] excludes = { "m_Script", "graph", "position", "ports", "dialogue" };
            DrawNodeProperties(serializedObject, excludes);
        }

        #endregion
    }
}
