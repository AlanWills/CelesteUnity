using Celeste.Localisation;
using CelesteEditor.Tools;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Localisation
{
    [CustomEditor(typeof(Verb))]
    public class VerbEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Create Missing Localisation Keys"))
            {
                string targetDirectory = EditorOnly.GetAssetFolderPath(target);
                Type verbType = typeof(Verb);
                Type localisationKeyType = typeof(LocalisationKey);

                foreach (var fieldInfo in verbType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if ((fieldInfo.IsPublic || fieldInfo.GetCustomAttribute<SerializeField>() != null) &&
                        fieldInfo.FieldType == localisationKeyType &&
                        fieldInfo.GetValue(target) == null)
                    {
                        LocalisationKey fieldLocalisationKey = CreateInstance<LocalisationKey>();
                        fieldLocalisationKey.name = fieldInfo.Name;

                        AssetDatabase.CreateAsset(fieldLocalisationKey, Path.Combine(targetDirectory, $"{fieldInfo.Name}.asset"));
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        fieldInfo.SetValue(target, fieldLocalisationKey);

                        EditorUtility.SetDirty(target);
                    }
                }
                
                AssetDatabase.SaveAssets();
            }

            base.OnInspectorGUI();
        }
    }
}