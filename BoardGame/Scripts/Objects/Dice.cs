using Celeste.BoardGame.Interfaces;
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

        [NonSerialized] private List<InterfaceHandle<IBoardGameObjectDie>> dice = new List<InterfaceHandle<IBoardGameObjectDie>>();

        #endregion

        public void AddDie(InterfaceHandle<IBoardGameObjectDie> die)
        {
            dice.Add(die);
        }

        public void RemoveDie(InterfaceHandle<IBoardGameObjectDie> die)
        {
            dice.Remove(die);
        }

        public void RollAll()
        {
            for (int i = 0, n = dice.Count; i < n; ++i)
            {
                dice[i].iFace.Roll(dice[i].instance);
            }
        }
    }
}
