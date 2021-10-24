using Celeste.Narrative.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Narrative
{
    public interface IChoiceNode
    {
        int NumChoices { get; }

        T AddChoice<T>(string name) where T : Choice;
        Choice GetChoice(int index);
        void SelectChoice(IChoice choice);
    }
}
