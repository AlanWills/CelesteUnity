using Celeste.FSM.Nodes.UI;
using CelesteEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.FSM.Nodes.UI
{
    [CustomNodeEditor(typeof(ShowDialogNode))]
    public class ShowDialogNodeEditor : FSMNodeEditor
    {
        #region GUI

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            ShowDialogNode showDialogNode = target as ShowDialogNode;

            using (new LabelColourResetter(Color.black))
            {
                string[] excludes = { "m_Script", "graph", "position", "ports" };

                // Iterate through serialized properties and draw them like the Inspector (But with ports)
                SerializedProperty iterator = serializedObject.GetIterator();
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    enterChildren = false;
                    if (excludes.Contains(iterator.name)) continue;
                    NodeEditorGUILayout.PropertyField(iterator, true);
                }

                NodeEditorGUILayout.PortField(showDialogNode.GetInputPort(ShowDialogNode.DEFAULT_INPUT_PORT_NAME));

                if (GUILayout.Button("Create Ports", GUILayout.ExpandWidth(false)))
                {
                    for (int i = 0; i < showDialogNode.parameters.customDialogEvents.Count; ++i)
                    {
                        showDialogNode.AddOutputPort(showDialogNode.parameters.customDialogEvents[i].name);
                    }
                }

                for (int i = 0; i < showDialogNode.parameters.customDialogEvents.Count; ++i)
                {
                    Celeste.Events.Event customDialogEvent = showDialogNode.parameters.customDialogEvents[i];
                    if (customDialogEvent)
                    {
                        NodeEditorGUILayout.PortField(showDialogNode.GetOutputPort(customDialogEvent.name));
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
