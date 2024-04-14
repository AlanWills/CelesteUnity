using UnityEngine;

namespace Celeste.Tools
{
    public interface IGUIAttribute : IOrderableAttribute
    {
#if UNITY_EDITOR
        Rect OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label);
#endif
    }
}