using Celeste.FSM;
using UnityEngine;

namespace Celeste.Advertising.Nodes
{
    [CreateNodeMenu("Celeste/Advertising/Show Ad Placement")]
    public class ShowAdPlacementFSMNode : FSMNode
    {
        #region Properties and Fields

        [SerializeField] private AdRecord adRecord;
        [SerializeField] private AdPlacement adPlacement;

        private bool inProgress = false;

        #endregion

        #region FSM Node Overrides

        protected override void OnEnter()
        {
            base.OnEnter();

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

        #endregion

        private void OnShowCallback(AdWatchResult result)
        {
            UnityEngine.Debug.Log($"Ad Placement {adPlacement.PlacementId} finished with result {result}.");
            inProgress = false;
        }
    }
}
