using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Set Camera Value")]
    [RequireComponent(typeof(Camera))]
    public class SetCameraValue : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private CameraValue cameraValue = default;

        #endregion

        private void Awake()
        {
            cameraValue.Value = GetComponent<Camera>();
        }
    }
}
