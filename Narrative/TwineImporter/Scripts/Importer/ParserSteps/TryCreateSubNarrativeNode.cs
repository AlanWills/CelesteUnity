using Celeste.FSM.Nodes;
using Celeste.Narrative.TwineImporter;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
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
            string nonLinkText = parseContext.StrippedLinksText;
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
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            
            string nonLinkText = parseContext.StrippedLinksText;
            string[] splitText = nonLinkText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            string subNarrativeKey = splitText[1];

            SubFSMNode subFSMNode = parseContext.Graph.AddNode<SubFSMNode>();
            subFSMNode.subFSM = importerSettings.FindSubNarrative(subNarrativeKey);
            UnityEngine.Debug.Assert(subFSMNode.subFSM != null, $"Could not find sub narrative {subNarrativeKey}");

            parseContext.FSMNode = subFSMNode;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(subFSMNode);
            }
#endif
        }
    }
}