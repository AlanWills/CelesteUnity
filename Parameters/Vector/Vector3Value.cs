using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "Vector3Value", menuName = "Celeste/Parameters/Vector/Vector3 Value")]
    public class Vector3Value : ParameterValue<Vector3>
    {
        #region Operators

        public static bool operator ==(Vector3Value value, Vector3 v)
        {
            return value.Value == v;
        }

        public static Vector3Value operator +(Vector3Value value, Vector3 v)
        {
            value.Value += v;
            return value;
        }

        public static Vector3Value operator -(Vector3Value value, Vector3 v)
        {
            value.Value -= v;
            return value;
        }

        public static bool operator !=(Vector3Value value, Vector3 v)
        {
            return value.Value != v;
        }

        #endregion
    }
}
