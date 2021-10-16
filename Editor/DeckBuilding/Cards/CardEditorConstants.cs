using Celeste.DeckBuilding.Cards;
using CelesteEditor.Tools.Utils;
using System;
using UnityEditor;
using UnityEngine;

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
            TypeUtils.LoadTypes<Celeste.DeckBuilding.Cards.Component>(ref AllCardComponentTypes, ref AllCardComponentDisplayNames);
        }
    }
}