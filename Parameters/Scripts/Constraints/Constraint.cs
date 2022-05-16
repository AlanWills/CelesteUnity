using UnityEngine;

namespace Celeste.Parameters.Constraints
{
    public abstract class Constraint<T> : ScriptableObject
    {
        public abstract T Constrain(T input);
    }
}
