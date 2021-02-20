using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "Vector3IntValue", menuName = "Celeste/Parameters/Vector/Vector3Int Value")]
    public class Vector3IntValue : ParameterValue<Vector3Int>
    {
        #region Operators

        public static bool operator ==(Vector3IntValue value, Vector3Int v)
        {
            return value.Value == v;
        }

        public static Vector3IntValue operator +(Vector3IntValue value, Vector3Int v)
        {
            value.Value += v;
            return value;
        }

        public static Vector3IntValue operator -(Vector3IntValue value, Vector3Int v)
        {
            value.Value -= v;
            return value;
        }

        public static bool operator !=(Vector3IntValue value, Vector3Int v)
        {
            return value.Value != v;
        }

        #endregion
    }
}
