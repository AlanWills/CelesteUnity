using Celeste.Objects;
using System;
using UnityEngine;

namespace Celeste.UI
{
    [CreateAssetMenu(fileName = nameof(CanvasLayers), menuName = "Celeste/UI/Canvas Layers")]
    public class CanvasLayers : ListScriptableObject<CanvasLayer>
    {
        public void Synchronize()
        {
            for (int i = 0; i < NumItems; ++i)
            {
                CanvasLayer canvasLayer = GetItem(i);
                if (canvasLayer != null)
                {
                    canvasLayer.SortOrder = i;
                }
            }
        }
    }
}