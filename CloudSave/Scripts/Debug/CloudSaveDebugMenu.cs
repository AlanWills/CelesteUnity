using Celeste.Coroutines;
using Celeste.Debug.Menus;
using Celeste.Persistence.Snapshots;
using Celeste.Scene;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.CloudSave
{
    [CreateAssetMenu(fileName = nameof(CloudSaveDebugMenu), menuName = "Celeste/Cloud Save/Debug/Cloud Save Debug Menu")]
    public class CloudSaveDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private SnapshotRecord snapshotRecord;
        [SerializeField] private CloudSaveRecord cloudSave;
        [SerializeField] private SceneSet startupSceneSet;

        #endregion

        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Playtime Start: {cloudSave.PlaytimeStart}");

            if (GUILayout.Button("Authenticate"))
            {
                CoroutineManager.Instance.StartCoroutine(cloudSave.AuthenticateAsync());
            }

            if (GUILayout.Button("Load Save Game"))
            {
                CoroutineManager.Instance.StartCoroutine(cloudSave.ReadDefaultSaveGameAsync(
                    (saveGameString) =>
                    {
                        DataSnapshot snapshot = CreateInstance<DataSnapshot>();
                        JsonUtility.FromJsonOverwrite(saveGameString, snapshot);
                        snapshot.UnpackItems();

                        CoroutineManager.Instance.StartCoroutine(
                            startupSceneSet.LoadAsync(LoadSceneMode.Single, (f) => { }, (s) => { }, () => { }));
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
