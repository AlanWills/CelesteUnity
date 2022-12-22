using Celeste.BoardGame.Interfaces;
using Celeste.Components;
using Celeste.Events;
using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.BoardGame.Components
{
    [DisplayName("Token")]
    [CreateAssetMenu(fileName = nameof(TokenBoardGameObjectComponent), menuName = "Celeste/Board Game/Board Game Object Components/Token")]
    public class TokenBoardGameObjectComponent : BoardGameObjectComponent, IBoardGameObjectToken, IBoardGameObjectTooltip
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

        [Header("Sprites")]
        [SerializeField] private Sprite faceUpSprite;
        [SerializeField] private Sprite faceDownSprite;

        [Header("Tooltips")]
        [SerializeField] private string faceUpTooltip;
        [SerializeField] private string faceDownTooltip;

        [Header("Events")]
        [SerializeField] private ShowTooltipEvent showTooltipEvent;
        [SerializeField] private Celeste.Events.Event hideTooltipEvent;

        #endregion

        public override ComponentData CreateData()
        {
            return new SaveData();
        }

        public override ComponentEvents CreateEvents()
        {
            return new Events();
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

        public void AddIsFaceUpChangedCallback(Instance instance, UnityAction<ValueChangedArgs<bool>> listener)
        {
            (instance.events as Events).OnIsFaceUpChanged.AddListener(listener);
        }

        public void RemoveIsFaceUpChangedCallback(Instance instance, UnityAction<ValueChangedArgs<bool>> listener)
        {
            (instance.events as Events).OnIsFaceUpChanged.RemoveListener(listener);
        }

        #region IBoardGameObjectTooltip

        public void ShowTooltip(Instance instance, Vector3 position, bool isWorldSpace)
        {
            showTooltipEvent.Invoke(TooltipArgs.AnchoredToMouse(IsFaceUp(instance) ? faceUpTooltip : faceDownTooltip));
        }

        public void HideTooltip(Instance instance)
        {
            hideTooltipEvent.Invoke();
        }

        #endregion
    }
}
