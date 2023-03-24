using System;
using Celeste.DataImporters;
using Celeste.Web.ImportSteps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Web.DataImporters
{
    [CreateAssetMenu(fileName = nameof(GoogleSheetDataImporter), menuName = "Celeste/Data Importers/Google Sheet Data Importer")]
    public class GoogleSheetDataImporter : DataImporter
    {
        #region Properties and Fields

        [SerializeField] private string sheetId;
        [SerializeField] private string tabId;
        [SerializeField] private List<GoogleSheetReceivedImportStep> onGoogleSheetReceivedImportSteps = new List<GoogleSheetReceivedImportStep>();

        #endregion

        protected override IEnumerator DoImport(Action<string, float> progressCallback = null)
        {
            yield return GoogleSheetsCSVDownloader.DownloadData(sheetId, tabId, OnDownloadData);
        }

        private void OnDownloadData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                Debug.LogError($"Importer {name} did not receive any data from sheet id: {sheetId} and tab id {tabId}.");
                return;
            }

            GoogleSheet googleSheet = GoogleSheet.FromCSV(data);
            
            if (googleSheet == null)
            {
                Debug.LogError($"Importer {name} could not create correct CSV data from sheet id: {sheetId} and tab id: {tabId}.");
                return;
            }

            for (int i = 0; i < onGoogleSheetReceivedImportSteps.Count; i++)
            {
                onGoogleSheetReceivedImportSteps[i].Execute(googleSheet);
            }

            Debug.Log($"{name}: Import done!", this);
        }
    }
}
