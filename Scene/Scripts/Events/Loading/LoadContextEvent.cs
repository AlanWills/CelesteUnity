using Celeste.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Scene.Events
{
    [Serializable]
    public struct LoadContextArgs
    {
        public SceneSet sceneSet;
        public bool showOutputOnLoadingScreen;
        public Context context;
        public OnContextLoadedEvent onContextLoaded;

        public LoadContextArgs(SceneSet sceneSet, bool showOutputOnLoadingScreen, Context context, OnContextLoadedEvent onContextLoaded)
        {
            this.sceneSet = sceneSet;
            this.showOutputOnLoadingScreen = showOutputOnLoadingScreen;
            this.context = context;
            this.onContextLoaded = onContextLoaded;
        }

        public override string ToString()
        {
            return $"{sceneSet.name} - {context} - {(onContextLoaded != null ? onContextLoaded.name : "null")}";
        }
    }

    [Serializable]
    public class Context { }

    [Serializable]
    public class LoadContextUnityEvent : UnityEvent<LoadContextArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = "LoadContextEvent", menuName = "Celeste/Events/Loading/Load Context Event")]
    public class LoadContextEvent : ParameterisedEvent<LoadContextArgs> { }
}
