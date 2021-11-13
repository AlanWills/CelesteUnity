using Celeste.Events;

namespace Celeste.UI
{
    public interface IPopupController
    {
        void OnShow(ShowPopupArgs args);
        void OnHide();

        void OnConfirmPressed();
        void OnClosePressed();
    }
}
