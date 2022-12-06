using Celeste.DeckBuilding.Catalogue;
using Celeste.DeckBuilding.Decks;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.DeckBuilding
{
    [CustomEditor(typeof(DeckCatalogue))]
    public class DeckCatalogueEditor : IIndexableItemsEditor<Deck>
    {
    }
}
