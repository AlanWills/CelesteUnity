using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Set Camera Value")]
    [RequireComponent(typeof(Camera))]
    public class SetCameraValue : MonoBehaviour
    {
        #region Properties and Fields

        public CameraValue cameraValue = default;

        #endregion

        private void Awake()
        {
            cameraValue.Value = GetComponent<Camera>();
        }
    }
}
