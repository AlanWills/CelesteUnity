using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class ReadOnlyAtRuntimeAttribute : MultiPropertyAttribute, IPreGUIAttribute, IPostGUIAttribute
    {
#if UNITY_EDITOR
        public Rect OnPreGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.enabled = !Application.isPlaying;
            return position;
        }

        public Rect OnPostGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.enabled = true;
            return position;
        }
#endif
    }
}
