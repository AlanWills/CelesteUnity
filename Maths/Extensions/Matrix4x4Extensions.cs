using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Maths
{
    public static class Matrix4x4Extensions
    {
        // Unity is column major

        public static Vector3 Right(this Matrix4x4 matrix)
        {
            Vector4 column = matrix.GetColumn(0);  
            return new Vector3(column.x, column.y, column.z);
        }

        public static Vector3 Up(this Matrix4x4 matrix)
        {
            Vector4 column = matrix.GetColumn(1);
            return new Vector3(column.x, column.y, column.z);
        }
    }
}
