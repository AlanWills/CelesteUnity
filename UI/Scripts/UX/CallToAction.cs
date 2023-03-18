using System;
using Celeste.Events;
using Celeste.Logic;
using UnityEngine;

namespace Celeste.UI.UX
{
    [AddComponentMenu("Celeste/UI/UX/Call To Action")]
    public class CallToAction : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Condition callToActionCondition;
        [SerializeField] private GameObject callToActionUI;

        #endregion

        private void RefreshUI()
        {
            callToActionUI.SetActive(callToActionCondition.IsMet);
        }
        
        #region Unity Methods

        private void OnEnable()
        {
            RefreshUI();
            
            callToActionCondition.AddOnIsMetConditionChanged(OnCallToActionConditionMetChanged);
        }

        private void OnDisable()
        {
            callToActionCondition.RemoveOnIsMetConditionChanged(OnCallToActionConditionMetChanged);
        }

        #endregion
        
        #region Callbacks

        private void OnCallToActionConditionMetChanged(ValueChangedArgs<bool> args)
        {
            RefreshUI();
        }
        
        #endregion
    }
}