using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;
using Celeste.BoardGame.Runtime;

namespace Celeste.Events
{
	[Serializable]
	public struct BoardGameObjectAddedArgs
	{
		public BoardGameRuntime boardGameRuntime;
		public BoardGameObjectRuntime boardGameObjectRuntime;
	}

	[Serializable]
	public class BoardGameObjectAddedUnityEvent : UnityEvent<BoardGameObjectAddedArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(BoardGameObjectAddedEvent), menuName = "Celeste/Events/Board Game/Board Game Object Added")]
	public class BoardGameObjectAddedEvent : ParameterisedEvent<BoardGameObjectAddedArgs>
	{
	}
}
