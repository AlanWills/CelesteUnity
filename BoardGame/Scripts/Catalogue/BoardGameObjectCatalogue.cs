using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame.Catalogue
{
    [CreateAssetMenu(fileName = nameof(BoardGameObjectCatalogue), menuName = "Celeste/Board Game/Catalogue/Board Game Object Catalogue")]
    public class BoardGameObjectCatalogue : List<BoardGameObject>
    {
        public BoardGameObject FindByGuid(int guid)
        {
            return Find(x => x.Guid == guid);
        }
    }
}
