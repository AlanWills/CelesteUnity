using Celeste.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Mouse Handler")]
    public class MouseHandler : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Vector2UnityEvent onMouseEnter = new Vector2UnityEvent();
        [SerializeField] private UnityEvent onMouseExit = new UnityEvent();

        #endregion

        private void OnMouseEnter(Vector2 mousePosition)
        {
            onMouseEnter.Invoke(mousePosition);
        }

        private void OnMouseExit()
        {
            onMouseExit.Invoke();
        }
    }
}
