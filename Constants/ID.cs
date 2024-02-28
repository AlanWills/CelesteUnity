using System;
using UnityEngine;

namespace Celeste.Constants
{
    [CreateAssetMenu(
        fileName = nameof(ID), 
        menuName = CelesteMenuItemConstants.CONSTANTS_MENU_ITEM + "ID",
        order = CelesteMenuItemConstants.CONSTANTS_MENU_ITEM_PRIORITY)]
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

        public static bool operator ==(ID a, int b)
        {
            return a != null && a.Equals(b);
        }

        public static bool operator==(int a, ID b)
        {
            return b == a;
        }

        public static bool operator !=(ID a, int b)
        {
            return !(a == b);
        }

        public static bool operator !=(int a, ID b)
        {
            return !(b == a);
        }
    }
}