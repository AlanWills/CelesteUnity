using Celeste.BoardGame.Components;
using CelesteEditor.Tools.Utils;
using System;

namespace CelesteEditor.BoardGame
{
    public static class BoardGameEditorConstants
    {
        #region Properties and Fields

        public static readonly Type[] AllBoardGameComponentTypes;
        public static readonly string[] AllBoardGameComponentDisplayNames;
        
        public static readonly Type[] AllBoardGameObjectComponentTypes;
        public static readonly string[] AllBoardGameObjectComponentDisplayNames;

        #endregion

        static BoardGameEditorConstants()
        {
            TypeExtensions.LoadTypes<BoardGameComponent>(ref AllBoardGameComponentTypes, ref AllBoardGameComponentDisplayNames);
            TypeExtensions.LoadTypes<BoardGameObjectComponent>(ref AllBoardGameObjectComponentTypes, ref AllBoardGameObjectComponentDisplayNames);
        }
    }
}