using Celeste.FSM.Nodes.Parameters;
using Celeste.Narrative;
using Celeste.Parameters;
using Celeste.Twine;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryCreateSetParameterNode", menuName = "Celeste/Twine/Parser Steps/Try Create Set Parameter Node")]
    public class TryCreateSetParameterNode : TwineNodeParserStep
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

            if (splitText == null || splitText.Length < 3)
            {
                return false;
            }

            if (!importerSettings.IsSetParameterInstruction(splitText[0]))
            {
                return false;
            }

            return importerSettings.IsRegisteredParameterKey(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;

            string nonLinkText = importerSettings.StripLinksFromText(twineNode.Text);
            string[] splitText = nonLinkText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            ScriptableObject parameter = importerSettings.FindParameter(splitText[1]);

            if (CreateNode<bool, BoolValue, BoolReference, SetBoolValueNode>(parseContext, parameter))
            {
                if (bool.TryParse(splitText[2], out bool result))
                {
                    (parseContext.FSMNode as SetBoolValueNode).newValue.Value = result;
                }
                else
                {
                    Debug.LogAssertion($"Failed to parse {splitText[2]} as a bool value.");
                }
            }
            else if (CreateNode<string, StringValue, StringReference, SetStringValueNode>(parseContext, parameter))
            {
                (parseContext.FSMNode as SetStringValueNode).newValue.Value = splitText[2];
            }
            else
            {
                Debug.LogAssertion($"Unhandled type for instruction.  Failed to create SetParameterNode for parameter {parameter.name} ({parameter.GetType().Name}).");
            }
        }

        private bool CreateNode<T, TValue, TReference, TNode>(TwineNodeParseContext context, ScriptableObject parameter)
            where TValue : ParameterValue<T>
            where TReference : ParameterReference<T, TValue, TReference>
            where TNode : SetValueNode<T, TValue, TReference>
        {
            if (parameter is TValue)
            {
                TNode setNode = context.Graph.AddNode<TNode>();
                setNode.value = parameter as TValue;
                context.FSMNode = setNode;
                
                UnityEditor.EditorUtility.SetDirty(setNode);
                return true;
            }

            return false;
        }
    }
}