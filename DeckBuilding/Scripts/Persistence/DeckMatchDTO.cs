using Celeste.DeckBuilding.Match;
using Celeste.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class DeckMatchDTO
    {
        public bool isPlayerTurn;
        public DeckRuntimeDTO playerDeckRuntime;
        public DeckRuntimeDTO enemyDeckRuntime;
        public string currentFSMRuntimeNode;

        public DeckMatchDTO(DeckMatchRuntime deckMatchRuntime, FSMRuntime deckMatchFSMRuntime)
        {
            isPlayerTurn = deckMatchRuntime.ActiveDeckRuntime == deckMatchRuntime.PlayerDeckRuntime;
            playerDeckRuntime = new DeckRuntimeDTO(deckMatchRuntime.PlayerDeckRuntime);
            enemyDeckRuntime = new DeckRuntimeDTO(deckMatchRuntime.EnemyDeckRuntime);
            currentFSMRuntimeNode = deckMatchFSMRuntime.CurrentNode != null ? deckMatchFSMRuntime.CurrentNode.Guid : "";
        }
    }
}