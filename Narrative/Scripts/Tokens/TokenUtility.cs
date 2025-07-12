using System;
using System.Collections.Generic;
using Celeste.DataStructures;
using Object = UnityEngine.Object;

namespace Celeste.Narrative.Tokens
{
    public static class TokenUtility
    {
        public static List<LocaToken> FindLocaTokens(this LocaTokens locaTokens, string dialogueText)
        {
            List<LocaToken> foundTokens = new List<LocaToken>();
            int startDelimiterIndex = dialogueText.IndexOf(locaTokens.StartDelimiter);

            while (startDelimiterIndex != -1)
            {
                int endDelimiterIndex = dialogueText.IndexOf(locaTokens.EndDelimiter, startDelimiterIndex + 1);
                string key = dialogueText.Substring(startDelimiterIndex + 1, endDelimiterIndex - startDelimiterIndex - 1);
                var locaToken = locaTokens.FindLocaToken(key);

                if (locaToken.IsValid)
                {
                    foundTokens.Add(locaToken);
                }
                else
                {
                    UnityEngine.Debug.LogAssertion($"Could not find loca token for key {key} in node.");
                }

                startDelimiterIndex = dialogueText.IndexOf(locaTokens.StartDelimiter, endDelimiterIndex + 1);
            }
            
            return foundTokens;
        }
        
        public static string SubstituteTokens(
            string dialogueText, 
            IReadOnlyList<LocaToken> locaTokens,
            char startDelimiter = '{',
            char endDelimiter = '}')
        {
            int startDelimiterIndex = dialogueText.IndexOf(startDelimiter);

            while (startDelimiterIndex != -1)
            {
                int endDelimiterIndex = dialogueText.IndexOf(endDelimiter, startDelimiterIndex + 1);
                string key = dialogueText.Substring(startDelimiterIndex + 1, endDelimiterIndex - startDelimiterIndex - 1);
                
                int locaTokenIndex = locaTokens.FindIndex(x => string.CompareOrdinal(x.key, key) == 0);
                if (locaTokenIndex >= 0)
                {
                    Object locaToken = locaTokens[locaTokenIndex].token;
                    string locaTokenString = locaToken.ToString();
                    dialogueText = dialogueText.Replace($"{startDelimiter}{key}{endDelimiter}", locaTokenString, StringComparison.Ordinal);
                    startDelimiterIndex = dialogueText.IndexOf(startDelimiter, startDelimiterIndex + locaTokenString.Length + 1);
                }
                else
                {
                    UnityEngine.Debug.LogAssertion($"Could not find loca token for key {key} in node.");
                    startDelimiterIndex = dialogueText.IndexOf(startDelimiter, endDelimiterIndex + 1);
                }
            }

            return dialogueText.Trim();
        }
    }
}
