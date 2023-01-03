using Celeste.BoardGame.Interfaces;
using Celeste.Components;
using Celeste.DataStructures;
using Celeste.Events;
using Celeste.Objects.Types;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.BoardGame.Components
{
    [DisplayName("2D Die")]
    [CreateAssetMenu(fileName = nameof(Die2DBoardGameObjectComponent), menuName = "Celeste/Board Game/Board Game Object Components/Die 2D")]
    public class Die2DBoardGameObjectComponent : BoardGameObjectComponent, IBoardGameObjectDie2D, IBoardGameObjectTooltip
    {
        #region Save Data

        [Serializable]
        private class SaveData : ComponentData
        {
            public int currentValue;
        }

        #endregion

        #region Events

        private class Events : ComponentEvents
        {
            public IntValueChangedUnityEvent CurrentValueChanged { get; } = new IntValueChangedUnityEvent();
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int minValue = 1;
        [SerializeField] private int maxValue = 6;
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();
        [SerializeField] private bool customTooltips = false;
        [SerializeField, ShowIf(nameof(customTooltips))] private StringList tooltips;

        [Header("Events")]
        [SerializeField] private ShowTooltipEvent showTooltipEvent;
        [SerializeField] private Celeste.Events.Event hideTooltipEvent;

        #endregion

        public override ComponentData CreateData()
        {
            return new SaveData() { currentValue = minValue };
        }

        public override ComponentEvents CreateEvents()
        {
            return new Events();
        }

        public Sprite GetSprite(Instance instance)
        {
            return GetSprite(instance, GetValue(instance));
        }

        public Sprite GetSprite(Instance instance, int value)
        {
            return sprites.Get(value - minValue);
        }

        public int GetValue(Instance instance)
        {
            SaveData saveData = instance.data as SaveData;
            return saveData.currentValue;
        }

        public int Roll(Instance instance)
        {
            int value = UnityEngine.Random.Range(minValue, maxValue + 1);
            SetValue(instance, value);
            
            return value;
        }

        public void SetValue(Instance instance, int value)
        {
            SaveData saveData = instance.data as SaveData;
            Events events = instance.events as Events;
            int oldValue = saveData.currentValue;

            saveData.currentValue = value;
            events.CurrentValueChanged.Invoke(new ValueChangedArgs<int>(oldValue, value));
            events.ComponentDataChanged.Invoke();
        }

        public void AddValueChangedListener(Instance instance, UnityAction<ValueChangedArgs<int>> listener)
        {
            (instance.events as Events).CurrentValueChanged.AddListener(listener);
        }

        public void RemoveValueChangedListener(Instance instance, UnityAction<ValueChangedArgs<int>> listener)
        {
            (instance.events as Events).CurrentValueChanged.RemoveListener(listener);
        }

        public void ShowTooltip(Instance instance, Vector3 position, bool isWorldSpace)
        {
            int value = GetValue(instance);
            showTooltipEvent.Invoke(TooltipArgs.AnchoredToMouse(customTooltips ? tooltips.GetItem(value - minValue) : value.ToString()));
        }

        public void HideTooltip(Instance instance)
        {
            hideTooltipEvent.Invoke();
        }
    }
}
