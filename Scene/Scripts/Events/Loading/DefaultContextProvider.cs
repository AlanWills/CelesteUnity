using Celeste.Scene.Events;
using System.Collections;
using UnityEngine;

namespace Celeste.Scene
{
    [CreateAssetMenu(fileName = nameof(DefaultContextProvider), menuName = "Celeste/Scene/Default Context Provider")]
    public class DefaultContextProvider : ContextProvider
    {
        public override Context Create()
        {
            return new Context();
        }
    }
}