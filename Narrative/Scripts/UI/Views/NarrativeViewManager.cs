using Celeste.FSM;
using Celeste.Narrative.Loading;
using Celeste.Scene.Events;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Narrative View Manager")]
    public class NarrativeViewManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private NarrativeView[] narrativeViews;

        #endregion

        #region Callbacks

        public void OnNarrativeBegin(ILinearRuntime narrativeRuntime)
        {
            for (int i = 0, n = narrativeViews.Length; i < n; ++i)
            {
                narrativeViews[i].OnNarrativeBegin();
                narrativeViews[i].gameObject.SetActive(false);
            }

            if (narrativeRuntime.CurrentNode != null)
            {
                OnNodeEnter(narrativeRuntime.CurrentNode);
            }
        }

        public void OnNarrativeEnd(ILinearRuntime narrativeRuntime)
        {
            for (int i = 0, n = narrativeViews.Length; i < n; ++i)
            {
                narrativeViews[i].OnNarrativeEnd();
                narrativeViews[i].gameObject.SetActive(false);
            }
        }

        public void OnNodeEnter(FSMNode fsmNode)
        {
            for (int i = 0, n = narrativeViews.Length; i < n; ++i)
            {
                NarrativeView narrativeView = narrativeViews[i];
                bool isValid = narrativeView.IsValidForNode(fsmNode);
                narrativeView.gameObject.SetActive(isValid);
                
                if (isValid)
                {
                    narrativeView.OnNodeEnter(fsmNode);
                }
            }
        }

        public void OnNodeUpdate(FSMNode fsmNode)
        {
            for (int i = 0, n = narrativeViews.Length; i < n; ++i)
            {
                if (narrativeViews[i].isActiveAndEnabled)
                {
                    narrativeViews[i].OnNodeUpdate(fsmNode);
                }
            }
        }

        public void OnNodeExit(FSMNode fsmNode)
        {
            for (int i = 0, n = narrativeViews.Length; i < n; ++i)
            {
                if (narrativeViews[i].isActiveAndEnabled)
                {
                    narrativeViews[i].OnNodeExit(fsmNode);
                }
            }
        }

        #endregion
    }
}
