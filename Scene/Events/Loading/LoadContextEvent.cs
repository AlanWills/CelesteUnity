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
        public Context context;
        public OnContextLoadedEvent onContextLoaded;

        public LoadContextArgs(SceneSet sceneSet, Context context, OnContextLoadedEvent onContextLoaded)
        {
            this.sceneSet = sceneSet;
            this.context = context;
            this.onContextLoaded = onContextLoaded;
        }

        public override string ToString()
        {
            return $"{sceneSet.name} - {context} - {onContextLoaded.name}";
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
