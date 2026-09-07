using Celeste.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.UI.FX
{
    public class AutoScroll : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Vector2 velocity;

        #endregion
        
        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref scrollRect);
        }

        private void Update()
        {
            scrollRect.velocity = velocity;
        }

        #endregion
    }
}