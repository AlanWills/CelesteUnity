using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "Vector3Reference", menuName = "Celeste/Parameters/Vector/Vector3 Reference")]
    public class Vector3Reference : ParameterReference<Vector3, Vector3Value, Vector3Reference>
    {
        #region Operators

        public static bool operator ==(Vector3Reference reference, Vector3 v)
        {
            return reference.Value == v;
        }

        public static Vector3Reference operator +(Vector3Reference reference, Vector3 v)
        {
            reference.Value += v;
            return reference;
        }

        public static Vector3Reference operator -(Vector3Reference reference, Vector3 v)
        {
            reference.Value -= v;
            return reference;
        }

        public static bool operator !=(Vector3Reference reference, Vector3 v)
        {
            return reference.Value != v;
        }

        #endregion

        #region Equals & HashCode

        public override bool Equals(object obj)
        {
            return obj is Vector3Reference reference &&
                   base.Equals(obj) &&
                   Value.Equals(reference.Value);
        }

        public override int GetHashCode()
        {
            int hashCode = -159790080;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        #endregion
    }
}