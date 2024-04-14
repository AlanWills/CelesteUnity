namespace Celeste.Tools
{
    public interface IVisibilityAttribute : IOrderableAttribute
    {
#if UNITY_EDITOR
        bool IsVisible(UnityEditor.SerializedProperty property);
#endif
    }
}