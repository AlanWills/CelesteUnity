using Lua;
using UnityEngine;

namespace Celeste.Lua.Maths
{
    [LuaObject(kLuaName)]
    public partial class LuaVector2Int
    {
        #region Properties and Fields

        public const string kLuaName = nameof(Vector2Int);
        
        [LuaMember("x")]
        public int x
        {
            get => vector.x;
            set => vector = new Vector2Int(value, vector.y);
        }

        [LuaMember("y")]
        public int y
        {
            get => vector.y;
            set => vector = new Vector2Int(vector.x, value);
        }

        public Vector2Int v => vector;

        private Vector2Int vector;

        #endregion

        [LuaMember("new")]
        private static LuaVector2Int New(int x, int y)
        {
            return new LuaVector2Int
            {
                vector = new Vector2Int(x, y)
            };
        }
        
        [LuaMetamethod(LuaObjectMetamethod.Add)]
        public static LuaVector2Int Add(LuaVector2Int a, LuaVector2Int b)
        {
            return new LuaVector2Int
            {
                vector = a.vector + b.vector
            };
        }
    
        [LuaMetamethod(LuaObjectMetamethod.Sub)]
        public static LuaVector2Int Sub(LuaVector2Int a, LuaVector2Int b)
        {
            return new LuaVector2Int()
            {
                vector = a.vector - b.vector
            };
        }

        [LuaMetamethod(LuaObjectMetamethod.ToString)]
        public override string ToString()
        {
            return vector.ToString();
        }
    }
}