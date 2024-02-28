using System;
using UnityEngine;
using Celeste.Events;
using Celeste.UI.Skin;

namespace Celeste.Events 
{
	[Serializable]
	public class UISkinValueChangedUnityEvent : ValueChangedUnityEvent<UISkin> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(UISkinValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "UI/UI Skin Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class UISkinValueChangedEvent : ParameterisedValueChangedEvent<UISkin>
	{
	}
}
