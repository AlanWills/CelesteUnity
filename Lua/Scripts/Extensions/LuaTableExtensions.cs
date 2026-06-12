using System;
using Celeste.Lua.Maths;
using Lua;
using Lua.Unity;
using UnityEngine;

namespace Celeste.Lua
{
    public static class LuaTableExtensions
    {
        public static bool TryGetEnum<T>(
            this LuaTable luaTable, 
            string key, 
            out T enumValue) where T : Enum
        {
            enumValue = default;
            
            if (!luaTable.TryGetValue(key, out LuaValue luaValue))
            {
                return false;
            }

            if (!luaValue.IsString())
            {
                return false;
            }

            if (!Enum.TryParse(typeof(T), luaValue.ReadString(), out object e))
            {
                return false;
            }

            enumValue = (T)e;
            return true;
        }
        
        public static bool TryGetVector2Int(
            this LuaTable luaTable, 
            string key, 
            out Vector2Int v)
        {
            v = default;
            
            if (!luaTable.TryGetValue(key, out LuaValue luaValue))
            {
                return false;
            }

            if (!luaValue.IsUserData())
            {
                return false;
            }
            
            LuaVector2Int luaVector2Int = luaValue.As<LuaVector2Int>();
            v = luaVector2Int.v;
            return true;
        }
        
        public static bool TryGetFunction(
            this LuaTable luaTable, 
            string key, 
            out LuaFunction f)
        {
            f = null;
            
            if (!luaTable.TryGetValue(key, out LuaValue luaValue))
            {
                return false;
            }

            if (!luaValue.IsFunction())
            {
                return false;
            }
            
            f = luaValue.ReadFunction();
            return true;
        }

        public static bool TryGet<T>(this LuaTable luaTable, string key, out T value)
        {
            value = default;
            
            if (!luaTable.TryGetValue(key, out LuaValue luaValue))
            {
                return false;
            }

            return luaValue.TryRead(out value);
        }
    }
}