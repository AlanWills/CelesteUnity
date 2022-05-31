using Celeste;
using Celeste.Components;
using Celeste.Components.Catalogue;
using Celeste.LiveOps;
using Celeste.LiveOps.Settings;
using Celeste.Tools;
using System;
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

        private ComponentCatalogue componentCatalogue;
        private List<Type> availableTypes = new List<Type>();
        private string[] availableTypeNames = default;
        private int selectedType = -1;
        private int currentPage = 0;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            componentsProperty = serializedObject.FindProperty("components");
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
                componentCatalogue = EditorGUILayout.ObjectField(componentCatalogue, typeof(ComponentCatalogue), false) as ComponentCatalogue;

                if (changeCheck.changed)
                {
                    RefreshDataUsingComponentCatalogue();
                }

                if (availableTypes.Count > 0)
                {
                    using (var horizontal = new EditorGUILayout.HorizontalScope())
                    {
                        selectedType = EditorGUILayout.Popup("Component Type", selectedType, availableTypeNames);

                        if (selectedType >= 0)
                        {
                            if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
                            {
                                LiveOpTemplate liveOpTemplate = target as LiveOpTemplate;
                                liveOpTemplate.AddComponent(availableTypes[selectedType]);
                            }
                        }
                    }
                }
            }

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

        #endregion

        private void DrawValidationGUI()
        {
            LiveOpTemplate template = target as LiveOpTemplate;

            bool assetsFound = false;
            bool timerFound = false;
            bool progressFound = false;

            for (int i = 0, n = template.NumComponents; i < n; ++i)
            {
                var component = template.GetComponent(i).component;
                
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
                    availableTypes.Add(type.Value.GetType());
                }

                availableTypeNames = availableTypes.Select(x => x.Name).ToArray();
                selectedType = 0;
            }
            else
            {
                availableTypes.Clear();
                availableTypeNames = null;
                selectedType = -1;
            }
        }
    }
}
