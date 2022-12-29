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
	[CreateAssetMenu(fileName = nameof(BoardGameReadyEvent), menuName = "Celeste/Events/Board Game/Board Game Ready")]
	public class BoardGameReadyEvent : ParameterisedEvent<BoardGameReadyArgs>
	{
	}
}
