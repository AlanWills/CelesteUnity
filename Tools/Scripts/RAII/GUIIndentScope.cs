using System;
using UnityEngine;

namespace Celeste.Tools
{
    public class GUIIndentScope : IDisposable
    {
        public GUIIndentScope()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }
}