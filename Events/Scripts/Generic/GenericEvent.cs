using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
	public interface IEventArgs { }

	[Serializable]
	public class GenericUnityEvent : UnityEvent<IEventArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(GenericEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Generic/Generic Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class GenericEvent : ParameterisedEvent<IEventArgs> { }
	
	[Serializable]
	public class GuaranteedGenericEvent : GuaranteedParameterisedEvent<GenericEvent, IEventArgs> { }
}
