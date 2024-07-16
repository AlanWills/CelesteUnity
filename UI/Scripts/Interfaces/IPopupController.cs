using Celeste.Events;
using System;

namespace Celeste.UI
{
    public interface IPopupController
    {
        Action RequestToClosePopup { get; set; }

        void Show(IPopupArgs args);
        void Hide();

        void ConfirmPressed();
        void ClosePressed();
    }
}
