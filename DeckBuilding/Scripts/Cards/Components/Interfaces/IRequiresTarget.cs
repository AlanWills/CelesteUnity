using Celeste.Components;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    public interface IRequiresTarget
    {
        bool RequiresTarget(Instance instance);
    }
}