using Celeste.Testing;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Testing
{
    public class IntegrationTestGUI
    {
        #region Properties and Fields

        private static GUIStyle passedLabelStyle;
        private static GUIStyle PassedLabelStyle
        {
            get
            {
                if (passedLabelStyle == null)
                {
                    Color green = new Color(0, 0.75f, 0);

                    passedLabelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                    passedLabelStyle.normal.textColor = green;
                    passedLabelStyle.hover.textColor = green;
                    passedLabelStyle.focused.textColor = green;
                    passedLabelStyle.active.textColor = green;
                }

                return passedLabelStyle;
            }
        }

        private static GUIStyle failedLabelStyle;
        private static GUIStyle FailedLabelStyle
        {
            get
            {
                if (failedLabelStyle == null)
                {
                    failedLabelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                    failedLabelStyle.normal.textColor = Color.red;
                    failedLabelStyle.hover.textColor = Color.red;
                    failedLabelStyle.focused.textColor = Color.red;
                    failedLabelStyle.active.textColor = Color.red;
                }

                return failedLabelStyle;
            }
        }

        private static GUIStyle notRunLabelStyle;
        private static GUIStyle NotRunLabelStyle
        {
            get
            {
                if (notRunLabelStyle == null)
                {
                    notRunLabelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                }

                return notRunLabelStyle;
            }
        }

        public bool IsSelected { get; set; }
        public IntegrationTest IntegrationTest { get; private set; }
        
        public TestStatus TestStatus
        {
            get { return IntegrationTest.TestStatus; }
        }

        #endregion

        public IntegrationTestGUI(IntegrationTest integrationTest)
        {
            IsSelected = false;
            IntegrationTest = integrationTest;
        }

        public void GUI()
        {
            GUIStyle style;

            if (TestStatus == TestStatus.Passed)
            {
                style = PassedLabelStyle;
            }
            else if (TestStatus == TestStatus.Failed)
            {
                style = FailedLabelStyle;
            }
            else
            {
                style = NotRunLabelStyle;
            }

            using (var horizontalScope = new EditorGUILayout.HorizontalScope())
            {
                IsSelected = EditorGUILayout.ToggleLeft(IntegrationTest.DisplayName, IsSelected, style);

                if (TestStatus == TestStatus.Failed && GUILayout.Button("Save Last Results As Expected", GUILayout.ExpandWidth(false)))
                {
                    IntegrationTest.SaveLastResultsAsExpected();
                }
            }
        }
    }
}
