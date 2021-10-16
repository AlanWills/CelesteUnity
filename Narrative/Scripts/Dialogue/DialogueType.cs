using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative
{
    [Flags]
    public enum DialogueType
    {
        Speech = 1,
        Thinking = 2,
        Action = 4
    }

    [Serializable]
    public struct DialogueTypeStyle
    {
        public DialogueType dialogueType;
        public string format;
    }
}