using Celeste;
using Celeste.Components;
using Celeste.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Components
{
    public abstract class ComponentContainerUsingTemplatesEditor<TComponent> : Editor
        where TComponent : Celeste.Components.BaseComponent
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
            
            currentPage = GUIExtensions.PaginatedList(
                currentPage,
                40,
                componentTemplatesProperty.arraySize,
                (i) =>
                {
                    GUILayout.Space(16);

                    using (new EditorGUILayout.VerticalScope())
                    {
                        SerializedProperty componentTemplate = componentTemplatesProperty.GetArrayElementAtIndex(i);
                        SerializedProperty componentProperty = componentTemplate.FindPropertyRelative("component"); 
                        var component = componentProperty.objectReferenceValue as BaseComponent;

                        if (component == null)
                        {
                            using (var change = new EditorGUI.ChangeCheckScope())
                            {
                                EditorGUILayout.PropertyField(componentProperty);

                                if (change.changed && componentProperty.objectReferenceValue != null)
                                {
                                    serializedObject.ApplyModifiedProperties();

                                    ComponentData componentData = (componentProperty.objectReferenceValue as TComponent).CreateData();
                                    Container.SetComponentData(i, componentData);

                                    serializedObject.Update();
                                }
                            }
                        }
                        else
                        {
                            using (new EditorGUI.DisabledScope(true))
                            {
                                EditorGUILayout.PropertyField(componentProperty);
                            }

                            SerializedProperty componentDataProperty = componentTemplate.FindPropertyRelative("data");
                            ComponentDataDrawer drawer = ComponentDataDrawers.GetComponentDataDrawer(componentDataProperty, component);
                            drawer.InspectorGUI();
                        }
                    }
                },
                () => GUILayout.Button("+", GUILayout.ExpandWidth(false)),
                () => GUILayout.Button("-", GUILayout.ExpandWidth(false)),
                () => Container.AddEmptyTemplate(),
                (i) => Container.RemoveComponent(i),
                null,
                GUIExtensions.ListLayoutOptions.None);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
