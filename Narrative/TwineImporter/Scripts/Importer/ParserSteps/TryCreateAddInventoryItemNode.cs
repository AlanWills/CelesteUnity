using Celeste.Events;
using Celeste.Inventory;
using Celeste.Inventory.Nodes.Events;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateAddInventoryItemNode), menuName = "Celeste/Twine/Parser Steps/Try Create Add Inventory Item Node")]
    public class TryCreateAddInventoryItemNode : TwineNodeParserStep, IUsesKeys
    {
        #region Inventory Item Key Struct

        [Serializable]
        public struct InventoryItemKey
        {
            public string key;
            public InventoryItem inventoryItem;

            public InventoryItemKey(string key, InventoryItem inventoryItem)
            {
                this.key = key;
                this.inventoryItem = inventoryItem;
            }
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private string instruction = "AddInventoryItem";
        [SerializeField] private InventoryItemEvent addInventoryItemEvent;
        [SerializeField] private List<InventoryItemKey> inventoryItemKeys = new List<InventoryItemKey>();

        #endregion

        public void AddKeyForUse(string key, object inventoryItem)
        {
            inventoryItemKeys.Add(new InventoryItemKey(key, inventoryItem as InventoryItem));
        }

        public bool CouldUseKey(string key, object inventoryItem)
        {
            return inventoryItem as InventoryItem;
        }

        public bool UsesKey(string key)
        {
            return inventoryItemKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            return !string.IsNullOrWhiteSpace(parseContext.StrippedLinksText);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            FindInventoryItems(parseContext.SplitStrippedLinksText, parseContext.Analysis);
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            string[] splitText = parseContext.SplitStrippedLinksText;

            if (splitText == null || splitText.Length < 2)
            {
                return false;
            }

            if (string.CompareOrdinal(splitText[0], instruction) != 0)
            {
                return false;
            }

            return HasInventoryItem(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            string[] splitText = parseContext.SplitStrippedLinksText;

            InventoryItem inventoryItem = FindInventoryItem(splitText[1]);
            InventoryItemEventRaiserNode inventoryItemEventRaiserNode = parseContext.Graph.AddNode<InventoryItemEventRaiserNode>();
            inventoryItemEventRaiserNode.argument.Value = inventoryItem;
            inventoryItemEventRaiserNode.toRaise = addInventoryItemEvent;

            parseContext.FSMNode = inventoryItemEventRaiserNode;
        }

        #endregion

        private bool IsInstruction(string str)
        {
            return string.CompareOrdinal(instruction, str) == 0;
        }

        private bool HasInventoryItem(string key)
        {
            return inventoryItemKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        private InventoryItem FindInventoryItem(string key)
        {
            return inventoryItemKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).inventoryItem;
        }

        private void FindInventoryItems(string[] splitStrippedLinkText, TwineStoryAnalysis analysis)
        {
            if (splitStrippedLinkText != null &&
                splitStrippedLinkText.Length >= 2 &&
                IsInstruction(splitStrippedLinkText[0]))
            {
                string inventoryItemName = splitStrippedLinkText[1];

                if (HasInventoryItem(inventoryItemName))
                {
                    analysis.foundInventoryItems.Add(inventoryItemName);
                }
                else
                {
                    analysis.unrecognizedKeys.Add(inventoryItemName);
                }
            }
        }
    }
}