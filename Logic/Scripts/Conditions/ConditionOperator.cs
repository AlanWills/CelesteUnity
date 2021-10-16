using System;

namespace Celeste.Logic
{
    public enum ConditionOperator
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo
    }

    public static class Compare
    {
        public static bool SatisfiesComparison<T>(this T x, ConditionOperator op, T y) where T : IComparable<T>
        {
            var comparison = x.CompareTo(y);

            switch (op)
            {
                case ConditionOperator.GreaterThanOrEqualTo:
                    return comparison >= 0;
                case ConditionOperator.LessThanOrEqualTo:
                    return comparison <= 0;
                case ConditionOperator.LessThan:
                    return comparison < 0;
                case ConditionOperator.GreaterThan:
                    return comparison > 0;
                case ConditionOperator.NotEquals:
                    return comparison != 0;
                case ConditionOperator.Equals:
                    return comparison == 0;
            }
            return false;
        }
    }

    public static class Equate
    {
        public static bool SatisfiesEquality<T>(this T x, ConditionOperator op, T y) where T : IEquatable<T>
        {
            bool equals = x.Equals(y);

            switch (op)
            {
                case ConditionOperator.NotEquals:
                    return !equals;
                case ConditionOperator.Equals:
                    return equals;
                default:
                    UnityEngine.Debug.LogAssertion($"{op} is not supported for type {typeof(T).Name}.");
                    return false;
            }
        }
    }
}
