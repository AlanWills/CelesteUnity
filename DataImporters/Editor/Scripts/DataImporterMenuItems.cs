using Celeste.DataImporters.Settings;
using Unity.EditorCoroutines.Editor;
using UnityEditor;

namespace CelesteEditor.DataImporters
{
    public static class DataImporterMenuItems
    {
        [MenuItem("Celeste/Data Importers/Import All", validate = true)]
        public static bool CanImportAll()
        {
            return DataImporterEditorSettings.GetOrCreateSettings().dataImporterCatalogue != null;
        }

        [MenuItem("Celeste/Data Importers/Import All", validate = false)]
        public static void ImportAll()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(DataImporterEditorSettings.GetOrCreateSettings().dataImporterCatalogue.ImportAll());
        }
    }
}
