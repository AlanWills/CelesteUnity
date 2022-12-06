using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Catalogue;
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
        [SerializeField] private DeckBuildingRecord deckBuildingRecord;

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(DeckBuildingDTO dto)
        {
            foreach (DeckDTO deckDTO in dto.decks)
            {
                Deck deck = deckBuildingRecord.CreateDeck();
                
                foreach (CardRuntimeDTO cardRuntimeDTO in deckDTO.cardsInDrawPile)
                {
                    Card card = cardCatalogue.FindByGuid(cardRuntimeDTO.cardGuid);
                    UnityEngine.Debug.Assert(card != null, $"Could not find card with guid {cardRuntimeDTO}.");
                    deck.AddCardToDrawPile(card);
                }

                foreach (CardRuntimeDTO cardRuntimeDTO in deckDTO.cardsInDiscardPile)
                {
                    Card card = cardCatalogue.FindByGuid(cardRuntimeDTO.cardGuid);
                    UnityEngine.Debug.Assert(card != null, $"Could not find card with guid {cardRuntimeDTO}.");
                    deck.AddCardToDiscardPile(card);
                }

                foreach (CardRuntimeDTO cardRuntimeDTO in deckDTO.cardsInRemovedPile)
                {
                    Card card = cardCatalogue.FindByGuid(cardRuntimeDTO.cardGuid);
                    UnityEngine.Debug.Assert(card != null, $"Could not find card with guid {cardRuntimeDTO}.");
                    deck.AddCardToRemovedPile(card);
                }
            }
        }

        protected override DeckBuildingDTO Serialize()
        {
            return new DeckBuildingDTO(deckBuildingRecord);
        }

        protected override void SetDefaultValues()
        {
        }

        #endregion
    }
}