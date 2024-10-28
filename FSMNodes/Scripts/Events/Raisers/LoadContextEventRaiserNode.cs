using System;
using Celeste.Scene;
using Celeste.Scene.Events;
using UnityEngine;

namespace Celeste.FSM.Nodes.Events
{
    [NodeWidth(350)]
    [CreateNodeMenu("Celeste/Events/Raisers/Load Context Event Raiser")]
    public class LoadContextEventRaiserNode : FSMNode
    {
        #region Properties and Fields

        [SerializeField] private SceneSet sceneSetToLoad;
        [SerializeField] private bool showOutputOnLoadingScreen = true;
        [SerializeField] private ContextProvider contextProvider;
        [SerializeField] private OnContextLoadedEvent onContextLoaded;
        [SerializeField] private LoadContextEvent contextEvent;

        #endregion
        
        #region Unity Methods
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (contextEvent == null)
            {
                contextEvent = CelesteEditor.Scene.Settings.SceneEditorSettings.GetOrCreateSettings().defaultLoadContextEvent;
            }

            if (contextProvider == null)
            {
                contextProvider = CelesteEditor.Scene.Settings.SceneEditorSettings.GetOrCreateSettings().defaultContextProvider;
            }
        }
#endif

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            LoadContextArgs loadContextArgs = new LoadContextArgs(
                sceneSetToLoad, 
                showOutputOnLoadingScreen,
                contextProvider.Create(), 
                onContextLoaded);
            contextEvent.Invoke(loadContextArgs);
        }

        #endregion
    }
}
