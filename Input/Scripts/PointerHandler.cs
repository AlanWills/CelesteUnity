using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Pointer Handler")]
    public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Properties and Fields

        [SerializeField] private UnityEvent onPointerEnter = new UnityEvent();
        [SerializeField] private UnityEvent onPointerExit = new UnityEvent();

        #endregion

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit.Invoke();
        }
    }
}
