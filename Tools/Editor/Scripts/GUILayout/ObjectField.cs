using UnityEditor;
using UnityEngine;

namespace CelesteEditor
{
    public partial class CelesteEditorGUILayout
    {
        public static T ObjectField<T>(T obj) where T : Object
        {
            return EditorGUILayout.ObjectField(obj, typeof(T), false) as T;
        }
    }
}
