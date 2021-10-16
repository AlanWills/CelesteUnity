using Celeste.Narrative.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.UI
{
    [CustomEditor(typeof(DialogueView))]
    public class DialogueViewEditor : Editor
    {
        #region Properties and Fields

        private UIPosition uiPosition;

        #endregion

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            using (GUILayout.HorizontalScope horizontalScope = new GUILayout.HorizontalScope())
            {
                uiPosition = (UIPosition)EditorGUILayout.EnumPopup("UI Position", uiPosition);

                if (GUILayout.Button("Set UI Position", GUILayout.ExpandWidth(false)))
                {
                    (target as DialogueView).SetUIPosition(uiPosition);
                }
            }
        }
    }
}
