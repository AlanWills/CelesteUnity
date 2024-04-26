using Celeste.DeckBuilding.Components;
using CelesteEditor.Tools.Utils;
using System;

namespace CelesteEditor.DeckBuilding.Cards
{
    public static class CardEditorConstants
    {
        #region Properties and Fields

        public static readonly Type[] AllCardComponentTypes;
        public static readonly string[] AllCardComponentDisplayNames;

        #endregion

        static CardEditorConstants()
        {
            TypeExtensions.LoadTypes<CardComponent>(ref AllCardComponentTypes, ref AllCardComponentDisplayNames);
        }
    }
}