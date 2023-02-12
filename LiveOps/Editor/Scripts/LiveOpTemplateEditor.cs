using Celeste;
using Celeste.Components;
using Celeste.Components.Catalogue;
using Celeste.LiveOps;
using Celeste.LiveOps.Settings;
using Celeste.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.LiveOps
{
    [CustomEditor(typeof(LiveOpTemplate))]
    public class LiveOpTemplateEditor : Editor
    {
        #region Properties and Fields

        private SerializedProperty componentsProperty;
        private SerializedProperty componentsDataProperty;

        private ComponentCatalogue componentCatalogue;
        private List<Celeste.Components.Component> availableComponents = new List<Celeste.Components.Component>();
        private string[] availableComponentNames = default;
        private int selectedType = -1;
        private int currentPage = 0;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            componentsProperty = serializedObject.FindProperty("components");
            componentsDataProperty = serializedObject.FindProperty("componentsData");
            componentCatalogue = LiveOpsEditorSettings.GetOrCreateSettings().defaultComponentCatalogue;

            RefreshDataUsingComponentCatalogue();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "components");

            CelesteEditorGUILayout.HorizontalLine();

            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    componentCatalogue = EditorGUILayout.ObjectField(componentCatalogue, typeof(ComponentCatalogue), false) as ComponentCatalogue;

                    if (GUILayout.Button("Refresh Components", GUILayout.ExpandWidth(false)) || changeCheck.changed)
                    {
                        RefreshDataUsingComponentCatalogue();
                    }
                }

                if (availableComponents.Count > 0)
                {
                    using (var horizontal = new EditorGUILayout.HorizontalScope())
                    {
                        selectedType = EditorGUILayout.Popup("Component Type", selectedType, availableComponentNames);

                        if (selectedType >= 0)
                        {
                            if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
                            {
                                LiveOpTemplate liveOpTemplate = target as LiveOpTemplate;
                                liveOpTemplate.AddComponent(availableComponents[selectedType]);
                            }
                        }
                    }
                }
            }

            // Components
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Components", CelesteGUIStyles.BoldLabel);
                EditorGUILayout.Space();

                DrawValidationGUI();

                currentPage = GUIUtils.PaginatedList(
                    currentPage,
                    40,
                    componentsProperty.arraySize,
                    (i) =>
                    {
                        EditorGUILayout.PropertyField(componentsProperty.GetArrayElementAtIndex(i), true);
                    },
                    () => false,
                    () => GUILayout.Button("-", GUILayout.ExpandWidth(false)),
                    () => { },
                    (i) => { (target as LiveOpTemplate).RemoveComponent(i); });

                serializedObject.ApplyModifiedProperties();
            }

            // Components Data
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Components Data", CelesteGUIStyles.BoldLabel);
                EditorGUILayout.Space();

                if (componentsDataProperty.arraySize == 0 && componentsProperty.arraySize > 0)
                {
                    if (GUILayout.Button("Create Components Data"))
                    {
                        (target as LiveOpTemplate).CreateComponentsData();
                    }
                }
                else
                {
                    DrawValidationGUI();

                    currentPage = GUIUtils.ReadOnlyPaginatedList(
                        currentPage,
                        40,
                        componentsDataProperty.arraySize,
                        (i) =>
                        {
                            CelesteEditorGUILayout.JsonText(
                                componentsProperty.GetArrayElementAtIndex(i).objectReferenceValue.name, 
                                componentsDataProperty.GetArrayElementAtIndex(i));
                        });

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        #endregion

        private void DrawValidationGUI()
        {
            LiveOpTemplate template = target as LiveOpTemplate;

            bool assetsFound = false;
            bool timerFound = false;
            bool progressFound = false;

            for (int i = 0, n = template.NumComponents; i < n; ++i)
            {
                var component = template.GetComponent(i);
                
                if (!assetsFound)
                {
                    assetsFound = component is ILiveOpAssets;
                }

                if (!timerFound)
                {
                    timerFound = component is ILiveOpTimer;
                }

                if (!progressFound)
                {
                    progressFound = component is ILiveOpProgress;
                }
            }

            if (assetsFound)
            {
                EditorGUILayout.LabelField($"{nameof(ILiveOpAssets)} component found!", CelesteGUIStyles.SuccessLabel);
            }
            else
            {
                EditorGUILayout.LabelField($"No {nameof(ILiveOpAssets)} component found...", CelesteGUIStyles.ErrorLabel);
            }

            if (timerFound)
            {
                EditorGUILayout.LabelField($"{nameof(ILiveOpTimer)} component found!", CelesteGUIStyles.SuccessLabel);
            }
            else
            {
                EditorGUILayout.LabelField($"No {nameof(ILiveOpTimer)} component found...", CelesteGUIStyles.ErrorLabel);
            }

            if (progressFound)
            {
                EditorGUILayout.LabelField($"{nameof(ILiveOpProgress)} component found!", CelesteGUIStyles.SuccessLabel);
            }
            else
            {
                EditorGUILayout.LabelField($"No {nameof(ILiveOpProgress)} component found...", CelesteGUIStyles.ErrorLabel);
            }

            EditorGUILayout.Space();
        }

        private void RefreshDataUsingComponentCatalogue()
        {
            if (componentCatalogue != null)
            {
                foreach (var type in componentCatalogue.Items)
                {
                    availableComponents.Add(type.Value);
                }

                availableComponentNames = availableComponents.Select(x => x.name).ToArray();
                selectedType = 0;
            }
            else
            {
                availableComponents.Clear();
                availableComponentNames = null;
                selectedType = -1;
            }
        }
    }
}
