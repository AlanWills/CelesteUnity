using Celeste.Tools.Settings;
using System.IO;
using UnityEngine;

namespace Celeste.DataImporters.Settings
{
    [CreateAssetMenu(fileName = nameof(DataImporterEditorSettings), menuName = "Celeste/Data Importers/Data Importer Editor Settings")]
    public class DataImporterEditorSettings : EditorSettings<DataImporterEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/DataImporters/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "DataImporterEditorSettings.asset";

        public DataImporterCatalogue dataImporterCatalogue;

        #endregion

#if UNITY_EDITOR
        public static DataImporterEditorSettings GetOrCreateSettings()
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
