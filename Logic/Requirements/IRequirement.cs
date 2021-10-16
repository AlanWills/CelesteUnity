using Celeste.Objects;
using System;
using UnityEngine;

namespace Celeste.Logic
{
    public interface IRequirement<T>
    {
         bool Check(T args);
    }
}
