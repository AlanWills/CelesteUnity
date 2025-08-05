using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative
{
    public enum DialogueType
    {
        Speech = 1,
        Thinking = 2,
        Action = 3
    }

    [Serializable]
    public struct DialogueTypeStyle
    {
        public DialogueType dialogueType;
        public string format;
    }
}