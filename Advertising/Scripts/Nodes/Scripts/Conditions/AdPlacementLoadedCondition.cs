using Celeste.Events;
using Celeste.Logic;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Advertising.Nodes.Conditions
{
    [DisplayName("Ad Placement Loaded")]
    [CreateAssetMenu(fileName = nameof(AdPlacementLoadedCondition), menuName = "Celeste/Advertising/Logic/Ad Placement Loaded Condition")]
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

        protected override void DoInit()
        {
            adPlacement.AddIsLoadedChangedCallback(OnAdIsLoadedChanged);
        }

        protected override void DoShutdown()
        {
            adPlacement.RemoveIsLoadedChangedCallback(OnAdIsLoadedChanged);
        }

        protected override bool DoCheck()
        {
            return adPlacement.IsLoaded == trueIfLoaded;
        }

        #region Callbacks

        private void OnAdIsLoadedChanged(ValueChangedArgs<bool> callback)
        {
            Check();
        }

        #endregion
    }
}
