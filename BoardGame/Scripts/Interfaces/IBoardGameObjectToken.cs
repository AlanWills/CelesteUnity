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

        void AddIsFaceUpChangedCallback(Instance instance, UnityAction<ValueChangedArgs<bool>> callback);
        void RemoveIsFaceUpChangedCallback(Instance instance, UnityAction<ValueChangedArgs<bool>> callback);
    }
}
