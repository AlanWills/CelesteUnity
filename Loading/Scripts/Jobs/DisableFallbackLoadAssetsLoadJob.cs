using Celeste.Assets;
using Celeste.Coroutines;
using Celeste.Parameters;
using Celeste.Scene;
using Celeste.Scene.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(DisableFallbackLoadAssetsLoadJob), menuName = "Celeste/Loading/Load Jobs/Disable Fallback Load Assets")]
    public class DisableFallbackLoadAssetsLoadJob : LoadJob
    {
        #region Properties and Fields

        [SerializeField] private BoolValue shouldFallbackLoadAssets;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            shouldFallbackLoadAssets.Value = false;

            yield break;
        }
    }
}