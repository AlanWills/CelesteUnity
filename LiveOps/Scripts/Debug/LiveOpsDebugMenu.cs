using Celeste.Core;
using Celeste.Debug.Menus;
using System;
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
            for (int i = 0, n = liveOpsRecord.NumLiveOps; i < n; i++)
            {
                LiveOp liveOp = liveOpsRecord.GetLiveOp(i);

                GUILayout.Label($"Type: {liveOp.Type}");

                DateTimeOffset startTime = GameTime.ToDateTimeOffset(liveOp.StartTimestamp);
                GUILayout.Label($"UTC Start Time: {startTime}");
                GUILayout.Label($"Local Start Time: {startTime.ToLocalTime()}");
            }
        }
    }
}
