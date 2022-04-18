using System.Collections.Generic;

namespace Celeste.Narrative
{
    public interface IUsesTags
    {
        IEnumerable<string> Tags { get; }

        bool UsesTag(string tag);
    }
}
