using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Loading
{
    public abstract class LoadJob : ScriptableObject
    {
        public abstract IEnumerator Execute(Action<float> setProgress, Action<string> setOutput);
    }
}