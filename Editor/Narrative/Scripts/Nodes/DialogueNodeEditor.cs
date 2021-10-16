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
    [CustomNodeEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : FSMNodeEditor
    {
        #region GUI

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            DialogueNode dialogueNode = target as DialogueNode;

            DrawDefaultPortPair();
            
            dialogueNode.RawDialogue = EditorGUILayout.TextArea(dialogueNode.RawDialogue, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2));

            EditorGUILayout.Space();

            DrawDialogueNodeValues();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDialogueNodeValues()
        {
            string[] excludes = { "m_Script", "graph", "position", "ports", "dialogue" };
            DrawNodeProperties(serializedObject, excludes);
        }

        #endregion
    }
}
