using Celeste.Parameters;
using Celeste.Application;
using System;

namespace Celeste.FSM.Nodes.Application
{
    [Serializable]
    [CreateNodeMenu("Celeste/Application/Info")]
    public class ApplicationInfoNode : FSMNode
    {
        #region Properties and Fields

        public BoolValue isEditor;
        public BoolValue isMobile;
        public BoolValue isDebugBuild;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            isEditor.GetIsEditor();
            isMobile.GetIsMobile();
            isDebugBuild.GetIsDebugBuild();
        }

        #endregion
    }
}
