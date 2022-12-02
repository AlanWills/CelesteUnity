using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Loading;
using Celeste.DeckBuilding.Persistence;
using Celeste.DeckBuilding.Results;
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
        [SerializeField] private Deck enemyDeck;

        #endregion

        public DeckMatchContext CreateContext()
        {
            return new DeckMatchContext(this);
        }

        public Deck CreatePlayerDeck()
        {
            return deckBuildingRecord.GetDeck(0);
        }

        public Deck CreateEnemyDeck()
        {
            return enemyDeck;
        }
    }
}