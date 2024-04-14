using UnityEditor;
using UnityEngine;

namespace Celeste.Tools.Attributes.GUI
{
    public class HideUntilRuntimeAttribute : MultiPropertyAttribute, IVisibilityAttribute
    {
#if UNITY_EDITOR
        public bool IsVisible(SerializedProperty property)
        {
            return Application.isPlaying;
        }
#endif
    }
}
