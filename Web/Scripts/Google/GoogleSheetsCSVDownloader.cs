using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Celeste.Web
{
    public static class GoogleSheetsCSVDownloader
    {
        #region Properties and Fields

        private const string urlFormat = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv";

        #endregion

        public static IEnumerator DownloadData(string sheetId, System.Action<string> onCompleted)
        {
            string url = string.Format(urlFormat, sheetId);
            string downloadData = null;

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                Debug.Log("Starting Download...");
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("...Download Error: " + webRequest.error);
                    downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
                }
                else
                {
                    Debug.Log("Download Success!");
                    downloadData = webRequest.downloadHandler.text;
                }
            }

            onCompleted(downloadData);
        }
    }
}