using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ColourAttribute : MultiPropertyAttribute, IPreGUIAttribute, IPostGUIAttribute
    {
        #region Properties and Fields

        public Color Colour { get; }

        private Color oldGuiColour;

        #endregion

        public ColourAttribute(int r, int g, int b, int a) :
            this(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f)
        {
        }

        public ColourAttribute(float r, float g, float b, float a)
        {
            Colour = new Color(r, g, b, a);
        }

#if UNITY_EDITOR
        public Rect OnPreGUI(Rect position, SerializedProperty property)
        {
            oldGuiColour = UnityEngine.GUI.color;
            UnityEngine.GUI.color = Colour;
            return position;
        }

        public Rect OnPostGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.color = oldGuiColour;
            return position;
        }
#endif
    }
}