using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Popups
{
    public class TextInputPopup : EditorWindow
    {
        #region Properties and Fields

        private string inputText;
        private Action<string> onConfirm;

        #endregion

        #region Show/Hide

        public static void Display(string initialValue, Action<string> onConfirm)
        {
            TextInputPopup window = ScriptableObject.CreateInstance<TextInputPopup>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 150, 50);
            window.onConfirm = onConfirm;
            window.inputText = initialValue;
            window.ShowPopup();
        }

        #endregion

        #region Unity Methods

        void OnGUI()
        {
            inputText = EditorGUILayout.TextField(inputText);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Confirm"))
            {
                onConfirm(inputText);
                Close();
            }

            if (GUILayout.Button("Cancel"))
            {
                Close();
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}
