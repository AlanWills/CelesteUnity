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
	[CreateAssetMenu(fileName = nameof(BoardGameShutdownEvent), menuName = "Celeste/Events/Board Game/Board Game Shutdown")]
	public class BoardGameShutdownEvent : ParameterisedEvent<BoardGameShutdownArgs>
	{
	}
}
