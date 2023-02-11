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
                    boldLabel = new GUIStyle(GUI.skin.label);
                    boldLabel.fontStyle = FontStyle.Bold;
                    boldLabel.normal.textColor = Color.black;
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
                    centredBoldLabel = new GUIStyle(BoldLabel);
                    centredBoldLabel.alignment = TextAnchor.UpperCenter;
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

        #endregion
    }
}
