using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.BT
{
    public class BTBlackboard
    {
        #region Object
        
        private Dictionary<string, object> objectValues = new Dictionary<string, object>();

        public void SetObject(string internedKey, object obj)
        {
            objectValues[internedKey] = obj;
        }

        public T GetObject<T>(string internedKey, T fallbackValue = default) where T : class
        {
            return objectValues.ContainsKey(internedKey) ? objectValues[internedKey] as T : fallbackValue;
        }

        #endregion

        #region Float

        private Dictionary<string, float> floatValues = new Dictionary<string, float>();

        public void SetFloat(string internedKey, float value)
        {
            floatValues[internedKey] = value;
        }

        public float GetFloat(string internedKey, float fallbackValue = default)
        {
            return floatValues.ContainsKey(internedKey) ? floatValues[internedKey] : fallbackValue;
        }

        #endregion

        #region UInt

        private Dictionary<string, uint> uintValues = new Dictionary<string, uint>();

        public void SetUInt(string internedKey, uint value)
        {
            uintValues[internedKey] = value;
        }

        public uint GetUInt(string internedKey, uint fallbackValue = default)
        {
            return uintValues.ContainsKey(internedKey) ? uintValues[internedKey] : fallbackValue;
        }

        #endregion

        #region Int

        private Dictionary<string, int> intValues = new Dictionary<string, int>();

        public void SetInt(string internedKey, int value)
        {
            intValues[internedKey] = value;
        }

        public int GetInt(string internedKey, int fallbackValue = default)
        {
            return intValues.ContainsKey(internedKey) ? intValues[internedKey] : fallbackValue;
        }

        #endregion

        #region Vector2

        private Dictionary<string, Vector2> vector2Values = new Dictionary<string, Vector2>();

        public void SetVector2(string internedKey, Vector2 value)
        {
            vector2Values[internedKey] = value;
        }

        public Vector2 GetVector2(string internedKey, Vector2 fallbackValue = default)
        {
            return vector2Values.ContainsKey(internedKey) ? vector2Values[internedKey] : fallbackValue;
        }

        #endregion

        #region Bool

        public void SetBool(string internedKey, bool value)
        {
            uintValues[internedKey] = value ? 1U : 0U;
        }

        public bool GetBool(string internedKey, bool fallbackValue = default)
        {
            return uintValues.ContainsKey(internedKey) ? uintValues[internedKey] > 0 : fallbackValue;
        }

        #endregion
    }
}
