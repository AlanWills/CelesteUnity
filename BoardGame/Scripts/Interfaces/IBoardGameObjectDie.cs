using Celeste.Components;
using Celeste.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.BoardGame.Interfaces
{
    public interface IBoardGameObjectDie
    {
        int GetValue(Instance instance);
        void SetValue(Instance instance, int value);
        int Roll(Instance instance);

        void AddValueChangedListener(Instance instance, UnityAction<ValueChangedArgs<int>> listener);
        void RemoveValueChangedListener(Instance instance, UnityAction<ValueChangedArgs<int>> listener);
    }

    public interface IBoardGameObjectDie2D : IBoardGameObjectDie
    {
        Sprite GetSprite(Instance instance);
        Sprite GetSprite(Instance instance, int value);
    }
}
