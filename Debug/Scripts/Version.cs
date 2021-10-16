using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Celeste.Debug.Info
{
    [AddComponentMenu("Celeste/Debug/Version")]
    public class Version : MonoBehaviour
    {
        public TextMeshProUGUI text;

        private void Start()
        {
            text.text = string.Format("v{0}", UnityEngine.Application.version);
        }

        private void OnValidate()
        {
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();
            }
        }
    }
}
