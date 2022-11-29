using Celeste.BT;
using System.Collections;
using UnityEngine;
using static Celeste.DeckBuilding.AI.DeckBuildingAIBlackboardKeys;

namespace Celeste.DeckBuilding.AI
{
    [AddComponentMenu("Celeste/Deck Building/AI/AI Autoplay")]
    public class AIAutoplay : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private BTRuntime aiRuntime;
        [SerializeField] private DeckMatchPlayerRuntime controlledDeckRuntime;
        [SerializeField] private DeckMatchPlayerRuntime opponentDeckRuntime;

        [Header("Events")]
        [SerializeField] private Celeste.Events.Event endTurnEvent;

        private BTBlackboard blackboard = new BTBlackboard();
        private WaitForSeconds delay = new WaitForSeconds(1);

        #endregion

        private void WriteVariablesToBlackboard()
        {
            blackboard.SetInt(CURRENT_RESOURCES, controlledDeckRuntime.AvailableResources.CurrentResources);
            blackboard.SetObject(CONTROLLED_DECK, controlledDeckRuntime);
            blackboard.SetObject(OPPONENT_DECK, opponentDeckRuntime);
        }

        private IEnumerator Think()
        {
            do
            {
                yield return delay;
            }
            while (aiRuntime.Evaluate(blackboard));

            endTurnEvent.Invoke();
        }

        #region Callbacks

        public void OnDeckBecomesActive()
        {
            WriteVariablesToBlackboard();
            StartCoroutine(Think());
        }

        #endregion
    }
}