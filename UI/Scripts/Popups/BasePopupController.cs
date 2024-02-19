using Celeste.Events;
using System;
using UnityEngine;

namespace Celeste.UI
{
    public class BasePopupController : MonoBehaviour, IPopupController
    {
        Action IPopupController.RequestToClosePopup { get; set; }

        public virtual void OnShow(IPopupArgs args) { }
        public virtual void OnHide() { }

        public virtual void OnConfirmPressed() { }
        public virtual void OnClosePressed() { }

        protected void RequestToClosePopup()
        {
            (this as IPopupController).RequestToClosePopup?.Invoke();
        }
    }
}