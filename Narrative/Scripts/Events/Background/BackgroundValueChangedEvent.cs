using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Narrative.Backgrounds;

namespace Celeste.Events 
{
	[Serializable]
	public class BackgroundValueChangedUnityEvent : ValueChangedUnityEvent<Background> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(BackgroundValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Narrative/Background Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class BackgroundValueChangedEvent : ParameterisedValueChangedEvent<Background>
	{
	}
}
