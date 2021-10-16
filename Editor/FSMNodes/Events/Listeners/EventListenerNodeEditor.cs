using Celeste.FSM.Nodes.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static XNodeEditor.NodeEditor;

namespace CelesteEditor.FSM.Nodes.Events
{
    [CustomNodeEditor(typeof(EventListenerNode))]
    public class EventListenerNodeEditor : FSMNodeEditor
    {
        #region Properties and Fields

        private static GUIStyle editorLabelStyle;

        #endregion

        #region GUI

        public override void OnBodyGUI()
        {
            if (editorLabelStyle == null)
            {
                editorLabelStyle = new GUIStyle(EditorStyles.label);
            }

            EditorStyles.label.normal.textColor = Color.black;

            base.OnBodyGUI();

            EditorStyles.label.normal = editorLabelStyle.normal;
        }

        #endregion
    }
}
