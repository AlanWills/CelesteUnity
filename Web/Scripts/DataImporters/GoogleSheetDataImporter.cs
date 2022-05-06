using Celeste.DataImporters;
using System.Collections;
using UnityEngine;

namespace Celeste.Web.DataImporters
{
    public abstract class GoogleSheetDataImporter : DataImporter
    {
        #region Properties and Fields

        [SerializeField] private string sheetId;
        [SerializeField] private string tabId;

        #endregion

        public override IEnumerator Import()
        {
            yield return GoogleSheetsCSVDownloader.DownloadData(sheetId, tabId, OnDownloadData);
        }

        protected abstract void OnDataReceived(GoogleSheet googleSheet);

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

            OnDataReceived(googleSheet);
            Debug.Log($"{name}: Import done!", this);
        }
    }
}
