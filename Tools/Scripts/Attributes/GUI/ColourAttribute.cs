using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ColourAttribute : MultiPropertyAttribute
    {
        #region Properties and Fields

        public Color Colour { get; }

        private Color oldGuiColour;

        #endregion

        public ColourAttribute(Color colour)
        {
            Colour = colour;
        }

        public ColourAttribute(int r, int g, int b, int a) :
            this(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f)
        {
        }

        public ColourAttribute(float r, float g, float b, float a) :
            this(new Color(r, g, b, a))
        {
        }

#if UNITY_EDITOR
        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            oldGuiColour = UnityEngine.GUI.color;
            UnityEngine.GUI.color = Color.blue;
        }
        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.color = oldGuiColour;
        }
#endif
    }
}