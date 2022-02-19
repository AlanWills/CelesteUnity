using Celeste.Assets;
using Celeste.Coroutines;
using Celeste.Scene;
using Celeste.Scene.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(LoadSceneSetLoadJob), menuName = "Celeste/Loading/Load Jobs/Load Scene Set")]
    public class LoadSceneSetLoadJob : LoadJob
    {
        #region Properties and Fields

        [SerializeField] private SceneSet sceneSet;
        [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        [SerializeField] private bool unloadUnusedAssets = true;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            yield return sceneSet.LoadAsync(loadSceneMode, setProgress, () => { });

            if (unloadUnusedAssets)
            {
                yield return Resources.UnloadUnusedAssets();
            }
        }
    }
}