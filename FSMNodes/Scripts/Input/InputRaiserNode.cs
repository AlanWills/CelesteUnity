using Celeste.Events;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FSM.Nodes.Input
{
    [Serializable]
    [CreateNodeMenu("Celeste/Input/Input Raiser")]
    [NodeWidth(250)]
    public class InputRaiserNode : FSMNode
    {
        #region Properties and Fields

        public GameObjectClickEvent inputEvent;
        public Vector3 inputPosition;
        public string gameObjectName;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            GameObject gameObject = GameObject.Find(gameObjectName);
            if (gameObject != null)
            {
                inputEvent.Invoke(new GameObjectClickEventArgs() { clickWorldPosition = inputPosition, gameObject = gameObject });
            }
            else
            {
                Debug.LogAssertionFormat("GameObject {0} could not be found in InputRaiserNode", gameObjectName);
            }
        }

        #endregion
    }
}
