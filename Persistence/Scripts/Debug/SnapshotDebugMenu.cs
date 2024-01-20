using Celeste.Debug.Menus;
using Celeste.Log;
using Celeste.Persistence.Snapshots;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence.Debug
{
    [CreateAssetMenu(fileName = nameof(SnapshotDebugMenu), menuName = "Celeste/Persistence/Debug/Snapshot Debug Menu")]
    public class SnapshotDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private SnapshotRecord snapshotRecord;
        [SerializeField] private string snapshotCreationFolder = "Assets/Snapshots";

        private Dictionary<string, bool> snapshotListFoldout = new Dictionary<string, bool>();

        #endregion

        protected override void OnShowMenu()
        {
            base.OnShowMenu();

            snapshotListFoldout.Clear();

            for (int i = 0, n = snapshotRecord.NumBakedSnapshotLists; i < n; i++)
            {
                snapshotListFoldout.Add(snapshotRecord.GetSnapshotList(i).name, false);
            }

            snapshotListFoldout[snapshotRecord.RuntimeSnapshots.name] = false;
        }

        protected override void OnDrawMenu()
        {
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Create Data Snapshot"))
                {
                    // Create data snapshot - if in editor create it in project, otherwise create it in persistent data as json
                    DataSnapshot dataSnapshot = snapshotRecord.CreateDataSnapshot();

#if UNITY_EDITOR
                    if (Application.isEditor)
                    {
                        string snapshotPath = Path.Combine(snapshotCreationFolder, $"{dataSnapshot.name}.asset");
                        UnityEditor.AssetDatabase.CreateAsset(dataSnapshot, snapshotPath);
                    }
                    else
#endif
                    {
                        string snapshotAsJson = PersistenceUtility.Serialize(dataSnapshot);
                        string path = Path.Combine(Application.persistentDataPath, $"{dataSnapshot.name}.datasnapshot");
                        File.WriteAllText(path, snapshotAsJson);
                    }

                    HudLog.LogInfo($"Snapshot {dataSnapshot.name} created successfully!");
                    snapshotRecord.RuntimeSnapshots.AddItem(dataSnapshot);
                }
            }

            for (int i = 0, n = snapshotRecord.NumBakedSnapshotLists; i < n; i++)
            {
                DrawSnapshotList(snapshotRecord.GetSnapshotList(i));
            }

            DrawSnapshotList(snapshotRecord.RuntimeSnapshots);
        }

        private void DrawSnapshotList(SnapshotList snapshotList)
        {
            bool foldedOut = snapshotListFoldout[snapshotList.name];
            bool newFoldedOut = GUILayout.Toggle(foldedOut, snapshotList.name);

            if (foldedOut != newFoldedOut)
            {
                snapshotListFoldout[snapshotList.name] = newFoldedOut;
            }

            if (newFoldedOut)
            {
                for (int i = 0; i < snapshotList.NumItems; ++i)
                {
                    Snapshot snapshot = snapshotList.GetItem(i);

                    using (var horizontal = new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label(snapshot.name);

                        if (GUILayout.Button("Load", GUILayout.ExpandWidth(false)))
                        {
                            snapshot.UnpackItems();
                        }

                        if (GUILayout.Button("Share", GUILayout.ExpandWidth(false)))
                        {
                            string snapshotString = PersistenceUtility.Serialize(snapshot);
                            string tempFilePath = Path.Combine(Application.persistentDataPath, "Temp.txt");
                            File.WriteAllText(tempFilePath, snapshotString);

                            new NativeShare()
                                .SetText($"{snapshot.name}")
                                .SetSubject($"Share Live Ops Save")
                                .SetCallback((result, shareTarget) =>
                                    {
                                        HudLog.LogInfo($"Share result: {result}, selected app: {shareTarget}");
                                        File.Delete(tempFilePath);
                                    })
                                .AddFile(tempFilePath)
                                .Share();
                        }
                    }
                }
            }
        }
    }
}
