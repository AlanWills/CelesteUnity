using Celeste.Persistence.Snapshots;
using Celeste.Tools.Settings;

namespace Celeste.Persistence.Settings
{
    public class PersistenceEditorSettings : EditorSettings<PersistenceEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Persistence/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "PersistenceEditorSettings.asset";

        public SnapshotRecord snapshotRecord;

        #endregion

#if UNITY_EDITOR
        public static PersistenceEditorSettings GetOrCreateSettings()
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
