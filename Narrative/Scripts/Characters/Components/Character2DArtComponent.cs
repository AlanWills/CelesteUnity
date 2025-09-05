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
        
        public int NumExpressionNames => expressionNames?.Length ?? 0;

        public string[] ExpressionNames
        {
            get
            {
                if (expressionNames == null || expressionNames.Length != (expressions.Count + 1))
                {
                    expressionNames = new string[expressions.Count + 1];
                    expressionNames[DefaultExpressionNameIndex] = DEFAULT_EXPRESSION_NAME;

                    for (int i = 0, n = expressions.Count; i < n; ++i)
                    {
                        expressionNames[i + 1] = expressions[i].ExpressionName;
                    }
                }
                
                return expressionNames;
            }
        }
        
        private const string DEFAULT_EXPRESSION_NAME = "Default";
        public const int DefaultExpressionNameIndex = 0;
        
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