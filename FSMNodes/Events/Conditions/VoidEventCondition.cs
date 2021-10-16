using Celeste.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.FSM.Nodes.Events.Conditions
{
    [Serializable]
    [DisplayName("Void")]
    public class VoidEventCondition : EventCondition, IEventListener
    {
        #region Properties and Fields

        public Event listenFor;

        public override string name
        {
            get { return listenFor != null ? listenFor.name : ""; }
        }

        #endregion

        #region Listeners

        protected override void AddListenerInternal()
        {
            listenFor.AddListener(this);
        }

        protected override void RemoveListenerInternal()
        {
            listenFor.RemoveListener(this);
        }

        public void OnEventRaised()
        {
            RegisterEventRaised(null);
        }

        #endregion

        #region ICopyable

        public override void CopyFrom(EventCondition original)
        {
            VoidEventCondition voidEventCondition = original as VoidEventCondition;
            listenFor = voidEventCondition.listenFor;
        }

        #endregion
    }
}
