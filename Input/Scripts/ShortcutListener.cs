using Celeste.Events;
using Celeste.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Shortcut Listener")]
    public class ShortcutListener : MonoBehaviour, IEventListener
    {
        #region Properties and Fields

        public Shortcut shortcut;
        public UnityEvent response;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Debug.Assert(shortcut != null, string.Format("{0} has a null shortcut on listener {1}", name, GetType().Name));
            shortcut.fired.AddListener(this);
        }

        private void OnDisable()
        {
            Debug.Assert(shortcut != null, string.Format("{0} has a null shortcut on listener {1}", name, GetType().Name));
            shortcut.fired.RemoveListener(this);
        }

        #endregion

        #region Response Methods

        public void OnEventRaised()
        {
            response.Invoke();
        }

        #endregion
    }
}
