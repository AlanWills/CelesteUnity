using Celeste.Objects;
using Lua.Unity;
using UnityEngine;

namespace Celeste.Lua.Catalogues
{
    [CreateAssetMenu(fileName = nameof(LuaScriptCatalogue), menuName = CelesteMenuItemConstants.LUA_MENU_ITEM + "Lua Script Catalogue", order = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
    public class LuaScriptCatalogue : CatalogueScriptableObject<LuaScript>
    {
    }
}