using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;

namespace Celeste.UI.Events
{
	public interface IOverlayArgs { }

    [Serializable]
    public struct NoOverlayArgs : IOverlayArgs { }

    [Serializable]
	public class ShowOverlayEventUnity : UnityEvent<IOverlayArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(ShowOverlayEvent), menuName = "Celeste/Events/UI/Show Overlay Event")]
	public class ShowOverlayEvent : ParameterisedEvent<IOverlayArgs> { }
	
	[Serializable]
	public class GuaranteedShowOverlayEvent : GuaranteedParameterisedEvent<ShowOverlayEvent, IOverlayArgs> { }
}
