using Celeste.Assets;
using Celeste.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps.UI
{
    [AddComponentMenu("Celeste/Live Ops/UI/Live Op UI Spawner")]
    public class LiveOpUISpawner : MonoBehaviour
    {
        #region Properties and Fields

        private List<ValueTuple<LiveOp, ILoadRequest<GameObject>>> spawningUI = new List<(LiveOp, ILoadRequest<GameObject>)>();
        private List<ValueTuple<LiveOp, GameObject>> spawnedUI = new List<(LiveOp, GameObject)>();

        #endregion

        private void TrySpawnUI(LiveOp liveOp)
        {
            if (!spawningUI.Exists(x => x.Item1 == liveOp) &&
                !spawnedUI.Exists(x => x.Item1 == liveOp) &&
                liveOp.TryFindComponent<ILiveOpUI>(out var ui) &&
                liveOp.TryFindComponent<ILiveOpAssets>(out var assets))
            {
                StartCoroutine(LoadUI(ui, assets, liveOp));
            }
        }

        private IEnumerator LoadUI(
            InterfaceHandle<ILiveOpUI> ui,
            InterfaceHandle<ILiveOpAssets> assets,
            LiveOp liveOp)
        {
            // If the live op is running, we add the UI
            UnityEngine.Debug.Assert(!spawnedUI.Exists(x => x.Item1 == liveOp), $"UI already spawned for live op - the liveop may have been accidentally duplicated.");
            ILoadRequest<GameObject> loadRequest = ui.iFace.LoadUI(ui.instance, assets.iFace, transform);
            spawningUI.Add((liveOp, loadRequest));

            yield return loadRequest;

            int spawningIndex = spawningUI.FindIndex(x => x.Item1 == liveOp);
            spawningUI.RemoveAt(spawningIndex);
            spawnedUI.Add((liveOp, loadRequest.Asset));
        }

        private void TryRemoveUI(LiveOp liveOp)
        {
            // Remove spawning UI if it exists
            {
                int index = spawningUI.FindIndex(x => x.Item1 == liveOp);
                if (index >= 0)
                {
                    StopCoroutine(spawningUI[index].Item2);
                    spawningUI.RemoveAt(index);
                }
            }

            // Remove spawned UI if it exists
            {
                int index = spawnedUI.FindIndex(x => x.Item1 == liveOp);
                if (index >= 0)
                {
                    Destroy(spawnedUI[index].Item2);
                    spawnedUI.RemoveAt(index);
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
                TrySpawnUI(liveOp);
            }
            else
            {
                TryRemoveUI(liveOp);
            }
        }

        #endregion
    }
}
