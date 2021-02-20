using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    public interface IParameterContainer
    {
        T CreateParameter<T>(string name) where T : ScriptableObject;

        T CreateParameter<T>(T original) where T : ScriptableObject, ICopyable<T>;

        void RemoveAsset(ScriptableObject parameter);
    }
}
