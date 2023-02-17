using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;
using Celeste.BoardGame.Runtime;

namespace Celeste.BoardGame.Events
{
	[Serializable]
	public struct MoveBoardGameObjectArgs
	{
		public BoardGameObjectRuntime boardGameObjectRuntime;
		public int boardGameObjectRuntimeInstanceId;
		public string boardGameObjectRuntimeName;
		public string newLocation;
	}

	[Serializable]
	public class MoveBoardGameObjectUnityEvent : UnityEvent<MoveBoardGameObjectArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(MoveBoardGameObjectEvent), menuName = "Celeste/Events/Board Game/Move Board Game Object Event")]
	public class MoveBoardGameObjectEvent : ParameterisedEvent<MoveBoardGameObjectArgs> { }
	
	[Serializable]
	public class GuaranteedMoveBoardGameObjectEvent : GuaranteedParameterisedEvent<MoveBoardGameObjectEvent, MoveBoardGameObjectArgs> { }
}
