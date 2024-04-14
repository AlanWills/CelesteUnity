using UnityEngine;

namespace Celeste.Tools
{
    public interface IGetHeightAttribute : IOrderableAttribute
    {
#if UNITY_EDITOR
        float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label);
#endif
    }
}