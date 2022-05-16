using System.Collections.Generic;

namespace Celeste.Parameters.Constraints
{
    public static class ConstraintUtility
    {
        public static T Constrain<T>(this IReadOnlyList<Constraint<T>> constraints, T input)
        {
            for (int i = 0, n = constraints.Count; i < n; i++)
            {
                input = constraints[i].Constrain(input);
            }

            return input;
        }
    }
}
