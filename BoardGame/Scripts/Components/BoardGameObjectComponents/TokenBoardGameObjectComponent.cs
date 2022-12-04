using Celeste.Components;
using Celeste.Events;
using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.BoardGame.Components
{
    [DisplayName("Flippable")]
    [CreateAssetMenu(fileName = nameof(TokenBoardGameObjectComponent), menuName = "Celeste/Board Game/Board Game Object Components/Token")]
    public class TokenBoardGameObjectComponent : BoardGameObjectComponent, IBoardGameObjectToken
    {
        #region Save Data

        [Serializable]
        private class SaveData : ComponentData
        {
            public bool isFaceUp;
        }

        #endregion

        #region Events

        [Serializable]
        private class Events : ComponentEvents
        {
            public BoolValueChangedUnityEvent OnIsFaceUpChanged { get; } = new BoolValueChangedUnityEvent();
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private bool startsFaceUp = true;
        [SerializeField] private Sprite faceUpSprite;
        [SerializeField] private Sprite faceDownSprite;

        #endregion

        public override ComponentData CreateData()
        {
            return new SaveData();
        }

        public override ComponentEvents CreateEvents()
        {
            return new Events();
        }

        public override void SetDefaultValues(Instance instance)
        {
            SaveData saveData = instance.data as SaveData;
            saveData.isFaceUp = startsFaceUp;
        }

        public Sprite GetSprite(Instance instance)
        {
            return IsFaceUp(instance) ? faceUpSprite : faceDownSprite;
        }

        public bool IsFaceUp(Instance instance)
        {
            return (instance.data as SaveData).isFaceUp;
        }

        public void SetFaceUp(Instance instance, bool isFaceUp)
        {
            SaveData saveData = instance.data as SaveData;
            Events events = instance.events as Events;

            if (saveData.isFaceUp != isFaceUp)
            {
                saveData.isFaceUp = isFaceUp;
                events.ComponentDataChanged.Invoke();
                events.OnIsFaceUpChanged.Invoke(new ValueChangedArgs<bool>(!isFaceUp, isFaceUp));
            }
        }

        public void Flip(Instance instance)
        {
            SetFaceUp(instance, !IsFaceUp(instance));
        }

        public void AddIsFaceUpChangedListener(Instance instance, UnityAction<ValueChangedArgs<bool>> listener)
        {
            (instance.events as Events).OnIsFaceUpChanged.AddListener(listener);
        }

        public void RemoveIsFaceUpChangedListener(Instance instance, UnityAction<ValueChangedArgs<bool>> listener)
        {
            (instance.events as Events).OnIsFaceUpChanged.RemoveListener(listener);
        }
    }
}
