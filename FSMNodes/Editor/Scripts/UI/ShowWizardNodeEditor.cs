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
    [CustomNodeEditor(typeof(ShowWizardNode))]
    public class ShowWizardNodeEditor : FSMNodeEditor
    {
        #region GUI

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            ShowWizardNode showPagedDialogNode = target as ShowWizardNode;

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

                NodeEditorGUILayout.PortField(showPagedDialogNode.GetInputPort(ShowWizardNode.DEFAULT_INPUT_PORT_NAME));
                NodeEditorGUILayout.PortField(showPagedDialogNode.GetOutputPort(ShowWizardNode.CONFIRM_PRESSED_PORT_NAME));

                if (showPagedDialogNode.parameters.showCloseButton)
                {
                    NodeEditorGUILayout.PortField(showPagedDialogNode.GetOutputPort(ShowWizardNode.CLOSE_PRESSED_PORT_NAME));
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
