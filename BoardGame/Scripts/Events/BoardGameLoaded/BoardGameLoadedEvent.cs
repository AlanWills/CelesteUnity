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
	[CreateAssetMenu(fileName = nameof(BoardGameLoadedEvent), menuName = "Celeste/Events/Board Game/Board Game Loaded")]
	public class BoardGameLoadedEvent : ParameterisedEvent<BoardGameLoadedArgs>
	{
	}
}
