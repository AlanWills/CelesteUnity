using Celeste.BoardGame.Events;
using Celeste.BoardGame.Interfaces;
using Celeste.BoardGame.Runtime;
using Celeste.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame.Objects
{
    [CreateAssetMenu(fileName = nameof(Dice), menuName = "Celeste/Board Game/Objects/Dice")]
    public class Dice : ScriptableObject
    {
        #region Properties and Fields

        [SerializeField] private string dieResetLocation;

        [Header("Events")]
        [SerializeField] private MoveBoardGameObjectEvent moveBoardGameObjectEvent;

        [NonSerialized] private List<BoardGameObjectRuntime> dice = new List<BoardGameObjectRuntime>();

        #endregion

        public void AddDie(BoardGameObjectRuntime die)
        {
            UnityEngine.Debug.Assert(die.HasComponent<IBoardGameObjectDie>(), $"No {nameof(IBoardGameObjectDie)} found on {die.Name}.");
            dice.Add(die);
        }

        public void RemoveDie(BoardGameObjectRuntime die)
        {
            dice.Remove(die);
        }

        public void RollAll()
        {
            for (int i = 0, n = dice.Count; i < n; ++i)
            {
                dice[i].TryFindComponent<IBoardGameObjectDie>(out var die);
                die.iFace.Roll(die.instance);
            }
        }

        public void ResetAllPositions()
        {
            for (int i = 0, n = dice.Count; i < n; ++i)
            {
                Move(dice[i], dieResetLocation);
            }
        }

        public void MoveAllMatchingDiceTo(int value, string location)
        {
            for (int i = 0, n = dice.Count; i < n; ++i)
            {
                dice[i].TryFindComponent<IBoardGameObjectDie>(out var die);
                
                if (die.iFace.GetValue(die.instance) == value)
                {
                    Move(dice[i], location);
                }
            }
        }

        private void Move(BoardGameObjectRuntime die, string location)
        {
            moveBoardGameObjectEvent.Invoke(new MoveBoardGameObjectArgs()
            {
                boardGameObjectRuntime = die,
                newLocation = location
            });
        }
    }
}
