using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;
using Celeste.BoardGame.Runtime;

namespace Celeste.Events
{
	[Serializable]
	public struct BoardGameObjectMovedArgs
	{
		public BoardGameRuntime boardGameRuntime;
		public BoardGameObjectRuntime boardGameObjectRuntime;
		public string oldLocation;
		public string newLocation;
	}

	[Serializable]
	public class BoardGameObjectMovedUnityEvent : UnityEvent<BoardGameObjectMovedArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(BoardGameObjectMovedEvent), menuName = "Celeste/Events/Board Game/Board Game Object Moved")]
	public class BoardGameObjectMovedEvent : ParameterisedEvent<BoardGameObjectMovedArgs> { }
	
	[Serializable]
	public class GuaranteedBoardGameObjectMovedEvent : GuaranteedParameterisedEvent<BoardGameObjectMovedEvent, BoardGameObjectMovedArgs> { }
}
