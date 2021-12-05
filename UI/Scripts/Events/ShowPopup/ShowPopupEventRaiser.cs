using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/UI/Show Popup Event Raiser")]
    public class ShowPopupEventRaiser : ParameterisedEventRaiser<IPopupArgs, ShowPopupEvent>
    {
        private static readonly NoPopupArgs NO_ARGS = new NoPopupArgs();

        public void RaiseNoArgs()
        {
            Raise(NO_ARGS);
        }
    }
}
