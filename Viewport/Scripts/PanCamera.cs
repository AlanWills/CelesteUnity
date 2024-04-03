using Celeste.Parameters;
using Celeste.Parameters.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("DnD/Viewport/Pan Camera")]
    public class PanCamera : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Camera cameraToPan;
        [SerializeField] private FloatReference speed;
        [SerializeField] private Vector2 edgeOffset = new Vector2(0.01f, 0.01f);

        [Header("Keys")]
        [SerializeField] private KeyCodeReference leftKey= default;
        [SerializeField] private KeyCodeReference upKey = default;
        [SerializeField] private KeyCodeReference rightKey = default;
        [SerializeField] private KeyCodeReference downKey = default;

        #endregion

        #region Unity Methods
        
        //We could modify this so that the panning is applied as a decaying vector (so even with no input it'll move but die off).  To give a smoother pan - experiment and try it out.

        private void OnValidate()
        {
            if (cameraToPan == null)
            {
                cameraToPan = GetComponent<Camera>();
            }

            if (leftKey == null)
            {
                leftKey = ScriptableObject.CreateInstance<KeyCodeReference>();
                leftKey.Value = KeyCode.A;
            }

            if (upKey == null)
            {
                upKey = ScriptableObject.CreateInstance<KeyCodeReference>();
                upKey.Value = KeyCode.W;
            }

            if (rightKey == null)
            {
                rightKey = ScriptableObject.CreateInstance<KeyCodeReference>();
                rightKey.Value = KeyCode.D;
            }

            if (downKey == null)
            {
                downKey = ScriptableObject.CreateInstance<KeyCodeReference>();
                downKey.Value = KeyCode.S;
            }

            if (speed == null)
            {
                speed = ScriptableObject.CreateInstance<FloatReference>();
                speed.Value = 1;
            }
        }

        private void LateUpdate()
        {
#if !UNITY_EDITOR
            Vector3 bottomLeft = cameraToPan.ScreenToWorldPoint(new Vector3(0, 0, -cameraToPan.transform.position.z));
            Vector3 topRight = cameraToPan.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -cameraToPan.transform.position.z));
            float width = topRight.x - bottomLeft.x;
            float height = topRight.y - bottomLeft.y;

            Vector2 viewportCoords = cameraToPan.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
            Vector2 deltaPan = new Vector2();
            float deltaS = speed.Value * Time.deltaTime;

            if (UnityEngine.Input.GetKey(leftKey.Value) || (0 <= viewportCoords.x && viewportCoords.x <= edgeOffset.x))
            {
                deltaPan.x -= deltaS * width;
            }

            if (UnityEngine.Input.GetKey(upKey.Value) || ((1 - edgeOffset.y) <= viewportCoords.y) && viewportCoords.y <= 1)
            {
                deltaPan.y += deltaS * height;
            }

            if (UnityEngine.Input.GetKey(rightKey.Value) || ((1 - edgeOffset.x) <= viewportCoords.x) && (viewportCoords.y <= 1))
            {
                deltaPan.x += deltaS * width;
            }

            if (UnityEngine.Input.GetKey(downKey.Value) || (0 <= viewportCoords.y && viewportCoords.y <= edgeOffset.y))
            {
                deltaPan.y -= deltaS * height;
            }

            cameraToPan.transform.Translate(deltaPan);
#endif
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            float minusCameraZ = -cameraToPan.transform.position.z;
            Vector3 bottomLeft = cameraToPan.ViewportToWorldPoint(new Vector3(edgeOffset.x, edgeOffset.y, minusCameraZ));
            Vector3 topLeft = cameraToPan.ViewportToWorldPoint(new Vector3(edgeOffset.x, 1 - edgeOffset.y, minusCameraZ));
            Vector3 topRight = cameraToPan.ViewportToWorldPoint(new Vector3(1 - edgeOffset.x, 1 - edgeOffset.y, minusCameraZ));
            Vector3 bottomRight = cameraToPan.ViewportToWorldPoint(new Vector3(1 - edgeOffset.x, edgeOffset.y, minusCameraZ));

            // Bottom Left to Top Left
            UnityEditor.Handles.DrawDottedLine(bottomLeft, topLeft, 1 - edgeOffset.y * 2);

            // Top Left to Top Right
            UnityEditor.Handles.DrawDottedLine(topLeft, topRight, 1 - edgeOffset.y * 2);

            // Top Right to Bottom Right
            UnityEditor.Handles.DrawDottedLine(topRight, bottomRight, 1 - edgeOffset.y * 2);

            // Bottom Right to Bottom Left
            UnityEditor.Handles.DrawDottedLine(bottomRight, bottomLeft, 1 - edgeOffset.y * 2);
        }
#endif

#endregion
    }
}
