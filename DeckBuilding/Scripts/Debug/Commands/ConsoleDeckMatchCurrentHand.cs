using Celeste.Debug.Commands;
using Celeste.Events;
using Celeste.Log;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Celeste.DeckBuilding.Debug
{
    [CreateAssetMenu(fileName = nameof(ConsoleDeckMatchCurrentHand), menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Debug/Console Deck Match Current Hand", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class ConsoleDeckMatchCurrentHand : DebugCommand
    {
        #region Properties and Fields

        [SerializeField] private IntEvent activeDeckDrawsCards;
        [SerializeField] private IntEvent playerDeckDrawsCards;
        [SerializeField] private IntEvent enemyDeckDrawsCards;

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
                    activeDeckDrawsCards.Invoke(quantity);
                    HudLog.LogInfo($"Drawing {quantity} cards for active deck.");
                    return true;
                }
                else if (parameters[0] == PLAYER_DECK_CHAR)
                {
                    playerDeckDrawsCards.Invoke(quantity);
                    HudLog.LogInfo($"Drawing {quantity} cards for player deck.");
                    return true;
                }
                else if (parameters[0] == ENEMY_DECK_CHAR)
                {
                    enemyDeckDrawsCards.Invoke(quantity);
                    HudLog.LogInfo($"Drawing {quantity} cards for enemy deck.");
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
