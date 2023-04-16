using System.Text;
using UnityEngine;

namespace CelesteEditor.Validation
{
    public interface IFixableCondition
    {
        bool CanFix();
        void Fix(StringBuilder output);
    }

    public interface IFixableCondition<T> where T : Object
    {
        bool CanFix(T asset);
        void Fix(T asset, StringBuilder output);
    }
}
