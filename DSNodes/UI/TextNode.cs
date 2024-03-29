using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XNode;

namespace Celeste.DS.Nodes.UI
{
    [Serializable]
    [CreateNodeMenu("Celeste/UI/Text")]
    public class TextNode : DataNode, IUpdateable
    {
        #region Properties and Fields

        [Input]
        public string value;

        [Input]
        public string format;

        [Input]
        public Text text;

        #endregion

        #region IUpdateable

        public void Update()
        {
            if (text == null)
            {
                GetText();
            }

            string currentValue = GetInputValue("value", value);
            if (currentValue != text.text)
            {
                string _format = GetInputValue("format", format);
                text.text = string.IsNullOrEmpty(_format) ? currentValue : string.Format(_format, currentValue);
            }
        }

        #endregion

        #region Utility Methods

        private Text GetText()
        {
            text = GetInputValue(nameof(text), text);
            if (text == null)
            {
                GameObject gameObject = GetInputValue<GameObject>(nameof(text));
                if (gameObject != null)
                {
                    text = gameObject.GetComponent<Text>();
                    Debug.Assert(text != null, $"Could not find Text component on Game Object {gameObject.name} in node {name} in {graph.name}.");
                }
                else
                {
                    Debug.Assert(text != null, $"Could not find Text component or Game Object in {name} in {graph.name}.");
                }
            }

            return text;
        }

        #endregion
    }
}
