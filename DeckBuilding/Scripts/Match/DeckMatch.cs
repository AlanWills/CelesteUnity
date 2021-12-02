using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Loading;
using Celeste.DeckBuilding.Persistence;
using Celeste.DeckBuilding.Results;
using Celeste.Objects;
using UnityEngine;

namespace Celeste.DeckBuilding.Match
{
    [CreateAssetMenu(fileName = nameof(DeckMatch), menuName = "Celeste/Deck Building/Match/Deck Match")]
    public class DeckMatch : ScriptableObject
    {
        #region Properties and Fields

        [Header("Data")]
        [SerializeField] private DeckBuildingRecord deckBuildingRecord;

        [Header("Player")]
        [SerializeField] private LoseCondition playerLoseCondition;

        [Header("Enemy")]
        [SerializeField] private LoseCondition enemyLoseCondition;
        [SerializeField] private PrebuiltDeck enemyDeck;

        #endregion

        public DeckMatchContext CreateContext()
        {
            Deck playerDeck = deckBuildingRecord.GetDeck(0);
            playerDeck.LoseCondition = ScriptableObject.Instantiate(playerLoseCondition);

            Deck enemyDeck = this.enemyDeck.ToDeck();
            enemyDeck.LoseCondition = ScriptableObject.Instantiate(enemyLoseCondition);

            return new DeckMatchContext(this);
        }

        public Deck CreatePlayerDeck()
        {
            Deck playerDeck = deckBuildingRecord.GetDeck(0);
            playerDeck.LoseCondition = ScriptableObject.Instantiate(playerLoseCondition);

            return playerDeck;
        }

        public Deck CreateEnemyDeck()
        {
            Deck enemyDeck = this.enemyDeck.ToDeck();
            enemyDeck.LoseCondition = ScriptableObject.Instantiate(enemyLoseCondition);

            return enemyDeck;
        }
    }
}