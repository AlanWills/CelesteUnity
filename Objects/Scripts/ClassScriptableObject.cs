using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.Objects
{
    public class ClassScriptableObject<T> : ScriptableObject 
        where T : class, new()
    {
        [SerializeField, InlineDataInInspector] private T data;

        private void OnValidate()
        {
            if (data == null)
            {
                data = new T();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
#endif
            }
        }
    }
}
