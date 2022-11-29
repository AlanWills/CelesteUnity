using Celeste;
using Celeste.Components;
using Celeste.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Components
{
    public abstract class ComponentContainerUsingTemplatesEditor<TComponent> : Editor
        where TComponent : Celeste.Components.Component
    {
        #region Properties and Fields

        private ComponentContainerUsingTemplates<TComponent> Container => target as ComponentContainerUsingTemplates<TComponent>;

        private SerializedProperty componentTemplatesProperty;
        private int currentPage;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            componentTemplatesProperty = serializedObject.FindProperty("componentTemplates");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "componentTemplates");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Components", CelesteGUIStyles.BoldLabel);
            EditorGUILayout.Space();

            currentPage = GUIUtils.PaginatedList(
                currentPage,
                40,
                componentTemplatesProperty.arraySize,
                (i) =>
                {
                    GUILayout.Space(10);

                    using (var vertical = new EditorGUILayout.VerticalScope())
                    {
                        var componentTemplate = componentTemplatesProperty.GetArrayElementAtIndex(i);

                        using (var change = new EditorGUI.ChangeCheckScope())
                        {
                            SerializedProperty componentProperty = componentTemplate.FindPropertyRelative("component");
                            var component = componentProperty.objectReferenceValue;
                            
                            EditorGUILayout.PropertyField(componentTemplate, true);

                            if (change.changed &&
                                component == null && 
                                componentProperty.objectReferenceValue != null)
                            {
                                serializedObject.ApplyModifiedProperties();

                                ComponentData componentData = (componentProperty.objectReferenceValue as TComponent).CreateData();
                                Container.SetComponentData(i, componentData);

                                serializedObject.Update();
                            }
                        }
                    }
                },
                () => GUILayout.Button("+", GUILayout.ExpandWidth(false)),
                () => GUILayout.Button("-", GUILayout.ExpandWidth(false)),
                () => ++componentTemplatesProperty.arraySize,
                (i) => Container.RemoveComponent(i));

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
