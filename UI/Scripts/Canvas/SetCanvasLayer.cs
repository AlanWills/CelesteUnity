using Celeste.Tools;
using UnityEngine;

namespace Celeste.UI
{
    [AddComponentMenu("Celeste/UI/Set Canvas Layer")]
    [RequireComponent(typeof(Canvas))]
    public class SetCanvasLayer : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private CanvasLayer canvasLayer;
        [SerializeField] private Canvas canvas;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref canvas);
        }

        private void Awake()
        {
            Apply();
        }

        #endregion

        public void Apply()
        {
            Debug.Assert(canvasLayer != null, $"Canvas Layer is not set on {nameof(SetCanvasLayer)} script on {gameObject.name}.");
            Debug.Assert(canvas != null, $"Canvas is not set on {nameof(SetCanvasLayer)} script on {gameObject.name}.");
            canvas.sortingOrder = canvasLayer.SortOrder;
        }
    }
}