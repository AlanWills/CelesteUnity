using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.Characters.Components
{
    public class Character2DArtComponent : CharacterComponent
    {
        #region Expression

        [Serializable]
        public struct Expression
        {
            public string ExpressionName;
            public Sprite Sprite;
        }
        
        #endregion
        
        #region Properties and Fields

        public string[] ExpressionNames
        {
            get
            {
                if (expressionNames == null || expressionNames.Length != expressions.Count)
                {
                    expressionNames = new string[expressions.Count];

                    for (int i = 0, n = expressionNames.Length; i < n; ++i)
                    {
                        expressionNames[i] = expressions[i].ExpressionName;
                    }
                }
                
                return expressionNames;
            }
        }
        
        [SerializeField] private Sprite defaultExpression;
        [SerializeField] private List<Expression> expressions = new();
        
        [NonSerialized] private string[] expressionNames;
        
        #endregion

        public Sprite GetSpriteForExpression(string expressionName)
        {
            if (string.IsNullOrEmpty(expressionName))
            {
                return defaultExpression;
            }
            
            int expressionIndex = expressions.FindIndex(x => string.CompareOrdinal(x.ExpressionName, expressionName) == 0);
            return expressionIndex >= 0 ? expressions[expressionIndex].Sprite : defaultExpression;
        }
    }
}