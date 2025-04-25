using Celeste.Maths;
using UnityEngine;

namespace Celeste.Physics
{
    public static class Rigidbody2DExtensions
    {
        public static void SetLocalVelocity(this Rigidbody2D rigidbody2D, Vector2 vector2)
        {
            #if UNITY_6000_0_OR_NEWER
            rigidbody2D.linearVelocity = rigidbody2D.transform.up * vector2.y + rigidbody2D.transform.right * vector2.x;
            #else
            rigidbody2D.velocity = rigidbody2D.transform.up * vector2.y + rigidbody2D.transform.right * vector2.x;
            #endif
        }

        public static void SetLocalForwardVelocity(this Rigidbody2D rigidbody2D, float velocity)
        {
            #if UNITY_6000_0_OR_NEWER
            rigidbody2D.linearVelocity = rigidbody2D.transform.up.ToVector2() * velocity;
            #else
            rigidbody2D.velocity = rigidbody2D.transform.up.ToVector2() * velocity;
            #endif
        }
    }
}
