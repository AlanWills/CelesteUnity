using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Debug.UI
{
    [AddComponentMenu("Celeste/Debug/UI/Toggle Animate Debug UI")]
    public class ToggleAnimateDebugUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Animator uiAnimator;
        [SerializeField] private string showAnimName = "Show";

        private int showAnimHash;
        [NonSerialized] private bool isShown = false;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            showAnimHash = Animator.StringToHash(showAnimName);

            isShown = false;
            uiAnimator.SetBool(showAnimHash, isShown);
        }

        #endregion

        #region Callbacks

        public void Toggle()
        {
            isShown = !isShown;
            uiAnimator.SetBool(showAnimHash, isShown);

        }

        #endregion
    }
}
