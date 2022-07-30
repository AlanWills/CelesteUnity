using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    public interface IPopupArgs { }

    [Serializable]
    public struct NoPopupArgs : IPopupArgs { }

    [Serializable]
    public class ShowPopupUnityEvent : UnityEvent<IPopupArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(ShowPopupEvent), menuName = "Celeste/Events/UI/Show Popup Event")]
    public class ShowPopupEvent : ParameterisedEvent<IPopupArgs> 
    {
        public void InvokeNoArgs()
        {
            Invoke(new NoPopupArgs());
        }
    }
}
