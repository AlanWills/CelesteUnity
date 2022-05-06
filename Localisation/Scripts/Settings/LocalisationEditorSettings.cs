using Celeste.Localisation.Catalogue;
using Celeste.Localisation.Parameters;
using Celeste.Localisation.Tools;
using Celeste.Tools.Settings;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Celeste.Localisation.Settings
{
    [CreateAssetMenu(fileName = nameof(LocalisationEditorSettings), menuName = "Celeste/Localisation/Localisation Editor Settings")]
    public class LocalisationEditorSettings : EditorSettings<LocalisationEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Localisation/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "LocalisationEditorSettings.asset";

        public LanguageValue currentLanguageValue;
        public LocalisationKeyCatalogue localisationKeyCatalogue;
        public int localisationSheetLanguagesOffset = 3;
        public List<LocalisationPostImportStep> postImportSteps = new List<LocalisationPostImportStep>();

        #endregion

#if UNITY_EDITOR
        public static LocalisationEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        public static UnityEditor.SerializedObject GetSerializedSettings()
        {
            return GetSerializedSettings(FOLDER_PATH, FILE_PATH);
        }
#endif
    }
}