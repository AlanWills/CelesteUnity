using Celeste.BoardGame.Components;
using CelesteEditor.Tools.Utils;
using System;

namespace CelesteEditor.BoardGame
{
    public static class BoardGameEditorConstants
    {
        #region Properties and Fields

        public static readonly Type[] AllTokenComponentTypes;
        public static readonly string[] AllTokenComponentDisplayNames;

        #endregion

        static BoardGameEditorConstants()
        {
            TypeUtils.LoadTypes<TokenComponent>(ref AllTokenComponentTypes, ref AllTokenComponentDisplayNames);
        }
    }
}