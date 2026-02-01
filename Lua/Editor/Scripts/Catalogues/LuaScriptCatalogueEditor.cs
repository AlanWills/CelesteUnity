#if USE_LUA
using Celeste.Lua.Catalogues;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace Lua.Unity.Editor.Catalogues
{
    [CustomEditor(typeof(LuaScriptCatalogue))]
    public class LuaScriptCatalogueEditor : IIndexableItemsEditor<LuaScript>
    {
    }
}
#endif