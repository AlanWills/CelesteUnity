using Celeste.Coroutines;
using Celeste.Debug.Menus;
using Celeste.Persistence.Snapshots;
using Celeste.Tools;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.CloudSave
{
    [CreateAssetMenu(
        fileName = nameof(CloudSaveDebugMenu), 
        menuName = CelesteMenuItemConstants.CLOUDSAVE_MENU_ITEM + "Debug/Cloud Save Debug Menu",
        order = CelesteMenuItemConstants.CLOUDSAVE_MENU_ITEM_PRIORITY)]
    public class CloudSaveDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private SnapshotRecord snapshotRecord;
        [SerializeField] private CloudSaveRecord cloudSave;

        [NonSerialized] private DataSnapshot lastReadCloudSave;

        #endregion

        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Current Implementation: {cloudSave.ActiveImplementation}");
            GUILayout.Label($"Playtime Start: {cloudSave.PlaytimeStart}");

            using (new GUILayout.HorizontalScope())
            {
                using (new GUIEnabledScope(cloudSave.ActiveImplementation != Implementation.Disabled))
                {
                    if (GUILayout.Button("Disable"))
                    {
                        cloudSave.ActiveImplementation = Implementation.Disabled;
                    }
                }

                using (new GUIEnabledScope(cloudSave.ActiveImplementation == Implementation.Disabled))
                {
                    if (GUILayout.Button("Enable"))
                    {
                        cloudSave.ActiveImplementation = Implementation.PlatformAppropriate;
                    }
                }
            }

            using (new GUIEnabledScope(!cloudSave.IsAuthenticated))
            {
                if (GUILayout.Button("Authenticate"))
                {
                    CoroutineManager.Instance.StartCoroutine(cloudSave.AuthenticateAsync());
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Read Save"))
                {
                    CoroutineManager.Instance.StartCoroutine(cloudSave.ReadDefaultSaveGameAsync(
                        (saveGameString) =>
                        {
                            lastReadCloudSave = CreateInstance<DataSnapshot>();
                            JsonUtility.FromJsonOverwrite(saveGameString, lastReadCloudSave);
                        }));
                }

                if (GUILayout.Button("Overwrite Local Save"))
                {
                    CoroutineManager.Instance.StartCoroutine(cloudSave.ReadDefaultSaveGameAsync(
                        (saveGameString) =>
                        {
                            // Load the first scene in build settings (we assume it's the startup scene)
                            SceneManager.LoadScene(0, LoadSceneMode.Single);
                        }));
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Write Default Save"))
                {
                    Snapshot snapshot = snapshotRecord.CreateDataSnapshot();
                    string snapshotString = snapshot.Serialize();

                    CoroutineManager.Instance.StartCoroutine(cloudSave.WriteDefaultSaveGameAsync(snapshotString));
                }

                if (GUILayout.Button("Delete Default Save"))
                {
                    CoroutineManager.Instance.StartCoroutine(cloudSave.DeleteDefaultSaveGameAsync());
                }
            }

            if (lastReadCloudSave == null)
            {
                GUILayout.Label("No Cloud Save Read");
            }
            else
            {
                for (int i = 0, n = lastReadCloudSave.NumDataFiles; i < n; ++i)
                {
                    GUILayout.Label(lastReadCloudSave.GetUnpackPath(i), CelesteGUIStyles.BoldLabel);
                    GUILayout.Label(lastReadCloudSave.GetData(i));
                }
            }
        }
    }
}
