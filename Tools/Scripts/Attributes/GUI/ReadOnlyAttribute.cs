using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class ReadOnlyAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            base.OnPreGUI(position, property);

            UnityEngine.GUI.enabled = false;
        }

        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            base.OnPostGUI(position, property);

            UnityEngine.GUI.enabled = true;
        }
#endif
    }
}
