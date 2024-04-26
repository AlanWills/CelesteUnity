using Celeste.DeckBuilding.Cards;
using Celeste.Tools;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DeckBuilding.Utils
{
    public static class CreateCardsFromSprites
    {
        [Serializable]
        public struct CreateCardsFromSpritesParameters
        {
            public string outputFolder;
            public Sprite cardBack;
            public List<Sprite> cardFronts;
        }

        public static List<Card> CreateCards(CreateCardsFromSpritesParameters args)
        {
            EditorOnly.CreateFolder(args.outputFolder);

            List<Card> cards = new List<Card>();

            foreach (Sprite sprite in args.cardFronts)
            {
                Card card = ScriptableObject.CreateInstance<Card>();
                card.name = sprite.name;
                card.CardFront = sprite;
                card.CardBack = args.cardBack;

                AssetDatabase.CreateAsset(card, $"{args.outputFolder}/{card.name}.asset");
            }

            return cards;
        }
    }
}
