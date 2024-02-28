using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;

namespace Celeste.Events
{
    [Serializable]
    public struct BoardGameShutdownArgs
    {
    }

    [Serializable]
	public class BoardGameShutdownUnityEvent : UnityEvent<BoardGameShutdownArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(BoardGameShutdownEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Board Game/Board Game Shutdown", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class BoardGameShutdownEvent : ParameterisedEvent<BoardGameShutdownArgs>
	{
	}
}
