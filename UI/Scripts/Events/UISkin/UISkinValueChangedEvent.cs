using System;
using UnityEngine;
using Celeste.Events;
using Celeste.UI.Skin;

namespace Celeste.Events 
{
	[Serializable]
	public class UISkinValueChangedUnityEvent : ValueChangedUnityEvent<UISkin> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(UISkinValueChangedEvent), menuName = "Celeste/Events/UI/UI Skin Value Changed Event")]
	public class UISkinValueChangedEvent : ParameterisedValueChangedEvent<UISkin>
	{
	}
}
