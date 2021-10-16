using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CelesteEditor.Validation
{
    public interface IValidationCondition<T> where T : Object
    {
        string DisplayName { get; }

        bool Validate(T asset, StringBuilder output);
    }
}
