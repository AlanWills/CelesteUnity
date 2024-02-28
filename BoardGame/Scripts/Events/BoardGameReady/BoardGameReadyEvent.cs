using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;
using Celeste.BoardGame.Runtime;

namespace Celeste.Events
{
    [Serializable]
    public struct BoardGameReadyArgs
    {
        public BoardGameRuntime boardGameRuntime;
    }

    [Serializable]
	public class BoardGameReadyUnityEvent : UnityEvent<BoardGameReadyArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(BoardGameReadyEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Board Game/Board Game Ready", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class BoardGameReadyEvent : ParameterisedEvent<BoardGameReadyArgs>
	{
	}
}
