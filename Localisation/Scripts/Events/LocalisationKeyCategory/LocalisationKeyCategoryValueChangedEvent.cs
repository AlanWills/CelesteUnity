using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Localisation;

namespace Celeste.Events 
{
	[Serializable]
	public class LocalisationKeyCategoryValueChangedUnityEvent : ValueChangedUnityEvent<LocalisationKeyCategory> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(LocalisationKeyCategoryValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Localisation/Localisation Key Category Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class LocalisationKeyCategoryValueChangedEvent : ParameterisedValueChangedEvent<LocalisationKeyCategory>
	{
	}
}
