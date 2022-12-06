using System;
using UnityEngine;

namespace Celeste.Constants
{
    [CreateAssetMenu(fileName = nameof(ID), menuName = "Celeste/Constants/ID")]
    public class ID : ScriptableObject, IEquatable<ID>, IEquatable<int>
    {
        public bool Equals(ID other)
        {
            return this == other;
        }

        public bool Equals(int other)
        {
            return GetHashCode() == other;
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return name;
        }
    }
}