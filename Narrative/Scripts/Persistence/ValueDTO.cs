using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.Persistence
{
    [Serializable]
    public struct ValueDTO
    {
        public string name;
        public int type;
        public object value;

        public ValueDTO(ValueRecord valueRecord)
        {
            name = valueRecord.Name;
            type = (int)valueRecord.Type;
            value = valueRecord.Value;
        }
    }
}