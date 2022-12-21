using UnityEngine;

namespace Celeste.Tools
{
    [AddComponentMenu("Celeste/Tools/Gizmos/Rect Gizmo")]
    public class RectGizmo : MonoBehaviour
    {
        [SerializeField] private Vector2 size;

        private void OnDrawGizmos()
        {
            Vector3 position = transform.position;
            Vector3 topRight = position + new Vector3(size.x * 0.5f, size.y * 0.5f, 0);
            Vector3 bottomRight = position + new Vector3(size.x * 0.5f, -size.y * 0.5f, 0);
            Vector3 bottomLeft = position + new Vector3(-size.x * 0.5f, -size.y * 0.5f, 0);
            Vector3 topLeft = position + new Vector3(-size.x * 0.5f, size.y * 0.5f, 0);

            Color originalGizmoColour = Gizmos.color;
            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);

            Gizmos.color = originalGizmoColour;
        }
    }
}
