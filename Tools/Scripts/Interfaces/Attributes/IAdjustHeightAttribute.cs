using UnityEngine;

namespace Celeste.Tools
{
    public interface IAdjustHeightAttribute : IOrderableAttribute
    {
#if UNITY_EDITOR
        float AdjustPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label, float currentHeight);
#endif
    }
}