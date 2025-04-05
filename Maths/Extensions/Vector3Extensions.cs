using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Maths
{
    public static class Vector3Extensions
    {
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
        
        public static Vector2Int ToVector2Int(this Vector3Int vector3Int)
        {
            return new Vector2Int(vector3Int.x, vector3Int.y);
        }
    }
}
