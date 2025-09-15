using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Celeste.Web
{
    public static class GoogleSheetsCSVDownloader
    {
        #region Properties and Fields

        private const string urlFormat = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";

        #endregion

        public static IEnumerator DownloadData(string sheetId, System.Action<string> onCompleted)
        {
            return DownloadData(urlFormat, "0", onCompleted);
        }

        public static IEnumerator DownloadData(string sheetId, string tabId, System.Action<string> onCompleted)
        {
            string url = string.Format(urlFormat, sheetId, tabId);
            string downloadData = null;

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                UnityEngine.Debug.Log("Starting Download...");
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    UnityEngine.Debug.Log("...Download Error: " + webRequest.error);
                    downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
                }
                else
                {
                    UnityEngine.Debug.Log("Download Success!");
                    downloadData = webRequest.downloadHandler.text;
                    PlayerPrefs.GetString("LastDataDownloaded");
                    PlayerPrefs.Save();
                }
            }

            onCompleted(downloadData);
        }
    }
}