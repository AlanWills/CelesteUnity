using Celeste;
using Celeste.Localisation;
using Celeste.Tools;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CelesteEditor.Localisation
{
    [CustomEditor(typeof(EnglishNumberToLocalisedTextConverter))]
    public class EnglishNumberToLocalisedTextConverterEditor : Editor
    {
        #region Properties and Fields

        private EnglishNumberToLocalisedTextConverter Converter => target as EnglishNumberToLocalisedTextConverter;

        private List<ValueTuple<int, LocalisationKey>> specialCasesLookup = new List<(int, LocalisationKey)>();
        private ReorderableList specialCasesReorderableList;

        #endregion

        private void OnEnable()
        {
            RefreshSpecialCaseLookup();

            specialCasesReorderableList = new ReorderableList(specialCasesLookup, typeof(ValueTuple<int, LocalisationKey>));
            specialCasesReorderableList.drawHeaderCallback += DrawHeaderCallback;
            specialCasesReorderableList.drawElementCallback += DrawElementCallback;
    }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Try Set Up Values"))
            {
                // And
                {
                    SerializedProperty andProperty = serializedObject.FindProperty("and");
                    andProperty.objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("And");
                }

                // Negative
                {
                    SerializedProperty negativeProperty = serializedObject.FindProperty("negative");
                    negativeProperty.objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Negative");
                }

                // 0 - 9
                {
                    SerializedProperty zeroToNineProperty = serializedObject.FindProperty("zeroToNine");
                    zeroToNineProperty.arraySize = 10;
                    zeroToNineProperty.GetArrayElementAtIndex(0).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Zero");
                    zeroToNineProperty.GetArrayElementAtIndex(1).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("One");
                    zeroToNineProperty.GetArrayElementAtIndex(2).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Two");
                    zeroToNineProperty.GetArrayElementAtIndex(3).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Three");
                    zeroToNineProperty.GetArrayElementAtIndex(4).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Four");
                    zeroToNineProperty.GetArrayElementAtIndex(5).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Five");
                    zeroToNineProperty.GetArrayElementAtIndex(6).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Six");
                    zeroToNineProperty.GetArrayElementAtIndex(7).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Seven");
                    zeroToNineProperty.GetArrayElementAtIndex(8).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Eight");
                    zeroToNineProperty.GetArrayElementAtIndex(9).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Nine");
                }

                // 10 - 100
                {
                    SerializedProperty tenToNinetyProperty = serializedObject.FindProperty("tenToNinety");
                    tenToNinetyProperty.arraySize = 9;
                    tenToNinetyProperty.GetArrayElementAtIndex(0).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Ten");
                    tenToNinetyProperty.GetArrayElementAtIndex(1).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Twenty");
                    tenToNinetyProperty.GetArrayElementAtIndex(2).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Thirty");
                    tenToNinetyProperty.GetArrayElementAtIndex(3).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Fourty");
                    tenToNinetyProperty.GetArrayElementAtIndex(4).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Fifty");
                    tenToNinetyProperty.GetArrayElementAtIndex(5).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Sixty");
                    tenToNinetyProperty.GetArrayElementAtIndex(6).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Seventy");
                    tenToNinetyProperty.GetArrayElementAtIndex(7).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Eighty");
                    tenToNinetyProperty.GetArrayElementAtIndex(8).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Ninety");
                }

                // Special Cases
                {
                    Converter.SetSpecialCase(11, CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Eleven"));
                    Converter.SetSpecialCase(12, CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Twelve"));
                    Converter.SetSpecialCase(13, CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Thirteen"));
                    Converter.SetSpecialCase(14, CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Fourteen"));
                    Converter.SetSpecialCase(15, CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Fifteen"));
                    Converter.SetSpecialCase(16, CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Sixteen"));
                    Converter.SetSpecialCase(17, CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Seventeen"));
                    Converter.SetSpecialCase(18, CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Eighteen"));
                    Converter.SetSpecialCase(19, CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Nineteen"));

                    RefreshSpecialCaseLookup();
                }

                // Thousand Powers
                {
                    SerializedProperty powersOfTenProperty = serializedObject.FindProperty("powersOfTen");
                    powersOfTenProperty.arraySize = 4;
                    powersOfTenProperty.GetArrayElementAtIndex(0).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Hundred");
                    powersOfTenProperty.GetArrayElementAtIndex(1).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Thousand");
                    powersOfTenProperty.GetArrayElementAtIndex(2).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Million");
                    powersOfTenProperty.GetArrayElementAtIndex(3).objectReferenceValue = CelesteEditor.Tools.EditorOnly.FindAsset<LocalisationKey>("Billion");
                }
            }

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();

            serializedObject.Update();
            
            specialCasesReorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void RefreshSpecialCaseLookup()
        {
            specialCasesLookup.Clear();

            foreach (var specialCase in Converter.SpecialCases)
            {
                specialCasesLookup.Add(new ValueTuple<int, LocalisationKey>(specialCase.Key, specialCase.Value));
            }
        }

        #region Callbacks

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Special Cases", CelesteGUIStyles.BoldLabel);
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var specialCaseElement = specialCasesLookup[index];
            var newLocalisationKey = EditorGUI.ObjectField(rect, new GUIContent(specialCaseElement.Item1.ToString()), specialCaseElement.Item2, typeof(LocalisationKey), false);

            if (newLocalisationKey != specialCaseElement.Item2)
            {
                specialCasesLookup[index] = new ValueTuple<int, LocalisationKey>(specialCaseElement.Item1, newLocalisationKey as LocalisationKey);
                Converter.SetSpecialCase(specialCaseElement.Item1, newLocalisationKey as LocalisationKey);
            }
        }

        #endregion
    }
}
