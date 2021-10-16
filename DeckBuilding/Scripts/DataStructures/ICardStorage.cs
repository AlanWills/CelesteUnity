using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    public interface ICardStorage
    {
        int NumCards { get; }

        CardRuntime GetCard(int cardIndex);
    }
}