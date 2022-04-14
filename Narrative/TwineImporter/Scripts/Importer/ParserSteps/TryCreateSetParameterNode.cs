using Celeste.FSM.Nodes.Parameters;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [Serializable]
    public struct ParameterKey : IKey
    {
        string IKey.Key => key;

        public string key;
        public ScriptableObject parameter;

        public ParameterKey(string key, ScriptableObject parameter)
        {
            this.key = key;
            this.parameter = parameter;
        }
    }

    [CreateAssetMenu(fileName = nameof(TryCreateSetParameterNode), menuName = "Celeste/Twine/Parser Steps/Try Create Set Parameter Node")]
    public class TryCreateSetParameterNode : TwineNodeParserStep, IUsesKeys
    {
        #region Properties and Fields

        [SerializeField] private string instruction = "SetParameter";
        [SerializeField] private char parameterStartDelimiter = '$';
        [SerializeField] private char parameterEndDelimiter = '$';
        [SerializeField] private List<ParameterKey> parameterKeys = new List<ParameterKey>();

        #endregion

        public void AddKeyForUse(IKey key)
        {
            parameterKeys.Add((ParameterKey)key);
        }

        public bool CouldUseKey(IKey key)
        {
            return key is ParameterKey;
        }

        public bool UsesKey(IKey key)
        {
            return parameterKeys.Exists(x => string.CompareOrdinal(x.key, key.Key) == 0);
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext analyseContext)
        {
            return !string.IsNullOrWhiteSpace(analyseContext.StrippedLinksText);
        }

        public override void Analyse(TwineNodeAnalyseContext analyseContext)
        {
            FindParameters(analyseContext);
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

            if (splitText == null || splitText.Length < 3)
            {
                return false;
            }

            if (!IsInstruction(splitText[0]))
            {
                return false;
            }

            return HasParameter(splitText[1], true);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            string[] splitText = parseContext.SplitStrippedLinksText;

            ScriptableObject parameter = FindParameter(splitText[1]);

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

        #endregion

        private bool IsInstruction(string str)
        {
            return string.CompareOrdinal(instruction, str) == 0;
        }

        private string StripParameterDelimiters(string key)
        {
            if (!string.IsNullOrEmpty(key) &&
                key[0] == parameterStartDelimiter &&
                key[key.Length - 1] == parameterEndDelimiter)
            {
                key = key.Substring(1, key.Length - 2);
            }

            return key;
        }

        private bool HasParameter(string key, bool stripDelimiters)
        {
            if (stripDelimiters)
            {
                key = StripParameterDelimiters(key);
            }

            return parameterKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        private ScriptableObject FindParameter(string key)
        {
            key = StripParameterDelimiters(key);
            return parameterKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).parameter;
        }

        private void FindParameters(TwineNodeAnalyseContext analyseContext)
        {
            string text = analyseContext.TwineNode.Text;
            TwineStoryAnalysis analysis = analyseContext.Analysis;

            foreach (string key in Twine.Tokens.Get(text, parameterStartDelimiter, parameterEndDelimiter))
            {
                if (HasParameter(key, false))
                {
                    analysis.AddFoundParameter(key);
                }
                else
                {
                    analysis.AddUnrecognizedKey(key);
                }
            }

            // We need to check the links too, so don't use the stripped text in the analyze context
            if (!string.IsNullOrWhiteSpace(text))
            {
                string[] splitText = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (splitText != null &&
                    splitText.Length >= 2 &&
                    IsInstruction(splitText[0]))
                {
                    string parameterName = StripParameterDelimiters(splitText[1]);

                    if (HasParameter(parameterName, false))
                    {
                        analysis.AddFoundParameter(parameterName);
                    }
                    else
                    {
                        analysis.AddUnrecognizedKey(parameterName);
                    }
                }
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