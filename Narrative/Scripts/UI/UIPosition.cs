using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
