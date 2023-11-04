using Celeste.Events;
using Celeste.Parameters;
using Celeste.Tools;
using System.Collections;
using UnityEngine;

namespace Celeste.FX
{
    [AddComponentMenu("Celeste/FX/Rotate Object")]
    public class RotateObject : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Transform transformToRotate;
        [SerializeField] private float animateSpeed = 1f;

        #endregion

        private void OnEnable()
        {
            transformToRotate.rotation = Quaternion.identity;
        }

        private void Update()
        {
            transformToRotate.Rotate(new Vector3(0, 0, animateSpeed * Time.deltaTime));
        }
    }
}
