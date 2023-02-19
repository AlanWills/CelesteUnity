using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
	public interface IEventArgs { }

	[Serializable]
	public class GenericUnityEvent : UnityEvent<IEventArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(GenericEvent), menuName = "Celeste/Events/Generic/Generic Event")]
	public class GenericEvent : ParameterisedEvent<IEventArgs> { }
	
	[Serializable]
	public class GuaranteedGenericEvent : GuaranteedParameterisedEvent<GenericEvent, IEventArgs> { }
}
