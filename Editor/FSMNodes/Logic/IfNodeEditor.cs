using Celeste.FSM;
using Celeste.FSM.Nodes;
using Celeste.FSM.Nodes.Logic;
using Celeste.Logic;
using CelesteEditor;
using CelesteEditor.Popups;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using static UnityEditor.EditorGUILayout;

namespace CelesteEditor.FSM.Nodes.Logic
{
    [CustomNodeEditor(typeof(IfNode))]
    public class IfNodeEditor : FSMNodeEditor
    {
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

                if (GUILayout.Button("Add Condition"))
                {
                    TextInputPopup.Display("New Condition...", (string conditionName) =>
                    {
                        ifNode.AddCondition(conditionName);
                    });
                }

                for (uint i = ifNode.NumConditions; i > 0; --i)
                {
                    IfCondition condition = ifNode.GetCondition(i - 1);

                    EditorGUILayout.Separator();
                    EditorGUILayout.BeginHorizontal();

                    bool removeCondition = GUILayout.Button("-", GUILayout.MaxWidth(16), GUILayout.MaxHeight(16));
                    EditorGUILayout.LabelField(condition.Name);
                    Rect rect = GUILayoutUtility.GetLastRect();
                    NodeEditorGUILayout.PortField(rect.position + new Vector2(rect.width, 0), ifNode.GetOutputPort(condition.Name));

                    EditorGUILayout.EndHorizontal();

                    if (removeCondition)
                    {
                        ifNode.RemoveCondition(i - 1);
                    }
                    else
                    {
                        EditorGUI.BeginChangeCheck();

                        condition.Condition = ObjectField(condition.Condition, typeof(Condition), false) as Condition;
                        condition.UseArgument = ToggleLeft("In Arg Is Target", condition.UseArgument);

                        if (EditorGUI.EndChangeCheck())
                        {
                            EditorUtility.SetDirty(ifNode);
                        }
                    }
                }
            }
        }

        #endregion
    }
}