﻿using Celeste.Advertising;
using System;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = nameof(AdWatchResultCondition), menuName = "Celeste/Advertising/Logic/Ad Watch Result Condition")]
    public class AdWatchResultCondition : Condition
    {
        #region Properties and Fields

        [SerializeField] private ConditionOperator conditionOperator;
        [SerializeField] private AdWatchResult target;

        [NonSerialized] private AdWatchResult runtimeVariable;

        #endregion

        public override void CopyFrom(Condition original)
        {
            AdWatchResultCondition adWatchResultCondition = original as AdWatchResultCondition;
            conditionOperator = adWatchResultCondition.conditionOperator;
            target = adWatchResultCondition.target;
        }

        public override void SetVariable(object arg)
        {
            target = (AdWatchResult)arg;
        }

        protected override bool DoCheck()
        {
            return Compare.SatisfiesComparison((int)runtimeVariable, conditionOperator, (int)target);
        }
    }
}