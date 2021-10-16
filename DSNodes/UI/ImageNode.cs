using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using XNode;

namespace Celeste.DS.Nodes.UI
{
    [Serializable]
    [CreateNodeMenu("Celeste/UI/Image")]
    public class ImageNode : DataNode, IUpdateable
    {
        #region Properties and Fields

        [Input]
        public bool isEnabled;

        [Input]
        public Sprite sprite;

        [Input]
        public Image image;

        #endregion

        #region IUpdateable

        public void Update()
        {
            Image _image = GetImage();
            _image.enabled = GetInputValue(nameof(isEnabled), isEnabled);

            // Don't allow setting null sprites - use the isEnabled field instead
            Sprite _sprite = GetInputValue(nameof(sprite), sprite);
            if (_sprite != null && _image.sprite != _sprite)
            {
                _image.sprite = _sprite;
            }
        }

        #endregion

        #region Utility Methods

        private Image GetImage()
        {
            Image _image = GetInputValue(nameof(image), image);
            if (_image == null)
            {
                GameObject gameObject = GetInputValue<GameObject>(nameof(image));
                if (gameObject != null)
                {
                    _image = gameObject.GetComponent<Image>();
                }
            }

            return _image;
        }

        #endregion
    }
}
