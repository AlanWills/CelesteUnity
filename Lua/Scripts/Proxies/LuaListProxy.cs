#if USE_LUA
using System.Collections;
using Lua;

namespace Celeste.Lua.Proxies
{
    [LuaObject(kName)]
    public partial class LuaListProxy
    {
        #region Properties and Fields
        
        public IList List { get; }
        
        private const string kName = "List";
        
        #endregion

        public LuaListProxy(IList list)
        {
            List = list;
        }
    }
}
#endif