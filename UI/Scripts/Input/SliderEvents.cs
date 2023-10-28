using Celeste.Events;
using Celeste.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Celeste.UI
{
    [AddComponentMenu("Celeste/UI/Slider Events")]
    [RequireComponent(typeof(Slider))]
    public class SliderEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Properties and Fields

        [SerializeField] private Slider slider;
        [SerializeField] private GuaranteedEvent onBeginEditValue;
        [SerializeField] private GuaranteedFloatEvent onEndEditValue;

        #endregion

        private void OnValidate()
        {
            this.TryGet(ref slider);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onBeginEditValue.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onEndEditValue.Invoke(slider.value);
        }

        public void AddOnBeginEditValueCallback(UnityAction callback)
        {
            onBeginEditValue.AddListener(callback);
        }

        public void RemoveOnBeginEditValueCallback(UnityAction callback)
        {
            onBeginEditValue.AddListener(callback);
        }

        public void AddOnEndEditValueCallback(UnityAction<float> callback)
        {
            onEndEditValue.AddListener(callback);
        }

        public void RemoveOnEndEditValueCallback(UnityAction<float> callback)
        {
            onEndEditValue.AddListener(callback);
        }
    }
}
