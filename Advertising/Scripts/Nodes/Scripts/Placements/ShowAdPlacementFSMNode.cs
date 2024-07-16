using Celeste.FSM;
using UnityEngine;
using XNode;

namespace Celeste.Advertising.Nodes
{
    [CreateNodeMenu(CelesteMenuItemConstants.ADVERTISING_MENU_ITEM + "Show Ad Placement")]
    public class ShowAdPlacementFSMNode : FSMNode
    {
        #region Properties and Fields

        [Output] public AdWatchResult adWatchResult;

        [SerializeField] private AdRecord adRecord;
        [SerializeField] private AdPlacement adPlacement;

        private bool inProgress = false;

        #endregion

        #region FSM Node Overrides

        protected override void OnEnter()
        {
            base.OnEnter();

            adWatchResult = AdWatchResult.Unknown;
            inProgress = adPlacement.IsLoaded;
            adRecord.PlayAdPlacement(adPlacement, OnShowCallback);
        }

        protected override FSMNode OnUpdate()
        {
            return inProgress ? this : base.OnUpdate();
        }

        protected override void OnExit()
        {
            inProgress = false;

            base.OnExit();
        }

        public override object GetValue(NodePort port)
        {
            return adWatchResult;
        }

        #endregion

        private void OnShowCallback(AdWatchResult result)
        {
            UnityEngine.Debug.Log($"Ad Placement {adPlacement.PlacementId} finished with result {result}.");
            adWatchResult = result;
            inProgress = false;
        }
    }
}
