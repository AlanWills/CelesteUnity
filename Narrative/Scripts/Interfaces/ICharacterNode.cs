using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;

namespace Celeste.Narrative
{
    public interface ICharacterNode
    {
        UIPosition UIPosition { get; set; }
        Character Character { get; set; }
    }
}
