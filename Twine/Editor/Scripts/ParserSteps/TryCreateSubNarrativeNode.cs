using Celeste.FSM.Nodes;
using Celeste.Twine;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateSubNarrativeNode), menuName = "Celeste/Twine/Parser Steps/Try Create Sub Narrative Node")]
    public class TryCreateSubNarrativeNode : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            string nonLinkText = importerSettings.StripLinksFromText(parseContext.TwineNode.Text);

            if (string.IsNullOrWhiteSpace(nonLinkText))
            {
                return false;
            }

            string[] splitText = nonLinkText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (splitText == null || splitText.Length < 2)
            {
                return false;
            }

            if (!importerSettings.IsSubNarrativeInstruction(splitText[0]))
            {
                return false;
            }

            return importerSettings.IsRegisteredSubNarrativeKey(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            
            string nonLinkText = importerSettings.StripLinksFromText(twineNode.Text);
            string[] splitText = nonLinkText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            string subNarrativeKey = splitText[1];

            SubFSMNode subFSMNode = parseContext.Graph.AddNode<SubFSMNode>();
            subFSMNode.subFSM = importerSettings.FindSubNarrative(subNarrativeKey);
            Debug.Assert(subFSMNode.subFSM != null, $"Could not find sub narrative {subNarrativeKey}");

            parseContext.FSMNode = subFSMNode;
            UnityEditor.EditorUtility.SetDirty(subFSMNode);
        }
    }
}