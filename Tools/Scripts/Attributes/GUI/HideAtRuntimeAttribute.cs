using UnityEngine;

namespace Celeste.Tools.Attributes.GUI
{
    public class HideAtRuntimeAttribute : MultiPropertyAttribute, IVisibilityAttribute
    {
#if UNITY_EDITOR
        public bool IsVisible(UnityEditor.SerializedProperty property)
        {
            return !Application.isPlaying;
        }
#endif
    }
}
