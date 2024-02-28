using Celeste.Events;
using Celeste.Logic;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Advertising.Nodes.Conditions
{
    [DisplayName("Ad Placement Loaded")]
    [CreateAssetMenu(
        fileName = nameof(AdPlacementLoadedCondition), 
        menuName = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM + "Logic/Ad Placement Loaded Condition",
        order = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM_PRIORITY)]
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

        public override void SetVariable(object arg)
        {
            trueIfLoaded = (bool)arg;
        }

        protected override void DoInitialize()
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
