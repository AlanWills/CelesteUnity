using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Decks;
using Celeste.Persistence;
using UnityEngine;

namespace Celeste.DeckBuilding.Persistence
{
    [AddComponentMenu("Celeste/Deck Building/Deck Building")]
    public class DeckBuildingManager : PersistentSceneManager<DeckBuildingManager, DeckBuildingDTO>
    {
        #region Properties and Fields

        public static readonly string FILE_NAME = "DeckBuilding.dat";
        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        [SerializeField] private CardCatalogue cardCatalogue;
        [SerializeField] private PrebuiltDeck[] startingDecks;
        [SerializeField] private DeckBuildingRecord deckBuildingRecord;

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(DeckBuildingDTO dto)
        {
            foreach (DeckDTO deckDTO in dto.decks)
            {
                Deck deck = deckBuildingRecord.CreateDeck();
                
                foreach (int cardGuid in deckDTO.cards)
                {
                    Card card = cardCatalogue.FindByGuid(cardGuid);
                    UnityEngine.Debug.Assert(card != null, $"Could not find card with guid {cardGuid}.");
                    deck.AddCardToDeck(card);
                }
            }
        }

        protected override DeckBuildingDTO Serialize()
        {
            return new DeckBuildingDTO(deckBuildingRecord);
        }

        protected override void SetDefaultValues()
        {
            for (int i = 0, n = startingDecks != null ? startingDecks.Length : 0; i < n; ++i)
            {
                deckBuildingRecord.AddDeck(startingDecks[i].ToDeck());
            }
        }

        #endregion
    }
}