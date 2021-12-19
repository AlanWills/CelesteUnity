using Celeste.Inventory;
using Celeste.Inventory.Nodes.Events;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Nodes.Events;
using Celeste.Twine;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateAddInventoryItemNode), menuName = "Celeste/Twine/Parser Steps/Try Create Add Inventory Item Node")]
    public class TryCreateAddInventoryItemNode : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            string nonLinkText = parseContext.StrippedLinksText;
            string[] splitText = nonLinkText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (splitText == null || splitText.Length < 2)
            {
                return false;
            }

            if (!importerSettings.IsAddInventoryItemInstruction(splitText[0]))
            {
                return false;
            }

            return importerSettings.IsRegisteredInventoryItemKey(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;

            string nonLinkText = parseContext.StrippedLinksText;
            string[] splitText = nonLinkText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            InventoryItem inventoryItem = importerSettings.FindInventoryItem(splitText[1]);
            InventoryItemEventRaiserNode inventoryItemEventRaiserNode = parseContext.Graph.AddNode<InventoryItemEventRaiserNode>();
            inventoryItemEventRaiserNode.argument.Value = inventoryItem;
            inventoryItemEventRaiserNode.toRaise = importerSettings.addInventoryItemEvent;

            parseContext.FSMNode = inventoryItemEventRaiserNode;
        }
    }
}