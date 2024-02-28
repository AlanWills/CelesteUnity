using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class GameObjectValueChangedUnityEvent : ValueChangedUnityEvent<GameObject> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(GameObjectValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Game Object/Game Object Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class GameObjectValueChangedEvent : ParameterisedValueChangedEvent<GameObject>
	{
	}
}
