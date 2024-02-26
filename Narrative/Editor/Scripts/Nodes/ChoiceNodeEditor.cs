using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Narrative.Choices;
using CelesteEditor.FSM.Nodes;
using CelesteEditor.Narrative.Choices;
using CelesteEditor.Popups;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace CelesteEditor.Narrative
{
    [CustomNodeEditor(typeof(ChoiceNode))]
    public class ChoiceNodeEditor : FSMNodeEditor
    {
        #region Properties and Fields

        private int selectedEventType = 0;

        #endregion

        #region GUI

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            ChoiceNode choiceNode = target as ChoiceNode;

            NodeEditorGUILayout.PortField(choiceNode.GetInputPort(FSMNode.DEFAULT_INPUT_PORT_NAME));

            choiceNode.RawDialogue = TextArea(choiceNode.RawDialogue, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2));

            Space();

            DrawChoiceNodeValues();

            using (new HorizontalScope())
            {
                selectedEventType = Popup(selectedEventType, ChoicesConstants.ChoiceDisplayNames.ToArray());

                if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
                {
                    TextInputPopup.Display("Internal Choice ID...", (string choiceName) =>
                    {
                        choiceNode.AddChoice(choiceName, ChoicesConstants.ChoiceOptions[selectedEventType]);
                    });
                }
            }

            for (int i = 0, n = choiceNode.NumChoices; i < n; ++i)
            {
                Choice choice = choiceNode.GetChoice(i);

                Separator();
                
                using (new HorizontalScope())
                {
                    if (i > 0 && GUILayout.Button("^", GUILayout.MaxWidth(16), GUILayout.MaxHeight(16)))
                    {
                        choiceNode.MoveChoice(i, i - 1);
                        continue;
                    }

                    if (i < n - 1 && GUILayout.Button("V", GUILayout.MaxWidth(16), GUILayout.MaxHeight(16)))
                    {
                        choiceNode.MoveChoice(i, i + 1);
                        continue;
                    }

                    if (GUILayout.Button("-", GUILayout.MaxWidth(16), GUILayout.MaxHeight(16)))
                    {
                        choiceNode.RemoveChoice(choice);
                        --i;
                        --n;
                        continue;
                    }

                    LabelField(choice.name);
                    Rect rect = GUILayoutUtility.GetLastRect();
                    NodeEditorGUILayout.PortField(rect.position + new Vector2(rect.width, 0), choiceNode.GetOutputPort(choice.name));
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawChoiceNodeValues()
        {
            string[] excludes = { "m_Script", "graph", "position", "ports", "choices", "dialogue" };
            DrawNodeProperties(serializedObject, excludes);
        }

        #endregion
    }
}
