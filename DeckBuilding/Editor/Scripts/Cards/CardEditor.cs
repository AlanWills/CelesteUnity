using Celeste.Components;
using Celeste.DeckBuilding.Cards;
using CelesteEditor.Components;
using System;
using UnityEditor;

namespace CelesteEditor.DeckBuilding.Cards
{
    [CustomEditor(typeof(Card))]
    public class CardEditor : ComponentContainerUsingSubAssetsEditor<Component>
    {
        #region Properties and Fields

        protected override Type[] AllComponentTypes => CardEditorConstants.AllCardComponentTypes;
        protected override string[] AllComponentDisplayNames => CardEditorConstants.AllCardComponentDisplayNames;

        #endregion
    }
}