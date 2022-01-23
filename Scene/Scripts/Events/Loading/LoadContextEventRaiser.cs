using Celeste.Events;
using Celeste.Scene.Settings;
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

        private void OnValidate()
        {
            if (gameEvent == null)
            {
                gameEvent = SceneSettings.instance.defaultLoadContextEvent;
            }

            if (contextProvider == null)
            {
                contextProvider = SceneSettings.instance.defaultContextProvider;
            }
        }

        public void Raise()
        {
            Raise(new LoadContextArgs(sceneSetToLoad, contextProvider.Create(), onContextLoaded));
        }
    }
}
