using UnityEditor;
using UnityEngine;

namespace Celeste.Tools.Attributes.GUI
{
    public class HideUntilRuntimeAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override bool IsVisible(SerializedProperty property)
        {
            return Application.isPlaying;
        }
#endif
    }
}
