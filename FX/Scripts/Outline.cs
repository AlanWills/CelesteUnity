using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.FX
{
    [AddComponentMenu("Celeste/FX/Outline")]
    public class Outline : MonoBehaviour
    {
        #region Properties and Fields

        public Color Colour
        {
            get { return outlineColour; }
            set 
            { 
                outlineColour = value; 

                if (outlineMaterial != null)
                {
                    outlineMaterial.SetColor("_OutlineColour", outlineColour);
                }
            }
        }

        [SerializeField] private Renderer rendererToOutline = default;
        [SerializeField] private Graphic graphicToOutline = default;
        [SerializeField] private Material defaultMaterial = default;
        [SerializeField] private Shader outlineShader = default;
        [SerializeField] private int outlineThickness = 5;
        [SerializeField] private Color outlineColour = Color.white;

        private Material outlineMaterial;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (outlineMaterial == null)
            {
                outlineMaterial = new Material(outlineShader);
            }

            outlineMaterial.SetColor("_OutlineColour", outlineColour);
            outlineMaterial.SetInt("_OutlineThickness", outlineThickness);

            if (rendererToOutline != null)
            {
                rendererToOutline.material = outlineMaterial;
            }
            else if (graphicToOutline != null)
            {
                graphicToOutline.material = outlineMaterial;
            }
        }

        private void OnDisable()
        {
            if (rendererToOutline != null)
            {
                rendererToOutline.material = defaultMaterial;
            }
            else if (graphicToOutline != null)
            {
                graphicToOutline.material = defaultMaterial;
            }
        }

        private void OnDestroy()
        {
            if (outlineMaterial != null)
            {
                Destroy(outlineMaterial);
            }
        }

        #endregion
    }
}
