using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FX
{
    [AddComponentMenu("Celeste/FX/Fade To Die")]
    public class FadeToDie : MonoBehaviour
    {
        #region Properties and Fields

        public float lifetime = 1;
        public SpriteRenderer icon;

        private float currentLifetime = 0;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (currentLifetime >= lifetime)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            currentLifetime += Time.deltaTime;

            icon.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), currentLifetime / lifetime);
        }

        #endregion
    }
}
