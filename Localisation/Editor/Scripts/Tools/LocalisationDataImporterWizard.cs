using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using Celeste.Web;
using System;

namespace CelesteEditor.Localisation.Tools
{
    public class LocalisationDataImporterWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField] private string sheetId;

        private const string LOCALISATION_SHEET_ID = "LocalisationSheetId";

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Localisation Data Importer")]
        public static void ShowLocalisationDataImporterWizard()
        {
            DisplayWizard<LocalisationDataImporterWizard>("Localisation Data Importer", "Close", "Import");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            sheetId = EditorPrefs.GetString(LOCALISATION_SHEET_ID);
        }

        protected override bool DrawWizardGUI()
        {
            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                sheetId = EditorGUILayout.TextField("Sheet Id", sheetId);
                
                if (changeCheck.changed)
                {
                    EditorPrefs.SetString(LOCALISATION_SHEET_ID, sheetId);
                }

                return changeCheck.changed;
            }
        }

        private void OnWizardOtherButton()
        {
            EditorCoroutineUtility.StartCoroutine(GoogleSheetsCSVDownloader.DownloadData(sheetId, OnDownloadData), this);
        }

        private void OnWizardCreate()
        {
            Close();
        }

        #endregion

        #region Callbacks

        private void OnDownloadData(string data)
        {
            GoogleSheet googleSheet = GoogleSheet.FromCSV(data);
            Debug.Log(googleSheet.NumColumns);
        }

        #endregion
    }
}