using UnityEngine;

namespace Celeste.DeckBuilding.Interfaces
{
    public interface ICardUIController
    {
        Transform transform { get; }
        GameObject gameObject { get; }

        void Hookup(CardInstance card);
        bool IsForCard(CardInstance card);
    }
}
