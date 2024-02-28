using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Localisation 
{
	[Serializable]
	public class LanguageValueChangedUnityEvent : ValueChangedUnityEvent<Language> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(LanguageValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Localisation/Language Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class LanguageValueChangedEvent : ParameterisedValueChangedEvent<Language>
	{
	}
}
