using System;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    public enum UIPosition
    {
        Left,
        Centre,
        Right,
        Narrator
    }

    [Serializable]
    public struct UIPositionAnchor
    {
        public UIPosition uiPosition;
        public RectTransform anchor;
    }
}
