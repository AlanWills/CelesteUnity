using Celeste.BoardGame.Components;
using Celeste.BoardGame;
using CelesteEditor.Components;
using UnityEditor;
using System;

namespace CelesteEditor.BoardGame
{
    [CustomEditor(typeof(BoardGameObject))]
    public class BoardGameObjectEditor : ComponentContainerUsingSubAssetsEditor<BoardGameObjectComponent>
    {
        #region Properties and Fields

        protected override Type[] AllComponentTypes => BoardGameEditorConstants.AllBoardGameObjectComponentTypes;
        protected override string[] AllComponentDisplayNames => BoardGameEditorConstants.AllBoardGameObjectComponentDisplayNames;

        #endregion
    }
}
