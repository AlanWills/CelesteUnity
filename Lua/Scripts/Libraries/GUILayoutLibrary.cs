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
        public LuaRuntime LuaRuntime { private get; set; }
        
        private const string kName = "GUILayout";
        private readonly List<LibraryFunction> functions = new();
        
        #endregion

        private GUILayoutLibrary()
        {
            functions.Add(new(kName, "label", Label));
            functions.Add(new(kName, "button", Button));
            functions.Add(new(kName, "textInput", TextInput));
            functions.Add(new(kName, "beginHorizontal", BeginHorizontal));
            functions.Add(new(kName, "endHorizontal", EndHorizontal));
            functions.Add(new(kName, "beginSection", BeginSection));
            functions.Add(new(kName, "endSection", EndSection));
        }
        
        private ValueTask<int> Label(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            string text = context.GetArgument<string>(0);
            GUILayout.Label(text ?? string.Empty);
            return new ValueTask<int>(context.Return());
        }
        
        private ValueTask<int> Button(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            string text = context.GetArgument<string>(0);
            bool result = GUILayout.Button(text ?? string.Empty);
            return new ValueTask<int>(context.Return(result));
        }
        
        private ValueTask<int> TextInput(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            string text = context.GetArgument<string>(0);
            string newText = GUILayout.TextField(text ?? string.Empty, GUILayout.MaxWidth(400));
            return new ValueTask<int>(context.Return(newText));
        }
        
        private ValueTask<int> BeginHorizontal(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            GUILayout.BeginHorizontal();
            return new ValueTask<int>(context.Return());
        }
        
        private ValueTask<int> EndHorizontal(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            GUILayout.EndHorizontal();
            return new ValueTask<int>(context.Return());
        }
        
        public ValueTask<int> BeginSection(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            GUILayout.BeginVertical(GUI.skin.box);

            if (context.ArgumentCount > 0)
            {
                string sectionTitle = context.GetArgument<string>(0);
                GUILayout.Label(sectionTitle ?? string.Empty, CelesteGUIStyles.BoldLabel.Colour(Color.white));
            }

            return new ValueTask<int>(context.Return());
        }

        public ValueTask<int> EndSection(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            GUILayout.EndVertical();
            
            return new ValueTask<int>(context.Return());
        }
    }
}
#endif