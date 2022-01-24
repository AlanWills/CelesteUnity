using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Tools
{
    public class GUIEnabledScope : IDisposable
    {
        public GUIEnabledScope(bool enabled)
        {
            GUI.enabled = enabled;
        }

        public void Dispose()
        {
            GUI.enabled = true;
        }
    }
}