using UnityEngine;

namespace Celeste.Maths
{
    public static class TransformExtensions
    {
        public static void LookAtWorldPosition2D(this Transform transform, Vector3 worldPosition)
        {
            worldPosition.z = transform.position.z;
            transform.up = worldPosition - transform.position;
        }
    }
}
