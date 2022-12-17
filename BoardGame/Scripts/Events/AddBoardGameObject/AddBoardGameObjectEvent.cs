using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
	[Serializable]
	public struct AddBoardGameObjectArgs
	{
		[Tooltip("The guid of the board game object to add.")] public int boardGameObjectGuid;
		[Tooltip("The location key the board game object will start at.")] public string location;
	}

	[Serializable]
	public class AddBoardGameObjectUnityEvent : UnityEvent<AddBoardGameObjectArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(AddBoardGameObjectEvent), menuName = "Celeste/Events/Board Game/Add Board Game Object Event")]
	public class AddBoardGameObjectEvent : ParameterisedEvent<AddBoardGameObjectArgs>
	{
	}
}
