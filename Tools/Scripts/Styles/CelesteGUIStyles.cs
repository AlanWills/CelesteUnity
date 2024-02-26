using UnityEngine;

namespace Celeste
{
    public static class CelesteGUIStyles
    {
        #region Properties and Fields

        private static GUIStyle blackLabel;
        public static GUIStyle BlackLabel
        {
            get
            {
                if (blackLabel == null)
                {
                    blackLabel = new GUIStyle(GUI.skin.label);
                    blackLabel.normal.textColor = Color.black;
                }

                return blackLabel;
            }
        }

        private static GUIStyle boldLabel;
        public static GUIStyle BoldLabel
        {
            get
            {
                if (boldLabel == null)
                {
                    boldLabel = GUI.skin.label.New().Bold();
                }

                return boldLabel;
            }
        }

        private static GUIStyle centredBoldLabel;
        public static GUIStyle CentredBoldLabel
        {
            get
            {
                if (centredBoldLabel == null)
                {
                    centredBoldLabel = BoldLabel.New().UpperCentreAligned();
                }

                return centredBoldLabel;
            }
        }

        private static GUIStyle successLabel;
        public static GUIStyle SuccessLabel
        {
            get
            {
                if (successLabel == null)
                {
                    successLabel = new GUIStyle(GUI.skin.label);
                    successLabel.normal.textColor = new Color(0, 0.4f, 0);
                }

                return successLabel;
            }
        }

        private static GUIStyle errorLabel;
        public static GUIStyle ErrorLabel
        {
            get
            {
                if (errorLabel == null)
                {
                    errorLabel = new GUIStyle(GUI.skin.label);
                    errorLabel.normal.textColor = Color.red;
                }

                return errorLabel;
            }
        }

        private static GUIStyle wrappedTextArea;
        public static GUIStyle WrappedTextArea
        {
            get
            {
                if (wrappedTextArea == null)
                {
                    wrappedTextArea = new GUIStyle(GUI.skin.textArea);
                    wrappedTextArea.wordWrap = true;
                }

                return wrappedTextArea;
            }
        }

        #endregion

        public static GUIStyle New(this GUIStyle guiStyle)
        {
            return new GUIStyle(guiStyle);
        }

        public static GUIStyle Bold(this GUIStyle guiStyle)
        {
            guiStyle.fontStyle = FontStyle.Bold;
            return guiStyle;
        }

        public static GUIStyle UpperCentreAligned(this GUIStyle guiStyle)
        {
            guiStyle.alignment = TextAnchor.UpperCenter;
            return guiStyle;
        }

        public static GUIStyle MiddleCentreAligned(this GUIStyle guiStyle)
        {
            guiStyle.alignment = TextAnchor.MiddleCenter;
            return guiStyle;
        }

        public static GUIStyle EnableWrapping(this GUIStyle guiStyle)
        {
            guiStyle.wordWrap = true;
            return guiStyle;
        }
    }
}
