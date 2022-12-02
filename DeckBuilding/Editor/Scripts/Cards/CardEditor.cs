using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Components;
using CelesteEditor.Components;
using System;
using UnityEditor;

namespace CelesteEditor.DeckBuilding.Cards
{
    [CustomEditor(typeof(Card))]
    public class CardEditor : ComponentContainerUsingSubAssetsEditor<CardComponent>
    {
        #region Properties and Fields

        protected override Type[] AllComponentTypes => CardEditorConstants.AllCardComponentTypes;
        protected override string[] AllComponentDisplayNames => CardEditorConstants.AllCardComponentDisplayNames;

        #endregion
    }
}