using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Loading;

namespace Celeste.Events 
{
	[Serializable]
	public class LoadJobValueChangedUnityEvent : ValueChangedUnityEvent<LoadJob> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(LoadJobValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Loading/Load Job Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class LoadJobValueChangedEvent : ParameterisedValueChangedEvent<LoadJob>
	{
	}
}
