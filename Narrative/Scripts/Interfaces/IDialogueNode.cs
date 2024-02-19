﻿using Celeste.Narrative.UI;
using UnityEngine;

namespace Celeste.Narrative
{
    public interface IDialogueNode
    {
        string name { get; set; }
        Vector2 Position { get; set; }

        string Dialogue { get; }
        string RawDialogue { get; set; }
        DialogueType DialogueType { get; set; }
        UIPosition UIPosition { get; set; }
        Object[] DialogueTokens { set; }
    }
}
