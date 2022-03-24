using UnityEngine;

namespace Celeste.Narrative.Tokens
{
    public static class TokenUtility
    {
        public static string SubstituteTokens(string original, Object[] dialogueTokens)
        {
            if (dialogueTokens == null || dialogueTokens.Length == 0)
            {
                return original;
            }

            return string.Format(original, dialogueTokens);
        }
    }
}
