using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools
{
    public static class SerializedPropertyExtensions
    {
#if UNITY_EDITOR
        public static IEnumerable<SerializedProperty> EditorOnly_VisibleChildProperties(this SerializedProperty property)
        {
            SerializedProperty nextSiblingProperty = property.Copy();
            nextSiblingProperty.NextVisible(false);

            if (property.NextVisible(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(property, nextSiblingProperty))
                        yield break;

                    yield return property;
                }
                while (property.NextVisible(false));
            }
        }

        public static IEnumerable<SerializedProperty> EditorOnly_AllVisibleProperties(this SerializedObject serializedObject)
        {
            SerializedProperty serializedProperty = serializedObject.GetIterator();
            serializedProperty.NextVisible(true);

            yield return serializedProperty;

            if (serializedProperty.NextVisible(true))
            {
                do
                {
                    yield return serializedProperty;
                }
                while (serializedProperty.NextVisible(false));
            }
        }
#endif
    }
}