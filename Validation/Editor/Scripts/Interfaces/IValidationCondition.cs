using System.Text;
using UnityEngine;

namespace CelesteEditor.Validation
{
    public interface IValidationCondition
    {
        string DisplayName { get; }

        bool Validate(StringBuilder output);
    }

    public interface IValidationCondition<T> where T : Object
    {
        string DisplayName { get; }

        bool Validate(T asset, StringBuilder output);
    }
}
