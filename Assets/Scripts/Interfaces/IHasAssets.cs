using System.Collections;
using UnityEngine;

namespace Celeste.Assets
{
    public interface IHasAssets
    {
        bool ShouldLoadAssets();
        IEnumerator LoadAssets();
    }
}