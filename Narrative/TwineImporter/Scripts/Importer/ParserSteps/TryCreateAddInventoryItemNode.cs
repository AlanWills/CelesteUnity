using Celeste.Events;
using Celeste.Inventory;
using Celeste.Inventory.Nodes.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    #region Inventory Item Key Struct

    [Serializable]
    public struct InventoryItemKey : IKey
    {
        string IKey.Key => key;

        public string key;
        public InventoryItem inventoryItem;

        public InventoryItemKey(string key, InventoryItem inventoryItem)
        {
            this.key = key;
            this.inventoryItem = inventoryItem;
        }
    }

    #endregion


    [CreateAssetMenu(fileName = nameof(TryCreateAddInventoryItemNode), order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Try Create Add Inventory Item Node")]
    public class TryCreateAddInventoryItemNode : TwineNodeParserStep, IUsesKeys
    {
        #region Properties and Fields

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var inventoryItem in inventoryItemKeys)
                {
                    yield return inventoryItem.key;
                }
            }
        }

        [SerializeField] private string instruction = "AddInventoryItem";
        [SerializeField] private InventoryItemEvent addInventoryItemEvent;
        [SerializeField] private List<InventoryItemKey> inventoryItemKeys = new List<InventoryItemKey>();

        #endregion

        public void AddKeyForUse(IKey key)
        {
            inventoryItemKeys.Add((InventoryItemKey)key);
        }

        public bool CouldUseKey(IKey key)
        {
            return key is InventoryItemKey;
        }

        public bool UsesKey(IKey key)
        {
            return inventoryItemKeys.Exists(x => string.CompareOrdinal(x.key, key.Key) == 0);
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
                    analysis.AddFoundInventoryItem(inventoryItemName);
                }
                else
                {
                    analysis.AddUnrecognizedKey(inventoryItemName);
                }
            }
        }
    }
}