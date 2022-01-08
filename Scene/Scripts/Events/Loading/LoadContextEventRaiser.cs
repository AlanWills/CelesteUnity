using Celeste.Events;
using UnityEngine;

namespace Celeste.Scene.Events
{
    [AddComponentMenu("Celeste/Events/Loading/Load Context Event Raiser")]
    public class LoadContextEventRaiser : ParameterisedEventRaiser<LoadContextArgs, LoadContextEvent>
    {
        #region Properties and Fields

        [SerializeField] private SceneSet sceneSetToLoad;
        [SerializeField] private ContextProvider contextProvider;
        [SerializeField] private OnContextLoadedEvent onContextLoaded;

        #endregion

        public void Raise()
        {
            Raise(new LoadContextArgs(sceneSetToLoad, contextProvider.Create(), onContextLoaded));
        }
    }
}
