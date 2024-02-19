using Celeste.Events;
using System;

namespace Celeste.UI
{
    public interface IPopupController
    {
        Action RequestToClosePopup { get; set; }

        void OnShow(IPopupArgs args);
        void OnHide();

        void OnConfirmPressed();
        void OnClosePressed();
    }
}
