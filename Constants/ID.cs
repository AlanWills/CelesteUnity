using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Constants
{
    [CreateAssetMenu(fileName = nameof(ID), menuName = "Celeste/Constants/ID")]
    public class ID : ScriptableObject, IEquatable<ID>
    {
        public bool Equals(ID other)
        {
            return this == other;
        }
    }
}