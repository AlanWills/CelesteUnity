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
        #region Builder

        public class Builder
        {
            private SceneSet sceneSet;
            private LoadSceneMode loadSceneMode = LoadSceneMode.Single;
            private OnContextLoadedEvent onContextLoadedEvent;
            private Context context;

            public Builder WithSceneSet(SceneSet sceneSet)
            {
                this.sceneSet = sceneSet;
                return this;
            }

            public Builder WithLoadSceneMode(LoadSceneMode loadSceneMode)
            {
                this.loadSceneMode = loadSceneMode;
                return this;
            }

            public Builder WithOnContextLoadedEvent(OnContextLoadedEvent onContextLoadedEvent)
            {
                this.onContextLoadedEvent = onContextLoadedEvent;
                return this;
            }

            public Builder WithContext(Context context)
            {
                this.context = context;
                return this;
            }

            public LoadSceneSetLoadJob Build()
            {
                LoadSceneSetLoadJob loadSceneSetLoadJob = CreateInstance<LoadSceneSetLoadJob>();
                loadSceneSetLoadJob.name = nameof(LoadSceneSetLoadJob);
                loadSceneSetLoadJob.sceneSet = sceneSet;
                loadSceneSetLoadJob.loadSceneMode = loadSceneMode;
                loadSceneSetLoadJob.onContextLoadedEvent = onContextLoadedEvent;
                loadSceneSetLoadJob.context = context;

                return loadSceneSetLoadJob;
            }
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private SceneSet sceneSet;
        [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        [SerializeField] private bool unloadUnusedAssets = true;
        
        private OnContextLoadedEvent onContextLoadedEvent;
        private Context context;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            yield return sceneSet.LoadAsync(loadSceneMode, setProgress, () =>
            { 
                if (onContextLoadedEvent != null)
                {
                    onContextLoadedEvent.Invoke(new OnContextLoadedArgs(context));
                }
            });

            if (unloadUnusedAssets)
            {
                yield return Resources.UnloadUnusedAssets();
            }
        }
    }
}