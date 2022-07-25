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
    [CustomEditor(typeof(DutchNumberToLocalisedTextConverter))]
    public class DutchNumberToLocalisedTextConverterEditor : Editor
    {
        #region Properties and Fields

        private DutchNumberToLocalisedTextConverter Converter => target as DutchNumberToLocalisedTextConverter;

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
                    andProperty.objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("And");
                }

                // Negative
                {
                    SerializedProperty negativeProperty = serializedObject.FindProperty("negative");
                    negativeProperty.objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Negative");
                }

                // 0 - 9
                {
                    SerializedProperty zeroToNineProperty = serializedObject.FindProperty("zeroToNine");
                    zeroToNineProperty.arraySize = 10;
                    zeroToNineProperty.GetArrayElementAtIndex(0).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Zero");
                    zeroToNineProperty.GetArrayElementAtIndex(1).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("One");
                    zeroToNineProperty.GetArrayElementAtIndex(2).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Two");
                    zeroToNineProperty.GetArrayElementAtIndex(3).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Three");
                    zeroToNineProperty.GetArrayElementAtIndex(4).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Four");
                    zeroToNineProperty.GetArrayElementAtIndex(5).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Five");
                    zeroToNineProperty.GetArrayElementAtIndex(6).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Six");
                    zeroToNineProperty.GetArrayElementAtIndex(7).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Seven");
                    zeroToNineProperty.GetArrayElementAtIndex(8).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Eight");
                    zeroToNineProperty.GetArrayElementAtIndex(9).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Nine");
                }

                // 10 - 100
                {
                    SerializedProperty tenToNinetyProperty = serializedObject.FindProperty("tenToNinety");
                    tenToNinetyProperty.arraySize = 9;
                    tenToNinetyProperty.GetArrayElementAtIndex(0).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Ten");
                    tenToNinetyProperty.GetArrayElementAtIndex(1).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Twenty");
                    tenToNinetyProperty.GetArrayElementAtIndex(2).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Thirty");
                    tenToNinetyProperty.GetArrayElementAtIndex(3).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Fourty");
                    tenToNinetyProperty.GetArrayElementAtIndex(4).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Fifty");
                    tenToNinetyProperty.GetArrayElementAtIndex(5).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Sixty");
                    tenToNinetyProperty.GetArrayElementAtIndex(6).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Seventy");
                    tenToNinetyProperty.GetArrayElementAtIndex(7).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Eighty");
                    tenToNinetyProperty.GetArrayElementAtIndex(8).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Ninety");
                }

                // Special Cases
                {
                    Converter.SetSpecialCase(11, AssetUtility.FindAsset<LocalisationKey>("Eleven"));
                    Converter.SetSpecialCase(12, AssetUtility.FindAsset<LocalisationKey>("Twelve"));
                    Converter.SetSpecialCase(13, AssetUtility.FindAsset<LocalisationKey>("Thirteen"));
                    Converter.SetSpecialCase(14, AssetUtility.FindAsset<LocalisationKey>("Fourteen"));
                    Converter.SetSpecialCase(15, AssetUtility.FindAsset<LocalisationKey>("Fifteen"));
                    Converter.SetSpecialCase(16, AssetUtility.FindAsset<LocalisationKey>("Sixteen"));
                    Converter.SetSpecialCase(17, AssetUtility.FindAsset<LocalisationKey>("Seventeen"));
                    Converter.SetSpecialCase(18, AssetUtility.FindAsset<LocalisationKey>("Eighteen"));
                    Converter.SetSpecialCase(19, AssetUtility.FindAsset<LocalisationKey>("Nineteen"));

                    RefreshSpecialCaseLookup();
                }

                // Thousand Powers
                {
                    SerializedProperty powersOfTenProperty = serializedObject.FindProperty("powersOfTen");
                    powersOfTenProperty.arraySize = 4;
                    powersOfTenProperty.GetArrayElementAtIndex(0).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Hundred");
                    powersOfTenProperty.GetArrayElementAtIndex(1).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Thousand");
                    powersOfTenProperty.GetArrayElementAtIndex(2).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Million");
                    powersOfTenProperty.GetArrayElementAtIndex(3).objectReferenceValue = AssetUtility.FindAsset<LocalisationKey>("Billion");
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
