using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.UI
{
    [AddComponentMenu("Celeste/UI/Graphic Detail Context Provider")]
    public class GraphicDetailContextProvider : MonoBehaviour, IDetail
    {
        #region Properties and Fields

        [SerializeField] private bool useSpriteRenderer = true;
        [SerializeField, ShowIf(nameof(useSpriteRenderer))] private SpriteRenderer spriteRenderer;
        [SerializeField, HideIf(nameof(useSpriteRenderer))] private Image image;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (spriteRenderer == null && image == null)
            {
                if (TryGetComponent(out spriteRenderer))
                {
                    useSpriteRenderer = true;
                }
                else if (TryGetComponent(out image))
                {
                    useSpriteRenderer = false;
                }
            }
            else if (useSpriteRenderer && spriteRenderer == null)
            {
                this.TryGet(ref spriteRenderer);
                image = null;
            }
            else if (!useSpriteRenderer && image == null)
            {
                this.TryGet(ref image);
                spriteRenderer = null;
            }
        }

        #endregion

        public IDetailContext CreateDetailContext()
        {
            return new GraphicDetailContext(useSpriteRenderer ? spriteRenderer.sprite : image.sprite);
        }
    }
}
