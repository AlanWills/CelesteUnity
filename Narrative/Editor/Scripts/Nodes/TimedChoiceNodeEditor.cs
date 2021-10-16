using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Narrative.Choices;
using CelesteEditor.FSM.Nodes;
using CelesteEditor.Narrative.Choices;
using CelesteEditor.Popups;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;
using static XNodeEditor.NodeEditor;

namespace CelesteEditor.Narrative
{
    [CustomNodeEditor(typeof(TimedChoiceNode))]
    public class TimedChoiceNodeEditor : FSMNodeEditor
    {
        #region Properties and Fields

        private int selectedEventType = 0;
        private List<Choice> choicesToRemove = new List<Choice>();

        #endregion

        #region GUI

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            TimedChoiceNode timedChoiceNode = target as TimedChoiceNode;

            DrawDefaultPortPair();

            timedChoiceNode.RawDialogue = TextArea(timedChoiceNode.RawDialogue, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2));

            Space();

            DrawTimedChoiceNodeValues();

            using (HorizontalScope horizontalScope = new HorizontalScope())
            {
                selectedEventType = Popup(selectedEventType, ChoicesConstants.ChoiceDisplayNames.ToArray());

                if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
                {
                    TextInputPopup.Display("New Choice...", (string choiceName) =>
                    {
                        timedChoiceNode.AddChoice(choiceName, ChoicesConstants.ChoiceOptions[selectedEventType]);
                    });
                }
            }

            for (int i = 0, n = timedChoiceNode.NumChoices; i < n; ++i)
            {
                Choice choice = timedChoiceNode.GetChoice(i);

                Separator();
                
                using (HorizontalScope horizontal = new HorizontalScope())
                {
                    if (GUILayout.Button("-", GUILayout.MaxWidth(16), GUILayout.MaxHeight(16)))
                    {
                        choicesToRemove.Add(choice);
                    }

                    LabelField(choice.name);
                    Rect rect = GUILayoutUtility.GetLastRect();
                    NodeEditorGUILayout.PortField(rect.position + new Vector2(rect.width, 0), timedChoiceNode.GetOutputPort(choice.name));
                }
            }

            foreach (Choice choice in choicesToRemove)
            {
                timedChoiceNode.RemoveChoice(choice);
            }
            choicesToRemove.Clear();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawTimedChoiceNodeValues()
        {
            string[] excludes = { "m_Script", "graph", "position", "ports", "choices", "dialogue" };
            DrawNodeProperties(serializedObject, excludes);
        }

        #endregion
    }
}
