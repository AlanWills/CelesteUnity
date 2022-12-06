using Celeste.Objects;
using UnityEngine;

namespace Celeste.BoardGame.Catalogue
{
    [CreateAssetMenu(fileName = nameof(BoardGameObjectCatalogue), menuName = "Celeste/Board Game/Catalogue/Board Game Object Catalogue")]
    public class BoardGameObjectCatalogue : ListScriptableObject<BoardGameObject>
    {
        public BoardGameObject FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}
