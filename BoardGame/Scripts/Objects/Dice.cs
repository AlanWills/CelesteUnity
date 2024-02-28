using Celeste.BoardGame.Events;
using Celeste.BoardGame.Interfaces;
using Celeste.BoardGame.Runtime;
using Celeste.Components;
using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame.Objects
{
    [CreateAssetMenu(
        fileName = nameof(Dice), 
        menuName = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Objects/Dice",
        order = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
    public class Dice : ScriptableObject
    {
        #region Properties and Fields

        public int NumDice => dice.Count;

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

        public BoardGameObjectRuntime GetDie(int index)
        {
            return dice.Get(index);
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
                MoveDiceTo(dice[i], dieResetLocation);
            }
        }

        public void MoveDiceTo(BoardGameObjectRuntime die, string location)
        {
            UnityEngine.Debug.Assert(dice.Contains(die), $"Could not find inputted die {die.Name} in {name}.");
            moveBoardGameObjectEvent.Invoke(new MoveBoardGameObjectArgs()
            {
                boardGameObjectRuntime = die,
                newLocation = location
            });
        }
    }
}
