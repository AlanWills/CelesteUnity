using Celeste.BoardGame.Components;
using CelesteEditor.Components;
using System;
using UnityEditor;

namespace CelesteEditor.BoardGame.Objects
{
    [CustomEditor(typeof(Celeste.BoardGame.BoardGame))]
    public class BoardGameEditor : ComponentContainerUsingSubAssetsEditor<BoardGameComponent>
    {
        #region Properties and Fields

        protected override Type[] AllComponentTypes => BoardGameEditorConstants.AllBoardGameComponentTypes;
        protected override string[] AllComponentDisplayNames => BoardGameEditorConstants.AllBoardGameComponentDisplayNames;

        #endregion
    }
}
