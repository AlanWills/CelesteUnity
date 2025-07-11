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

        public bool IsValid => !string.IsNullOrWhiteSpace(key) && token != null;
        
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

        public LocaToken FindLocaToken(string key)
        {
            return FindItem(x => string.CompareOrdinal(x.key, key) == 0);
        }
    }
}
