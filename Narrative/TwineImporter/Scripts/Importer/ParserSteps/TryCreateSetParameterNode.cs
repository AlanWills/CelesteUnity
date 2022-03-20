using Celeste.FSM.Nodes.Parameters;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateSetParameterNode), menuName = "Celeste/Twine/Parser Steps/Try Create Set Parameter Node")]
    public class TryCreateSetParameterNode : TwineNodeParserStep
    {
        #region Properties and Fields

        [SerializeField] private StringValue instruction;

        #endregion

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            string[] splitText = parseContext.SplitStrippedLinksText;

            if (splitText == null || splitText.Length < 3)
            {
                return false;
            }

            if (string.CompareOrdinal(splitText[0], instruction.Value) != 0)
            {
                return false;
            }

            return importerSettings.IsRegisteredParameterKey(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            string[] splitText = parseContext.SplitStrippedLinksText;

            ScriptableObject parameter = importerSettings.FindParameter(splitText[1]);

            if (CreateNode<bool, BoolValue, BoolReference, SetBoolValueNode>(parseContext, parameter))
            {
                if (bool.TryParse(splitText[2], out bool result))
                {
                    (parseContext.FSMNode as SetBoolValueNode).newValue.Value = result;
                }
                else
                {
                    UnityEngine.Debug.LogAssertion($"Failed to parse {splitText[2]} as a bool value.");
                }
            }
            else if (CreateNode<string, StringValue, StringReference, SetStringValueNode>(parseContext, parameter))
            {
                (parseContext.FSMNode as SetStringValueNode).newValue.Value = splitText[2];
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Unhandled type for instruction.  Failed to create SetParameterNode for parameter {parameter.name} ({parameter.GetType().Name}).");
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
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.EditorUtility.SetDirty(setNode);
                }
#endif
                return true;
            }

            return false;
        }
    }
}