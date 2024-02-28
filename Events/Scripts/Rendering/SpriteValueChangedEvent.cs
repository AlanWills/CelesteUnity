using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class SpriteValueChangedUnityEvent : ValueChangedUnityEvent<Sprite> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(SpriteValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Rendering/Sprite Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class SpriteValueChangedEvent : ParameterisedValueChangedEvent<Sprite>
	{
	}
}
