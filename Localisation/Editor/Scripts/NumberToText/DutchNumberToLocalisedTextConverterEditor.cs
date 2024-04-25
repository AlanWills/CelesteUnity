using Celeste.Localisation;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Localisation
{
    [CustomEditor(typeof(DutchNumberToLocalisedTextConverter))]
    public class DutchNumberToLocalisedTextConverterEditor : Editor
    {
        #region Properties and Fields

        private DutchNumberToLocalisedTextConverter Converter => target as DutchNumberToLocalisedTextConverter;

        #endregion

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
        }
    }
}
