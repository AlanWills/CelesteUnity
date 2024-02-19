using Celeste.Narrative;
using CelesteEditor.FSM.Nodes;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative
{
    [CustomNodeEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : FSMNodeEditor
    {
        #region GUI

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            DrawDefaultPortPair();
            DrawDialogueNodeValues();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDialogueNodeValues()
        {
            string[] excludes = { "m_Script", "graph", "position", "ports" };
            DrawNodeProperties(serializedObject, excludes);
        }

        #endregion
    }
}
