using UnityEngine;

namespace Celeste.Tools.Attributes.GUI
{
    public class HideAtRuntimeAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override bool IsVisible(UnityEditor.SerializedProperty property)
        {
            return !Application.isPlaying;
        }
#endif
    }
}
