using Celeste.Debug.Commands;
using Celeste.Events;
using Celeste.Log;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Celeste.DeckBuilding.Debug
{
    [CreateAssetMenu(fileName = nameof(ConsoleDeckMatchResources), menuName = "Celeste/Deck Building/Debug/Console Deck Match Resources")]
    public class ConsoleDeckMatchResources : DebugCommand
    {
        #region Properties and Fields

        [SerializeField] private IntEvent activeDeckAddsResources;
        [SerializeField] private IntEvent playerDeckAddsResources;
        [SerializeField] private IntEvent enemyDeckAddsResources;

        private const string ACTIVE_DECK_CHAR = "a";
        private const string PLAYER_DECK_CHAR = "p";
        private const string ENEMY_DECK_CHAR = "e";
        private const string TARGET_OPTIONS_STRING = "[" + ACTIVE_DECK_CHAR + "|" + PLAYER_DECK_CHAR + "|" + ENEMY_DECK_CHAR + "]";

        #endregion

        public override bool Execute(List<string> parameters, StringBuilder output)
        {
            if (parameters.Count < 2)
            {
                output.Append($"Insufficient parameters.  Expected {TARGET_OPTIONS_STRING} [quantity].");
                return false;
            }
            else
            {
                int quantity = 0;
                if (!int.TryParse(parameters[1], out quantity))
                {
                    output.Append($"Invalid parameter: '{parameters[1]}'.  Expected int value for quantity.");
                    return false;
                }

                if (parameters[0] == ACTIVE_DECK_CHAR)
                {
                    activeDeckAddsResources.Invoke(quantity);
                    HudLog.LogInfo($"Adding {quantity} resources to active deck.");
                    return true;
                }
                else if (parameters[0] == PLAYER_DECK_CHAR)
                {
                    playerDeckAddsResources.Invoke(quantity);
                    HudLog.LogInfo($"Adding {quantity} resources to player deck.");
                    return true;
                }
                else if (parameters[0] == ENEMY_DECK_CHAR)
                {
                    enemyDeckAddsResources.Invoke(quantity);
                    HudLog.LogInfo($"Adding {quantity} resources to enemy deck.");
                    return true;
                }
                else
                {
                    output.Append($"Invalid parameter: '{parameters[0]}'.  Expected {TARGET_OPTIONS_STRING} for target.");
                    return false;
                }
            }
        }
    }
}
