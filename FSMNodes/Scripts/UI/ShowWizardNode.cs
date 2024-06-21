using Celeste.Events;
using Celeste.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Celeste.UI.Wizard;

namespace Celeste.FSM.Nodes.UI
{
    [Serializable]
    [CreateNodeMenu("Celeste/UI/Show Wizard")]
    [NodeWidth(250), NodeTint(0.8f, 0.9f, 0)]
    public class ShowWizardNode : FSMNode
    {
        private class DummyEventListener : IEventListener
        {
            public bool eventRaised;

            public void OnEventRaised()
            {
                eventRaised = true;
            }
        }

        #region Properties and Fields

        public const string CONFIRM_PRESSED_PORT_NAME = "Confirm Pressed";
        public const string CLOSE_PRESSED_PORT_NAME = "Close Pressed";

        public Wizard dialog;
        public ShowPagedDialogParams parameters = new ShowPagedDialogParams();

        private DummyEventListener confirmDummyEventListener = new DummyEventListener();
        private DummyEventListener closeDummyEventListener = new DummyEventListener();
        private Wizard dialogInstance;

        #endregion

        public ShowWizardNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);

            AddOutputPort(CONFIRM_PRESSED_PORT_NAME);
            AddOutputPort(CLOSE_PRESSED_PORT_NAME);

            parameters.showCloseButton = true;
        }

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            confirmDummyEventListener.eventRaised = false;
            closeDummyEventListener.eventRaised = false;

            dialogInstance = GameObject.Instantiate(dialog.gameObject).GetComponent<Wizard>();
            dialogInstance.ConfirmButtonClicked.AddListener(confirmDummyEventListener);
            dialogInstance.CloseButtonClicked.AddListener(closeDummyEventListener);
            dialogInstance.Show(parameters);
        }

        protected override FSMNode OnUpdate()
        {
            if (confirmDummyEventListener.eventRaised)
            {
                return GetConnectedNodeFromOutput(CONFIRM_PRESSED_PORT_NAME);
            }
            else if (closeDummyEventListener.eventRaised)
            {
                return GetConnectedNodeFromOutput(CLOSE_PRESSED_PORT_NAME);
            }

            return this;
        }

        protected override void OnExit()
        {
            base.OnExit();

            dialogInstance.ConfirmButtonClicked.RemoveListener(confirmDummyEventListener);
            dialogInstance.CloseButtonClicked.RemoveListener(closeDummyEventListener);
            dialogInstance = null;
        }

        #endregion
    }
}
