#if USE_LUA
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lua;
using Lua.Standard;
using UnityEngine;

namespace Celeste.Lua
{
    public sealed class GUILayoutLibrary : ILuaLibrary
    {
        #region Properties and Fields
        
        public static readonly GUILayoutLibrary Instance = new();
        
        public string Name => kName;
        public IReadOnlyList<LibraryFunction> Functions => functions;
        
        private const string kName = "GUILayout";
        private readonly List<LibraryFunction> functions = new();
        
        #endregion

        private GUILayoutLibrary()
        {
            functions.Add(new(kName, "button", Button));
        }
        
        private ValueTask<int> Button(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            string text = context.GetArgument<string>(0);
            return new ValueTask<int>(context.Return(GUILayout.Button(text)));
        }
    }
}
#endif