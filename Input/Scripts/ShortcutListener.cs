using Celeste.Events;
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
            UnityEngine.Debug.Assert(shortcut != null, $"{name} has a null shortcut on listener {GetType().Name},");
            shortcut.fired.AddListener(this);
        }

        private void OnDisable()
        {
            UnityEngine.Debug.Assert(shortcut != null, $"{name} has a null shortcut on listener {GetType().Name},");
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
