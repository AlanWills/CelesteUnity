using Celeste.Events;
using Celeste.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Celeste.UI.Dialog;
using Event = Celeste.Events.Event;

namespace Celeste.FSM.Nodes.UI
{
    [Serializable]
    [CreateNodeMenu("Celeste/UI/Show Dialog")]
    [NodeWidth(250), NodeTint(0.8f, 0.9f, 0)]
    public class ShowDialogNode : FSMNode
    {
        private class DummyEventListener : IEventListener
        {
            public bool eventRaised;
            public string portName;

            public DummyEventListener(string portName)
            {
                this.portName = portName;
            }

            public void OnEventRaised()
            {
                eventRaised = true;
            }
        }

        #region Properties and Fields

        public Dialog dialog;
        public ShowDialogParams parameters = new ShowDialogParams();

        private List<DummyEventListener> dummyEventListeners = new List<DummyEventListener>();
        private Dialog dialogInstance;

        #endregion

        public ShowDialogNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);

            parameters.showConfirmButton = true;
            parameters.showCloseButton = true;
        }

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            if (dummyEventListeners.Count == parameters.customDialogEvents.Count)
            {
                // We already have created the list so reset the listeners
                foreach (DummyEventListener listener in dummyEventListeners)
                {
                    listener.eventRaised = false;
                }
            }
            else
            {
                // Make the list from scratch - this should happen only once
                foreach (Event customEvent in parameters.customDialogEvents)
                {
                    dummyEventListeners.Add(new DummyEventListener(customEvent.name));
                }
            }

            dialogInstance = GameObject.Instantiate(dialog.gameObject).GetComponent<Dialog>();
            dialogInstance.gameObject.name = dialog.gameObject.name;

            for (int i = 0; i < parameters.customDialogEvents.Count; ++i)
            {
                parameters.customDialogEvents[i].AddEventListener(dummyEventListeners[i]);
            }

            dialogInstance.Show(parameters);
        }

        protected override FSMNode OnUpdate()
        {
            foreach (DummyEventListener dummyEventListener in dummyEventListeners)
            {
                if (dummyEventListener.eventRaised)
                {
                    return GetConnectedNode(dummyEventListener.portName);
                }
            }

            return this;
        }

        protected override void OnExit()
        {
            base.OnExit();

            for (int i = 0; i < parameters.customDialogEvents.Count; ++i)
            {
                parameters.customDialogEvents[i].RemoveEventListener(dummyEventListeners[i]);
            }

            dialogInstance = null;
        }

        #endregion
    }
}
