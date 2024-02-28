using Celeste.DataStructures;
using Celeste.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.Tokens
{
    #region Loca Token Struct

    [Serializable]
    public struct LocaToken : IKey
    {
        string IKey.Key => key;

        public string key;
        public UnityEngine.Object token;

        public LocaToken(string key, UnityEngine.Object token)
        {
            this.key = key;
            this.token = token;
        }
    }

    #endregion

    [CreateAssetMenu(fileName = nameof(LocaTokens), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Tokens/Loca Tokens", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class LocaTokens : ListScriptableObject<LocaToken>
    {
        #region Properties and Fields

        public char StartDelimiter => startDelimiter;
        public char EndDelimiter => endDelimiter;

        [SerializeField] private char startDelimiter = '{';
        [SerializeField] private char endDelimiter = '}';

        #endregion

        public bool HasLocaToken(string key)
        {
            return Items.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        public UnityEngine.Object FindLocaToken(string key)
        {
            return FindItem(x => string.CompareOrdinal(x.key, key) == 0).token;
        }

        public string ReplaceLocaTokens(string dialogueText, List<UnityEngine.Object> locaTokensUsed)
        {
            locaTokensUsed.Clear();

            int currentToken = 0;
            int startDelimiterIndex = dialogueText.IndexOf(startDelimiter);

            while (startDelimiterIndex != -1)
            {
                int endDelimiterIndex = dialogueText.IndexOf(endDelimiter, startDelimiterIndex + 1);
                string key = dialogueText.Substring(startDelimiterIndex + 1, endDelimiterIndex - startDelimiterIndex - 1);
                string token = currentToken.ToString();
                dialogueText = dialogueText.Remove(startDelimiterIndex + 1, key.Length);
                dialogueText = dialogueText.Insert(startDelimiterIndex + 1, token);

                UnityEngine.Object locaToken = FindLocaToken(key);
                UnityEngine.Debug.Assert(locaToken != null, $"Could not find loca token for key {key} in node.");
                locaTokensUsed.Add(locaToken);
                ++currentToken;

                startDelimiterIndex = dialogueText.IndexOf(startDelimiter, startDelimiterIndex + token.Length + 1);
            }

            return dialogueText.Trim();
        }
    }
}
