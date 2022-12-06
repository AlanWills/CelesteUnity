using Celeste.BoardGame;
using Celeste.BoardGame.Catalogue;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.BoardGame.Catalogue
{
    [CustomEditor(typeof(BoardGameObjectCatalogue))]
    public class BoardGameObjectCatalogueEditor : IIndexableItemsEditor<BoardGameObject>
    {
    }
}
