using Celeste.UI.Events;

namespace Celeste.UI
{
    public interface IOverlayController
    {
        void OnShow(IOverlayArgs args);
        void OnHide();
    }
}
