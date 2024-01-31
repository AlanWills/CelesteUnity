using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Loading
{
    public abstract class LoadJob : ScriptableObject
    {
        public bool ShowOutputInLoadingScreen => showOutputInLoadingScreen;

        [SerializeField] protected bool showOutputInLoadingScreen = true;

        public abstract IEnumerator Execute(Action<float> setProgress, Action<string> setOutput);
    }
}