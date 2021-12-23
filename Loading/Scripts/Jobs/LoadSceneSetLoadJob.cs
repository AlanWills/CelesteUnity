using Celeste.Scene;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(LoadSceneSetLoadJob), menuName = "Celeste/Loading/Load Jobs/Load Scene Set")]
    public class LoadSceneSetLoadJob : LoadJob
    {
        [SerializeField] private SceneSet sceneSet;
        [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            yield return sceneSet.LoadAsync(loadSceneMode, setProgress, () => { });
        }
    }
}