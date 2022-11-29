using Celeste.Components;
using Celeste.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.BoardGame
{
    public interface IBoardGameObjectToken
    {
        Sprite GetSprite(Instance instance);

        bool IsFaceUp(Instance instance);
        void SetFaceUp(Instance instance, bool isFaceUp);
        void Flip(Instance instance);

        void AddIsFaceUpChangedListener(Instance instance, UnityAction<ValueChangedArgs<bool>> listener);
        void RemoveIsFaceUpChangedListener(Instance instance, UnityAction<ValueChangedArgs<bool>> listener);
    }
}
