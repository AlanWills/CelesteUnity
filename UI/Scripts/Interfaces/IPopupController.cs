using Celeste.Events;

namespace Celeste.UI
{
    public interface IPopupController
    {
        void OnShow(IPopupArgs args);
        void OnHide();

        void OnConfirmPressed();
        void OnClosePressed();
    }
}
