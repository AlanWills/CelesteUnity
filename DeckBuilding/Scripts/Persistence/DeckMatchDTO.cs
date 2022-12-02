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
        public DeckMatchPlayerRuntimeDTO playerDeckRuntime;
        public DeckMatchPlayerRuntimeDTO enemyDeckRuntime;
        public string currentFSMRuntimeNode;

        public DeckMatchDTO(DeckMatchRuntime deckMatchRuntime, FSMRuntime deckMatchFSMRuntime)
        {
            isPlayerTurn = deckMatchRuntime.ActiveDeckRuntime == deckMatchRuntime.PlayerDeckRuntime;
            playerDeckRuntime = new DeckMatchPlayerRuntimeDTO(deckMatchRuntime.PlayerDeckRuntime);
            enemyDeckRuntime = new DeckMatchPlayerRuntimeDTO(deckMatchRuntime.EnemyDeckRuntime);
            currentFSMRuntimeNode = deckMatchFSMRuntime.CurrentNode != null ? deckMatchFSMRuntime.CurrentNode.Guid : "";
        }
    }
}