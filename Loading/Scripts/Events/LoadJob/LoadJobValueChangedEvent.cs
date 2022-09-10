using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Loading;

namespace Celeste.Events 
{
	[Serializable]
	public class LoadJobValueChangedUnityEvent : ValueChangedUnityEvent<LoadJob> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(LoadJobValueChangedEvent), menuName = "Celeste/Events/Loading/Load Job Value Changed Event")]
	public class LoadJobValueChangedEvent : ParameterisedValueChangedEvent<LoadJob>
	{
	}
}
