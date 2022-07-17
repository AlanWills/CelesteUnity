using Celeste.Assets;
using Celeste.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps.UI
{
    [AddComponentMenu("Celeste/Live Ops/UI/Live Op Widget Spawner")]
    public class LiveOpWidgetSpawner : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LiveOpsRecord liveOps;

        private List<ValueTuple<LiveOp, ILoadRequest<GameObject>>> spawningWidgets = new List<(LiveOp, ILoadRequest<GameObject>)>();
        private List<ValueTuple<LiveOp, GameObject>> spawnedWidgets = new List<(LiveOp, GameObject)>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            for (int i = 0, n = liveOps.NumLiveOps; i < n; ++i)
            {
                TrySpawnWidget(liveOps.GetLiveOp(i));
            }
        }

        #endregion

        private void TrySpawnWidget(LiveOp liveOp)
        {
            if (!spawningWidgets.Exists(x => x.Item1 == liveOp) &&
                !spawnedWidgets.Exists(x => x.Item1 == liveOp) &&
                liveOp.TryFindComponent<ILiveOpWidget>(out var widget) &&
                liveOp.TryFindComponent<ILiveOpAssets>(out var assets))
            {
                StartCoroutine(LoadWidget(widget, assets, liveOp));
            }
        }

        private IEnumerator LoadWidget(
            InterfaceHandle<ILiveOpWidget> widget, 
            InterfaceHandle<ILiveOpAssets> assets,
            LiveOp liveOp)
        {
            // If the live op is running, we add the widget
            UnityEngine.Debug.Assert(!spawnedWidgets.Exists(x => x.Item1 == liveOp), $"Widget already spawned for live op - the liveop may have been accidentally duplicated.");
            ILoadRequest<GameObject> loadRequest = widget.iFace.LoadWidget(widget.instance, assets.iFace, transform);
            spawningWidgets.Add((liveOp, loadRequest));

            yield return loadRequest;

            int spawningIndex = spawningWidgets.FindIndex(x => x.Item1 == liveOp);
            spawningWidgets.RemoveAt(spawningIndex);
            spawnedWidgets.Add((liveOp, loadRequest.Asset));
        }

        private void TryRemoveWidget(LiveOp liveOp)
        {
            // Remove spawning widget if it exists
            {
                int index = spawningWidgets.FindIndex(x => x.Item1 == liveOp);
                if (index >= 0)
                {
                    StopCoroutine(spawningWidgets[index].Item2);
                    spawningWidgets.RemoveAt(index);
                }
            }

            // Remove spawned widget if it exists
            {
                int index = spawnedWidgets.FindIndex(x => x.Item1 == liveOp);
                if (index >= 0)
                {
                    Destroy(spawnedWidgets[index].Item2);
                    spawnedWidgets.RemoveAt(index);
                }
            }
        }

        #region Callbacks

        public void OnLiveOpAdded(LiveOp liveOp)
        {
            OnLiveOpStateChanged(liveOp);
        }

        public void OnLiveOpStateChanged(LiveOp liveOp)
        {
            if (liveOp.State == LiveOpState.Running ||
                liveOp.State == LiveOpState.Completed)
            {
                TrySpawnWidget(liveOp);
            }
            else
            {
                TryRemoveWidget(liveOp);
            }
        }

        #endregion
    }
}
