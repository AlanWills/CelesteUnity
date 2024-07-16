using Celeste.Events;

namespace Celeste.UI
{
    public interface IPopupElement
    {
        void OnShow(IPopupArgs args);
        void OnHide();

        void OnConfirmPressed();
        void OnClosePressed();
    }
}