using Celeste;
using Celeste.Localisation;
using Celeste.Tools;
using System;
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

        #endregion

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Try Set Up Values"))
            {
                // And
                {
                    SerializedProperty andProperty = serializedObject.FindProperty("and");
                    andProperty.objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("And");
                }

                // Negative
                {
                    SerializedProperty negativeProperty = serializedObject.FindProperty("negative");
                    negativeProperty.objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Negative");
                }

                // 0 - 9
                {
                    SerializedProperty zeroToNineProperty = serializedObject.FindProperty("zeroToNine");
                    zeroToNineProperty.arraySize = 10;
                    zeroToNineProperty.GetArrayElementAtIndex(0).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Zero");
                    zeroToNineProperty.GetArrayElementAtIndex(1).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("One");
                    zeroToNineProperty.GetArrayElementAtIndex(2).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Two");
                    zeroToNineProperty.GetArrayElementAtIndex(3).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Three");
                    zeroToNineProperty.GetArrayElementAtIndex(4).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Four");
                    zeroToNineProperty.GetArrayElementAtIndex(5).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Five");
                    zeroToNineProperty.GetArrayElementAtIndex(6).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Six");
                    zeroToNineProperty.GetArrayElementAtIndex(7).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Seven");
                    zeroToNineProperty.GetArrayElementAtIndex(8).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Eight");
                    zeroToNineProperty.GetArrayElementAtIndex(9).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Nine");
                }

                // 10 - 100
                {
                    SerializedProperty tenToNinetyProperty = serializedObject.FindProperty("tenToNinety");
                    tenToNinetyProperty.arraySize = 9;
                    tenToNinetyProperty.GetArrayElementAtIndex(0).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Ten");
                    tenToNinetyProperty.GetArrayElementAtIndex(1).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Twenty");
                    tenToNinetyProperty.GetArrayElementAtIndex(2).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Thirty");
                    tenToNinetyProperty.GetArrayElementAtIndex(3).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Fourty");
                    tenToNinetyProperty.GetArrayElementAtIndex(4).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Fifty");
                    tenToNinetyProperty.GetArrayElementAtIndex(5).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Sixty");
                    tenToNinetyProperty.GetArrayElementAtIndex(6).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Seventy");
                    tenToNinetyProperty.GetArrayElementAtIndex(7).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Eighty");
                    tenToNinetyProperty.GetArrayElementAtIndex(8).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Ninety");
                }

                // Special Cases
                {
                    Converter.SetSpecialCase(11, EditorOnly.FindAsset<LocalisationKey>("Eleven"));
                    Converter.SetSpecialCase(12, EditorOnly.FindAsset<LocalisationKey>("Twelve"));
                    Converter.SetSpecialCase(13, EditorOnly.FindAsset<LocalisationKey>("Thirteen"));
                    Converter.SetSpecialCase(14, EditorOnly.FindAsset<LocalisationKey>("Fourteen"));
                    Converter.SetSpecialCase(15, EditorOnly.FindAsset<LocalisationKey>("Fifteen"));
                    Converter.SetSpecialCase(16, EditorOnly.FindAsset<LocalisationKey>("Sixteen"));
                    Converter.SetSpecialCase(17, EditorOnly.FindAsset<LocalisationKey>("Seventeen"));
                    Converter.SetSpecialCase(18, EditorOnly.FindAsset<LocalisationKey>("Eighteen"));
                    Converter.SetSpecialCase(19, EditorOnly.FindAsset<LocalisationKey>("Nineteen"));
                }

                // Thousand Powers
                {
                    SerializedProperty powersOfTenProperty = serializedObject.FindProperty("powersOfTen");
                    powersOfTenProperty.arraySize = 4;
                    powersOfTenProperty.GetArrayElementAtIndex(0).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Hundred");
                    powersOfTenProperty.GetArrayElementAtIndex(1).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Thousand");
                    powersOfTenProperty.GetArrayElementAtIndex(2).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Million");
                    powersOfTenProperty.GetArrayElementAtIndex(3).objectReferenceValue = EditorOnly.FindAsset<LocalisationKey>("Billion");
                }
            }

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
