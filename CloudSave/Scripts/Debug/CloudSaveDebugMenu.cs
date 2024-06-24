using Celeste.Coroutines;
using Celeste.Debug.Menus;
using Celeste.Persistence.Snapshots;
using Celeste.Tools;
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

            if (GUILayout.Button("Load Save Game"))
            {
                CoroutineManager.Instance.StartCoroutine(cloudSave.ReadDefaultSaveGameAsync(
                    (saveGameString) =>
                    {
                        DataSnapshot snapshot = CreateInstance<DataSnapshot>();
                        JsonUtility.FromJsonOverwrite(saveGameString, snapshot);
                        snapshot.UnpackItems();

                        // Load the first scene in build settings (we assume it's the startup scene)
                        SceneManager.LoadScene(0, LoadSceneMode.Single);
                    }));
            }

            if (GUILayout.Button("Write Default Save Game"))
            {
                Snapshot snapshot = snapshotRecord.CreateDataSnapshot();
                string snapshotString = snapshot.Serialize();

                CoroutineManager.Instance.StartCoroutine(cloudSave.WriteDefaultSaveGameAsync(snapshotString));
            }

            if (GUILayout.Button("Delete Default Save Game"))
            {
                CoroutineManager.Instance.StartCoroutine(cloudSave.DeleteDefaultSaveGameAsync());
            }
        }
    }
}
