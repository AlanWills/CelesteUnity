using System;
using Celeste.Parameters;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.UI
{
    public class UseCameraForCanvas : MonoBehaviour
    {
        #region Properties and Fields
        
        [SerializeField] private CameraReference cameraReference;
        [SerializeField] private Canvas canvas;
        
        #endregion
        
        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref canvas);
            
            if (cameraReference == null)
            {
                cameraReference = ScriptableObject.CreateInstance<CameraReference>();
                cameraReference.name = "CameraReference";
                cameraReference.IsConstant = false;
            }
        }

        private void Awake()
        {
            if (cameraReference.Value != null)
            {
                ApplyCameraToCanvas();
            }
        }

        private void Start()
        {
            if (cameraReference.Value != null)
            {
                ApplyCameraToCanvas();
            }
        }

        #endregion

        private void ApplyCameraToCanvas()
        {
            if (cameraReference.Value != null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = cameraReference.Value;
            }
        }
    }
}