using Celeste.FSM.Nodes.Parameters;
using Celeste.Narrative;
using Celeste.Parameters;
using UnityEngine;

namespace CelesteEditor.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryCreateSetParameterNode", menuName = "Celeste/Narrative/Twine/Parser Steps/Try Create Set Parameter Node")]
    public class TryCreateSetParameterNode : TwineNodeParserStep
    {
        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            string text = parseContext.TwineNode.text;

            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            string[] splitText = text.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (splitText == null || splitText.Length < 3)
            {
                return false;
            }

            if (!parseContext.ImporterSettings.IsSetParameterInstruction(splitText[0]))
            {
                return false;
            }

            return parseContext.ImporterSettings.IsRegisteredParameterKey(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            string[] splitText = parseContext.TwineNode.text.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            ScriptableObject parameter = parseContext.ImporterSettings.FindParameter(splitText[1]);

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