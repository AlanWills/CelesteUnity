using Celeste.BoardGame.Components;
using Celeste.BoardGame.Tokens;
using CelesteEditor.Components;
using System;
using UnityEditor;

namespace CelesteEditor.BoardGame.Tokens
{
    [CustomEditor(typeof(Token))]
    public class TokenEditor : ComponentContainerUsingSubAssetsEditor<TokenComponent>
    {
        #region Properties and Fields

        protected override Type[] AllComponentTypes => BoardGameEditorConstants.AllTokenComponentTypes;
        protected override string[] AllComponentDisplayNames => BoardGameEditorConstants.AllTokenComponentDisplayNames;

        #endregion
    }
}
