using Celeste.Core;
using Celeste.Debug.Menus;
using Celeste.LiveOps.Persistence;
using Celeste.Log;
using Celeste.Persistence;
using System;
using System.IO;
using UnityEngine;

namespace Celeste.LiveOps.Debug
{
    [CreateAssetMenu(fileName = nameof(LiveOpsDebugMenu), menuName = "Celeste/Live Ops/Debug/Live Ops Debug Menu")]
    public class LiveOpsDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private LiveOpsRecord liveOpsRecord;

        #endregion

        protected override void OnDrawMenu()
        {
            using (var horizontal = new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Delete Save"))
                {
                    PersistenceUtility.DeletePersistentDataFile(LiveOpsManager.FILE_NAME);
                }

                if (GUILayout.Button($"Share Save"))
                {
                    string liveOpPath = Path.Combine(Application.persistentDataPath, LiveOpsManager.FILE_NAME);
                    new NativeShare()
                        .AddFile(Path.Combine(Application.persistentDataPath, liveOpPath))
                        .SetSubject($"Share Live Ops Save")
                        .SetCallback((result, shareTarget) => HudLog.LogInfo($"Share result: {result}, selected app: {shareTarget}"))
                        .Share();
                }
            }

            if (GUILayout.Button("Remove All Liveops"))
            {
                liveOpsRecord.RemoveAllLiveOps();
            }

            for (int i = liveOpsRecord.NumLiveOps - 1; i >= 0; --i)
            {
                LiveOp liveOp = liveOpsRecord.GetLiveOp(i);

                GUILayout.Label($"Type: {liveOp.Type}");
                GUILayout.Label($"SubType: {liveOp.SubType}");
                GUILayout.Label($"State: {liveOp.State}");

                DateTimeOffset startTime = GameTime.ToDateTimeOffset(liveOp.StartTimestamp);
                GUILayout.Label($"UTC Start Time: {startTime}");
                GUILayout.Label($"Local Start Time: {startTime.ToLocalTime()}");

                DateTimeOffset endTime = GameTime.ToDateTimeOffset(liveOp.EndTimestamp);
                GUILayout.Label($"UTC End Time: {endTime}");
                GUILayout.Label($"Local End Time: {endTime.ToLocalTime()}");

                using (var horizontal = new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(liveOp.State.ToString());

                    if (GUILayout.Button("Complete", GUILayout.ExpandWidth(false)))
                    {
                        liveOp.Complete();
                    }

                    if (GUILayout.Button("Finish", GUILayout.ExpandWidth(false)))
                    {
                        liveOp.Finish();
                    }

                    if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                    {
                        liveOpsRecord.RemoveLiveOp(i);
                    }
                }
            }
        }
    }
}
