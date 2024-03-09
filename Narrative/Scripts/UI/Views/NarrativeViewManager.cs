﻿using Celeste.FSM;
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
                narrativeViews[i].gameObject.SetActive(false);
            }

            narrativeRuntime.OnNodeEnter.AddListener(OnNodeEnter);
            narrativeRuntime.OnNodeUpdate.AddListener(OnNodeUpdate);
            narrativeRuntime.OnNodeExit.AddListener(OnNodeExit);

            if (narrativeRuntime.CurrentNode != null)
            {
                OnNodeEnter(narrativeRuntime.CurrentNode);
            }
        }

        public void OnNarrativeEnd(ILinearRuntime narrativeRuntime)
        {
            for (int i = 0, n = narrativeViews.Length; i < n; ++i)
            {
                narrativeViews[i].gameObject.SetActive(false);
            }

            narrativeRuntime.OnNodeEnter.RemoveListener(OnNodeEnter);
            narrativeRuntime.OnNodeUpdate.RemoveListener(OnNodeUpdate);
            narrativeRuntime.OnNodeExit.RemoveListener(OnNodeExit);
        }

        private void OnNodeEnter(FSMNode fsmNode)
        {
            for (int i = 0, n = narrativeViews.Length; i < n; ++i)
            {
                NarrativeView narrativeView = narrativeViews[i];

                if (narrativeView.IsValidForNode(fsmNode))
                {
                    narrativeView.gameObject.SetActive(true);
                    narrativeView.OnNodeEnter(fsmNode);
                }
            }
        }

        private void OnNodeUpdate(FSMNode fsmNode)
        {
            for (int i = 0, n = narrativeViews.Length; i < n; ++i)
            {
                if (narrativeViews[i].isActiveAndEnabled)
                {
                    narrativeViews[i].OnNodeUpdate(fsmNode);
                }
            }
        }

        private void OnNodeExit(FSMNode fsmNode)
        {
            for (int i = 0, n = narrativeViews.Length; i < n; ++i)
            {
                if (narrativeViews[i].isActiveAndEnabled)
                {
                    narrativeViews[i].OnNodeExit(fsmNode);
                    narrativeViews[i].gameObject.SetActive(false);
                }
            }
        }

        #endregion
    }
}
