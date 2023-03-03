using Celeste.Objects;
using System;
using TMPro;
using UnityEngine;

namespace Celeste.DS.Nodes.UI
{
    [Serializable]
    [CreateNodeMenu("Celeste/UI/TMP Text")]
    public class TMPTextNode : DataNode, IUpdateable
    {
        #region Properties and Fields

        [Input]
        public string value;

        [Input]
        public string format;

        [Input]
        public TMP_Text text;

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

        private TMP_Text GetText()
        {
            text = GetInputValue(nameof(text), text);
            if (text == null)
            {
                GameObject gameObject = GetInputValue<GameObject>(nameof(text));
                if (gameObject != null)
                {
                    text = gameObject.GetComponent<TMP_Text>();
                }
            }

            Debug.AssertFormat(text != null, "Could not find TMP_Text component in {0} in {1}", name, graph.name);
            return text;
        }

        #endregion
    }
}
