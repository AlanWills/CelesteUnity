using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Tools
{
    public struct GUIChangedScope : IDisposable
    {
        public bool Changed => GUI.changed;

        public void Dispose()
        {
        }
    }
}