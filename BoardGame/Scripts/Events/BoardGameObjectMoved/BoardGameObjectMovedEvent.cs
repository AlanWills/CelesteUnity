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
	[CreateAssetMenu(fileName = nameof(BoardGameObjectMovedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Board Game/Board Game Object Moved", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class BoardGameObjectMovedEvent : ParameterisedEvent<BoardGameObjectMovedArgs> { }
	
	[Serializable]
	public class GuaranteedBoardGameObjectMovedEvent : GuaranteedParameterisedEvent<BoardGameObjectMovedEvent, BoardGameObjectMovedArgs> { }
}
