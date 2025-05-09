﻿using Celeste.Events;
using Celeste.Input.Settings;
using Celeste.Parameters;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Set Camera Value")]
    [RequireComponent(typeof(Camera))]
    public class SetCameraValue : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Camera _camera;
        [SerializeField] private bool useParameter = true;
        [ShowIf(nameof(useParameter))] public CameraValue cameraValue;
        [HideIf(nameof(useParameter))] public CameraEvent setCameraEvent;
        [SerializeField] private bool setInAwake = true;
        [SerializeField] private bool setInOnEnable = false;
        [SerializeField] private bool setInStart = false;
        [SerializeField] private bool setInUpdate = false;

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.TryGet(ref _camera);

            if (cameraValue == null && useParameter)
            {
                cameraValue = InputEditorSettings.GetOrCreateSettings().InputCamera;
            }
            else if (cameraValue != null && !useParameter)
            {
                cameraValue = null;
            }

            if (setCameraEvent == null && !useParameter)
            {
                setCameraEvent = InputEditorSettings.GetOrCreateSettings().SetInputCamera;
            }
            else if (setCameraEvent != null && useParameter)
            {
                setCameraEvent = null;
            }
        }
#endif

        private void Awake()
        {
            if (setInAwake)
            {
                SetCamera();
            }
        }

        private void OnEnable()
        {
            if (setInOnEnable)
            {
                SetCamera();
            }
        }

        private void Start()
        {
            if (setInStart)
            {
                SetCamera();
            }
        }

        private void Update()
        {
            if (setInUpdate)
            {
                SetCamera();
            }
        }

        #endregion

        private void SetCamera()
        {
            if (useParameter)
            {
                cameraValue.Value = _camera;
            }
            else
            {
                setCameraEvent.Invoke(_camera);
            }
        }
    }
}
