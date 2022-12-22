using Celeste.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Mouse Handler")]
    public class MouseHandler : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private UnityEvent<InputState> onMouseEnter = new UnityEvent<InputState>();
        [SerializeField] private UnityEvent<InputState> onMouseOver = new UnityEvent<InputState>();
        [SerializeField] private UnityEvent onMouseExit = new UnityEvent();

        #endregion

        private void OnMouseEnterCollider(InputState inputState)
        {
            onMouseEnter.Invoke(inputState);
        }

        private void OnMouseOverCollider(InputState inputState)
        {
            onMouseOver.Invoke(inputState);
        }

        private void OnMouseExitCollider()
        {
            onMouseExit.Invoke();
        }
    }
}
