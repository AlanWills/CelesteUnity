using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/UI/Show Popup Event Raiser")]
    public class ShowPopupEventRaiser : ParameterisedEventRaiser<ShowPopupArgs, ShowPopupEvent>
    {
        private static readonly ShowPopupArgs DEFAULT_ARGS = new ShowPopupArgs();

        public void RaiseDefaultArgs()
        {
            Raise(DEFAULT_ARGS);
        }
    }
}
