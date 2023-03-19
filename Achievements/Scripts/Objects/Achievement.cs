using System;
using System.Collections.Generic;
using Celeste.Achievements.Events;
using Celeste.Events;
using Celeste.Localisation;
using Celeste.Logic;
using Celeste.Objects;
using Celeste.Rewards.Catalogue;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Achievements.Objects
{
    public enum AchievementState
    {
        InProgress,
        Achieved,
        Collected
    }
    
    [CreateAssetMenu(fileName = nameof(Achievement), menuName = "Celeste/Achievements/Achievement")]
    public class Achievement : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set => guid = value;
        }
        
        public LocalisationKey Title => titleLocalisationKey;
        public IReadOnlyList<Reward> Rewards => rewards;

        public AchievementState State
        {
            get
            {
                if (!achievedCondition.IsMet)
                {
                    return AchievementState.InProgress;
                }

                if (!collected)
                {
                    return AchievementState.Achieved;
                }

                return AchievementState.Collected;
            }
            set
            {
                AchievementState oldState = State;
                achievedCondition.IsMet = value != AchievementState.InProgress;
                collected = value == AchievementState.Collected;

                if (oldState != State)
                {
                    onStateChanged.Invoke(this);
                }
            }
        }

        [SerializeField] private int guid;
        [SerializeField] private LocalisationKey titleLocalisationKey;
        [SerializeField] private Condition achievedCondition;
        [SerializeField] private List<Reward> rewards = new();

        [Header("Events")] 
        [SerializeField] private GuaranteedAchievementEvent onStateChanged = new();

        [NonSerialized] private bool collected;
        
        #endregion

        public void Init()
        {
            achievedCondition.AddOnIsMetConditionChanged(OnAchievedConditionChanged);
        }

        public void Shutdown()
        {
            achievedCondition.RemoveOnIsMetConditionChanged(OnAchievedConditionChanged);
            onStateChanged.RemoveAllListeners();
        }
        
        #region Callbacks

        public void AddOnStateChangedCallback(UnityAction<Achievement> callback)
        {
            onStateChanged.AddListener(callback);
        }

        public void RemoveOnStateChangedCallback(UnityAction<Achievement> callback)
        {
            onStateChanged.RemoveListener(callback);
        }

        private void OnAchievedConditionChanged(ValueChangedArgs<bool> args)
        {
            onStateChanged.Invoke(this);
        }
        
        #endregion
    }
}