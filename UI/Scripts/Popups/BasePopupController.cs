using Celeste.Events;
using Celeste.Tools;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Celeste.UI
{
    public class BasePopupController : MonoBehaviour, IPopupController
    {
        #region Properties and Fields

        Action IPopupController.RequestToClosePopup { get; set; }

        [SerializeField] private List<GameObject> popupElements = new List<GameObject>();

        [NonSerialized] private bool popupElementsBuilt = false;
        [NonSerialized] private List<IPopupElement> _popupElements = new List<IPopupElement>();

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            for (int i = popupElements.Count -1; i >= 0; --i)
            {
                if (popupElements[i] == null || popupElements[i].GetComponent<IPopupElement>() == null)
                {
                    popupElements.RemoveAt(i);
                    EditorOnly.SetDirty(this);
                }
            }
        }

        protected virtual void DoOnValidate() { }

        #endregion

        public void Show(IPopupArgs args)
        {
            BuildPopupElements();

            for (int i = 0; i < _popupElements.Count; ++i)
            {
                _popupElements[i].OnShow(args);
            }

            OnShow(args);
        }

        public void Hide()
        {
            BuildPopupElements();

            for (int i = 0; i < _popupElements.Count; ++i)
            {
                _popupElements[i].OnHide();
            }

            OnHide();
        }

        public void ConfirmPressed() 
        {
            BuildPopupElements();

            for (int i = 0; i < _popupElements.Count; ++i)
            {
                _popupElements[i].OnConfirmPressed();
            }

            OnConfirmPressed();
        }

        public void ClosePressed() 
        {
            BuildPopupElements();

            for (int i = 0; i < _popupElements.Count; ++i)
            {
                _popupElements[i].OnClosePressed();
            }

            OnClosePressed();
        }

        protected virtual void OnShow(IPopupArgs args) { }
        protected virtual void OnHide() { }

        protected virtual void OnConfirmPressed() { }
        protected virtual void OnClosePressed() { }

        protected void RequestToClosePopup()
        {
            (this as IPopupController).RequestToClosePopup?.Invoke();
        }

        [Conditional("UNITY_EDITOR")]
        public void FindPopupElements_EditorOnly()
        {
            HashSet<GameObject> gameObjectsWithPopupElements = new HashSet<GameObject>();
            foreach (IPopupElement popupElement in gameObject.GetComponentsInChildren<IPopupElement>())
            {
                gameObjectsWithPopupElements.Add((popupElement as Component).gameObject);
            }

            popupElements.Clear();
            popupElements.AddRange(gameObjectsWithPopupElements);
            EditorOnly.SetDirty(this);
        }

        private void BuildPopupElements()
        {
            if (popupElementsBuilt) return;

            _popupElements.Clear();
            _popupElements.Capacity = popupElements.Count;

            for (int i = 0; i < popupElements.Count; ++i)
            {
                _popupElements.AddRange(popupElements[i].GetComponents<IPopupElement>());
            }

            popupElementsBuilt = true;
        }
    }
}