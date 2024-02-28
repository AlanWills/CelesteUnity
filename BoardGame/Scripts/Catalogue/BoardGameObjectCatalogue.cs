using Celeste.Objects;
using UnityEngine;

namespace Celeste.BoardGame.Catalogue
{
    [CreateAssetMenu(
        fileName = nameof(BoardGameObjectCatalogue), 
        menuName = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Catalogue/Board Game Object Catalogue",
        order = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
    public class BoardGameObjectCatalogue : ListScriptableObject<BoardGameObject>
    {
        public BoardGameObject FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}
