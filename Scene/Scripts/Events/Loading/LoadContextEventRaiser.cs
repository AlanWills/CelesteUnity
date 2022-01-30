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

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameEvent == null)
            {
                gameEvent = CelesteEditor.Scene.Settings.SceneSettings.instance.defaultLoadContextEvent;
            }

            if (contextProvider == null)
            {
                contextProvider = CelesteEditor.Scene.Settings.SceneSettings.instance.defaultContextProvider;
            }
        }
#endif

        public void Raise()
        {
            Raise(new LoadContextArgs(sceneSetToLoad, contextProvider.Create(), onContextLoaded));
        }
    }
}
