using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.FX
{
    [AddComponentMenu("Celeste/FX/Graphic Colour Lerper")]
    public class GraphicColourLerper : MonoBehaviour
    {
        #region Properties and Fields

        public Graphic graphic;
        public Color startingColour = Color.black;
        public Color endingColour = Color.white;
        public float lerpTime = 1;

        private float currentTime = 0;
        private bool lerpingUp = true;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (graphic == null)
            {
                graphic = GetComponent<Graphic>();
            }

            if (graphic != null)
            {
                graphic.color = startingColour;
            }
        }

        private void OnEnable()
        {
            currentTime = 0;
            lerpingUp = true;
            graphic.color = startingColour;
        }

        private void Update()
        {
            graphic.color = Color.Lerp(lerpingUp ? startingColour : endingColour, lerpingUp ? endingColour : startingColour, currentTime);
            currentTime += Time.deltaTime;

            if (currentTime > lerpTime)
            {
                lerpingUp = !lerpingUp;
                currentTime = 0;
            }
        }
        
        #endregion
    }
}
