using Celeste.Tools;
using Celeste.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Wallet.UI
{
    [AddComponentMenu("Celeste/Wallet/UI/Animated Currency Controller")]
    public class AnimatedCurrencyController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Image icon;

        private Vector3 source;
        private Vector3 target;
        private float duration;
        private float currentTime;
        private Action<AnimatedCurrencyController> onComplete;

        #endregion

        public void Hookup(
            Currency currency, 
            RectTransform source, 
            RectTransform target, 
            float duration,
            Action<AnimatedCurrencyController> onComplete)
        {
            this.source = source.GetWorldRect().center;
            this.target = target.GetWorldRect().center;
            this.duration = duration;
            this.onComplete = onComplete;

            icon.sprite = currency.Icon;
            currentTime = 0;
            transform.position = source.position;
        }

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref icon);
        }

        private void OnDisable()
        {
            source = Vector3.zero;
            target = Vector3.zero;
            duration = 0;
            currentTime = 0;
            onComplete = null;
        }

        private void FixedUpdate()
        {
            currentTime += Time.fixedDeltaTime;
            float cosCurrentTime = Mathf.Cos(Mathf.PI * 0.5f * currentTime / duration);
            transform.position = Vector3.Lerp(source, target, 1 - cosCurrentTime * cosCurrentTime);

            if (currentTime >= duration)
            {
                if (onComplete != null)
                {
                    onComplete(this);
                }

                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
