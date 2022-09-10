using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Narrative.Backgrounds;

namespace Celeste.Events 
{
	[Serializable]
	public class BackgroundValueChangedUnityEvent : ValueChangedUnityEvent<Background> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(BackgroundValueChangedEvent), menuName = "Celeste/Events/Narrative/Background Value Changed Event")]
	public class BackgroundValueChangedEvent : ParameterisedValueChangedEvent<Background>
	{
	}
}
