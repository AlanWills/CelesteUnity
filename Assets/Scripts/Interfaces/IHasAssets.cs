using System.Collections;
using UnityEngine;

namespace Celeste.Assets
{
    public interface IHasAssets
    {
        GameObject gameObject { get; }

        bool ShouldLoadAssets();
        IEnumerator LoadAssets();
    }
}