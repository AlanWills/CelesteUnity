using Celeste.Logic;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Advertising.Nodes.Conditions
{
    [DisplayName("Ad Placement Loaded")]
    public class AdPlacementLoadedCondition : Condition
    {
        #region Properties and Fields

        [SerializeField] private AdPlacement adPlacement;
        [SerializeField] private bool trueIfLoaded = true;

        #endregion

        public override void CopyFrom(Condition original)
        {
            AdPlacementLoadedCondition adPlacementLoadedCondition = original as AdPlacementLoadedCondition;
            adPlacement = adPlacementLoadedCondition.adPlacement;
            trueIfLoaded = adPlacementLoadedCondition.trueIfLoaded;
        }

        public override void SetTarget(object arg)
        {
            trueIfLoaded = (bool)arg;
        }

        protected override bool DoCheck()
        {
            return adPlacement.IsLoaded == trueIfLoaded;
        }
    }
}
