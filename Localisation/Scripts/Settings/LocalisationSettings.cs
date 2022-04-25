#if UNITY_EDITOR
using Celeste.Localisation.Parameters;
using Celeste.Localisation.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Celeste.Localisation.Settings
{
    [CreateAssetMenu(fileName = nameof(LocalisationSettings), menuName = "Celeste/Localisation/Localisation Settings")]
    public class LocalisationSettings : ScriptableObject
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Localisation/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "LocalisationSettings.asset";

        public LanguageValue currentLanguageValue;
        public int localisationSheetLanguagesOffset = 3;
        public List<LocalisationPostImportStep> postImportSteps = new List<LocalisationPostImportStep>();

        #endregion

        public static LocalisationSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<LocalisationSettings>(FILE_PATH);
            if (settings == null)
            {
                settings = CreateInstance<LocalisationSettings>();
                
                Directory.CreateDirectory(FOLDER_PATH);
                AssetDatabase.Refresh();
                AssetDatabase.CreateAsset(settings, FILE_PATH);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}
#endif