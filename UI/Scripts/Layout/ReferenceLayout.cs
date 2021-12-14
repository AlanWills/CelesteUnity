using System;
using System.Collections;
using UnityEngine;

namespace Celeste.UI.Layout
{
    [CreateAssetMenu(fileName = nameof(ReferenceLayout), menuName = "Celeste/UI/Layout/Reference Layout Rect")]
    public class ReferenceLayout : ScriptableObject
    {
        [NonSerialized] public RectTransform rectTransform;
    }
}