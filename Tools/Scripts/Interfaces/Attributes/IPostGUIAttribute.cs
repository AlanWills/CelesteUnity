using UnityEngine;

namespace Celeste.Tools
{
    public interface IPostGUIAttribute : IOrderableAttribute
    {
#if UNITY_EDITOR
        public Rect OnPostGUI(Rect position, UnityEditor.SerializedProperty property);
#endif
    }
}