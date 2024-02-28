using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.BoardGame.Runtime;

namespace Celeste.Events
{
    [Serializable]
    public struct BoardGameLoadedArgs
    {
        public BoardGameRuntime boardGameRuntime;
    }

    [Serializable]
	public class BoardGameLoadedUnityEvent : UnityEvent<BoardGameLoadedArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(BoardGameLoadedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Board Game/Board Game Loaded", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class BoardGameLoadedEvent : ParameterisedEvent<BoardGameLoadedArgs>
	{
	}
}
