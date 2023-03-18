using Celeste.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.UI
{
    public class GraphicDetailContext : IDetailContext
    {
        public Sprite sprite;

        public GraphicDetailContext(Sprite sprite)
        {
            this.sprite = sprite;
        }

        public GraphicDetailContext(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<SpriteRenderer>(out var graphic))
            {
                sprite = graphic.sprite;
            }
            else if (gameObject.TryGetComponent<Image>(out var image))
            {
                sprite = image.sprite;
            }
        }
    }

    [AddComponentMenu("Celeste/Board Game/UI/Graphic Detail UI Controller")]
    public class GraphicDetailUIController : DetailUIController
    {
        #region Properties and Fields

        [SerializeField] private GameObject detailUIRoot;
        [SerializeField] private Image image;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (detailUIRoot == null)
            {
                detailUIRoot = gameObject;
            }

            this.TryGetInChildren(ref image);
        }

        #endregion

        #region Detail UI Controller

        public override bool IsValidFor(IDetailContext detailContext)
        {
            return detailContext is GraphicDetailContext;
        }

        public override void Show(IDetailContext detailContext)
        {
            GraphicDetailContext cardDetailContext = detailContext as GraphicDetailContext;
            image.sprite = cardDetailContext.sprite;
            detailUIRoot.SetActive(true);
        }

        public override void Hide()
        {
            detailUIRoot.SetActive(false);
        }

        #endregion
    }
}
