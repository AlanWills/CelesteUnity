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
	[CreateAssetMenu(fileName = nameof(BoardGameObjectAddedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Board Game/Board Game Object Added", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class BoardGameObjectAddedEvent : ParameterisedEvent<BoardGameObjectAddedArgs>
	{
	}
}
