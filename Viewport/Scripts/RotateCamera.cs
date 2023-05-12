using Celeste.Events;
using Celeste.Parameters;
using Celeste.Parameters.Input;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Rotate Camera")]
    public class RotateCamera : MonoBehaviour
    {
        #region Properties and Fields

        public Camera cameraToRotate;
        public KeyCodeReference rotateAntiClockwiseKey;
        public KeyCodeReference rotateClockwiseKey;
        public FloatReference rotateSpeed;
        public Vector3 axis = Vector3.forward;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (cameraToRotate == null)
            {
                cameraToRotate = GetComponent<Camera>();
            }

            if (rotateAntiClockwiseKey == null)
            {
                rotateAntiClockwiseKey = ScriptableObject.CreateInstance<KeyCodeReference>();
                rotateAntiClockwiseKey.IsConstant = true;
                rotateAntiClockwiseKey.Value = KeyCode.Q;
            }

            if (rotateClockwiseKey == null)
            {
                rotateClockwiseKey = ScriptableObject.CreateInstance<KeyCodeReference>();
                rotateClockwiseKey.IsConstant = true;
                rotateClockwiseKey.Value = KeyCode.E;
            }

            if (rotateSpeed == null)
            {
                rotateSpeed = ScriptableObject.CreateInstance<FloatReference>();
                rotateSpeed.IsConstant = true;
                rotateSpeed.Value = 1;
            }
        }

        private void LateUpdate()
        {
            float deltaRotation = 0;

            if (Input.GetKey(rotateAntiClockwiseKey.Value))
            {
                deltaRotation += Time.deltaTime * rotateSpeed.Value;
            }

            if (Input.GetKey(rotateClockwiseKey.Value))
            {
                deltaRotation -= Time.deltaTime * rotateSpeed.Value;
            }

            if (deltaRotation != 0)
            {
                transform.Rotate(axis, deltaRotation);
            }
        }

        #endregion
    }
}
