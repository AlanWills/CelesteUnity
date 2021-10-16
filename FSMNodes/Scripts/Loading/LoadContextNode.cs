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
        public LoadContextEvent loadContextEvent;
        public OnContextLoadedEvent onContextLoadedEvent;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            Context context = CreateContext();
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

        protected virtual Context CreateContext()
        {
            return new Context();
        }
    }
}
