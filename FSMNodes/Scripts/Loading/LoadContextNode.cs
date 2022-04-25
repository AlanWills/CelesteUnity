using Celeste.Scene;
using Celeste.Scene.Events;
using System;

namespace Celeste.FSM.Nodes.Loading
{
    [CreateNodeMenu("Celeste/Loading/Load Context")]
    public class LoadContextNode : FSMNode
    {
        #region Properties and Fields

        public SceneSet sceneSet;
        public ContextProvider contextProvider;
        public LoadContextEvent loadContextEvent;
        public OnContextLoadedEvent onContextLoadedEvent;

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (loadContextEvent == null)
            {
                loadContextEvent = CelesteEditor.Scene.Settings.SceneSettings.instance.defaultLoadContextEvent;
            }

            if (contextProvider == null)
            {
                contextProvider = CelesteEditor.Scene.Settings.SceneSettings.instance.defaultContextProvider;
            }
        }
#endif

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            Context context = contextProvider.Create();
            if (context != null)
            {
                LoadContextArgs loadContextArgs = new LoadContextArgs(sceneSet, context, onContextLoadedEvent);
                loadContextEvent.Invoke(loadContextArgs);
            }
        }

        protected override FSMNode OnUpdate()
        {
            // Spin wait until context change
            return this;
        }

        #endregion
    }
}
