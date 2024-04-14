using UnityEngine;

namespace Celeste.Tools
{
    public interface IPreGUIAttribute : IOrderableAttribute
    {
#if UNITY_EDITOR
        public Rect OnPreGUI(Rect position, UnityEditor.SerializedProperty property);
#endif
    }
}