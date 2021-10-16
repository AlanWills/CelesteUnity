using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "Vector3IntReference", menuName = "Celeste/Parameters/Vector/Vector3Int Reference")]
    public class Vector3IntReference : ParameterReference<Vector3Int, Vector3IntValue, Vector3IntReference>
    {
        #region Operators

        public static bool operator ==(Vector3IntReference reference, Vector3Int v)
        {
            return reference.Value == v;
        }

        public static Vector3IntReference operator +(Vector3IntReference reference, Vector3Int v)
        {
            reference.Value += v;
            return reference;
        }

        public static Vector3IntReference operator -(Vector3IntReference reference, Vector3Int v)
        {
            reference.Value -= v;
            return reference;
        }

        public static bool operator !=(Vector3IntReference reference, Vector3Int v)
        {
            return reference.Value != v;
        }

        #endregion

        #region

        public override bool Equals(object obj)
        {
            return obj is Vector3IntReference reference &&
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