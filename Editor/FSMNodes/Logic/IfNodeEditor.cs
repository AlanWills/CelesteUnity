using Celeste.FSM;
using Celeste.FSM.Nodes;
using Celeste.FSM.Nodes.Logic;
using Celeste.FSM.Nodes.Logic.Conditions;
using CelesteEditor;
using CelesteEditor.Popups;
using CelesteEditor.FSM.Nodes.Logic.Conditions;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.FSM.Nodes.Logic
{
    [CustomNodeEditor(typeof(IfNode))]
    public class IfNodeEditor : FSMNodeEditor
    {
        #region Properties and Fields

        private int selectedEventType = 0;

        #endregion

        #region GUI

        public override void OnBodyGUI()
        {
            using (new LabelColourResetter(Color.black))
            {
                IfNode ifNode = target as IfNode;

                NodeEditorGUILayout.PortPair(
                    ifNode.GetInputPort(FSMNode.DEFAULT_INPUT_PORT_NAME),
                    ifNode.GetOutputPort(FSMNode.DEFAULT_OUTPUT_PORT_NAME));

                NodeEditorGUILayout.PortPair(
                    ifNode.GetInputPort(nameof(ifNode.inArgument)),
                    ifNode.GetOutputPort(nameof(ifNode.outArgument)));

                selectedEventType = EditorGUILayout.Popup(selectedEventType, ConditionsConstants.ConditionDisplayNames.ToArray());

                if (GUILayout.Button("Add Condition"))
                {
                    TextInputPopup.Display("New Condition...", (string conditionName) =>
                    {
                        ifNode.AddCondition(conditionName, ConditionsConstants.ConditionOptions[selectedEventType]);
                    });
                }

                for (uint i = ifNode.NumConditions; i > 0; --i)
                {
                    Condition valueCondition = ifNode.GetCondition(i - 1);

                    EditorGUILayout.Separator();
                    EditorGUILayout.BeginHorizontal();

                    bool removeCondition = GUILayout.Button("-", GUILayout.MaxWidth(16), GUILayout.MaxHeight(16));
                    EditorGUILayout.LabelField(valueCondition.name);
                    Rect rect = GUILayoutUtility.GetLastRect();
                    NodeEditorGUILayout.PortField(rect.position + new Vector2(rect.width, 0), ifNode.GetOutputPort(valueCondition.name));

                    EditorGUILayout.EndHorizontal();

                    if (removeCondition)
                    {
                        ifNode.RemoveCondition(valueCondition);
                    }
                    else
                    {
                        Editor conditionEditor = Editor.CreateEditor(valueCondition);
                        if (conditionEditor != null)
                        {
                            conditionEditor.OnInspectorGUI();
                        }
                        else
                        {
                            Debug.LogAssertionFormat("No editor found for ValueCondition Type: {0}", valueCondition.GetType().Name);
                        }
                    }
                }
            }
        }

        #endregion
    }
}