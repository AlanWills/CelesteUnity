using Celeste.Narrative.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Narrative
{
    public interface IDialogueNode
    {
        string Dialogue { get; }
        string RawDialogue { get; }
        DialogueType DialogueType { get; }
        UIPosition UIPosition { get; }
    }
}
