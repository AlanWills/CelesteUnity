using Celeste.Localisation;
using Celeste.Tools;
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
                    andProperty.objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("And");
                }

                // Negative
                {
                    SerializedProperty negativeProperty = serializedObject.FindProperty("negative");
                    negativeProperty.objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Negative");
                }

                // 0 - 9
                {
                    SerializedProperty zeroToNineProperty = serializedObject.FindProperty("zeroToNine");
                    zeroToNineProperty.arraySize = 10;
                    zeroToNineProperty.GetArrayElementAtIndex(0).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Zero");
                    zeroToNineProperty.GetArrayElementAtIndex(1).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("One");
                    zeroToNineProperty.GetArrayElementAtIndex(2).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Two");
                    zeroToNineProperty.GetArrayElementAtIndex(3).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Three");
                    zeroToNineProperty.GetArrayElementAtIndex(4).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Four");
                    zeroToNineProperty.GetArrayElementAtIndex(5).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Five");
                    zeroToNineProperty.GetArrayElementAtIndex(6).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Six");
                    zeroToNineProperty.GetArrayElementAtIndex(7).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Seven");
                    zeroToNineProperty.GetArrayElementAtIndex(8).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Eight");
                    zeroToNineProperty.GetArrayElementAtIndex(9).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Nine");
                }

                // 10 - 100
                {
                    SerializedProperty tenToNinetyProperty = serializedObject.FindProperty("tenToNinety");
                    tenToNinetyProperty.arraySize = 9;
                    tenToNinetyProperty.GetArrayElementAtIndex(0).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Ten");
                    tenToNinetyProperty.GetArrayElementAtIndex(1).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Twenty");
                    tenToNinetyProperty.GetArrayElementAtIndex(2).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Thirty");
                    tenToNinetyProperty.GetArrayElementAtIndex(3).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Fourty");
                    tenToNinetyProperty.GetArrayElementAtIndex(4).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Fifty");
                    tenToNinetyProperty.GetArrayElementAtIndex(5).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Sixty");
                    tenToNinetyProperty.GetArrayElementAtIndex(6).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Seventy");
                    tenToNinetyProperty.GetArrayElementAtIndex(7).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Eighty");
                    tenToNinetyProperty.GetArrayElementAtIndex(8).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Ninety");
                }

                // Special Cases
                {
                    Converter.SetSpecialCase(11, EditorOnly.MustFindAsset<LocalisationKey>("Eleven"));
                    Converter.SetSpecialCase(12, EditorOnly.MustFindAsset<LocalisationKey>("Twelve"));
                    Converter.SetSpecialCase(13, EditorOnly.MustFindAsset<LocalisationKey>("Thirteen"));
                    Converter.SetSpecialCase(14, EditorOnly.MustFindAsset<LocalisationKey>("Fourteen"));
                    Converter.SetSpecialCase(15, EditorOnly.MustFindAsset<LocalisationKey>("Fifteen"));
                    Converter.SetSpecialCase(16, EditorOnly.MustFindAsset<LocalisationKey>("Sixteen"));
                    Converter.SetSpecialCase(17, EditorOnly.MustFindAsset<LocalisationKey>("Seventeen"));
                    Converter.SetSpecialCase(18, EditorOnly.MustFindAsset<LocalisationKey>("Eighteen"));
                    Converter.SetSpecialCase(19, EditorOnly.MustFindAsset<LocalisationKey>("Nineteen"));
                }

                // Thousand Powers
                {
                    SerializedProperty powersOfTenProperty = serializedObject.FindProperty("powersOfTen");
                    powersOfTenProperty.arraySize = 4;
                    powersOfTenProperty.GetArrayElementAtIndex(0).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Hundred");
                    powersOfTenProperty.GetArrayElementAtIndex(1).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Thousand");
                    powersOfTenProperty.GetArrayElementAtIndex(2).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Million");
                    powersOfTenProperty.GetArrayElementAtIndex(3).objectReferenceValue = EditorOnly.MustFindAsset<LocalisationKey>("Billion");
                }
            }

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
