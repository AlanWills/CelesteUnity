using System;
using System.Collections.Generic;
using Celeste.Achievements.Events;
using Celeste.Events;
using Celeste.Localisation;
using Celeste.Logic;
using Celeste.Logic.Interfaces;
using Celeste.Objects;
using Celeste.Rewards.Catalogue;
using Celeste.Rewards.Objects;
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
    public class Achievement : ScriptableObject, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set => guid = value;
        }
        
        public LocalisationKey Title => titleLocalisationKey;
        public Reward Reward => reward;

        public AchievementState State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    state = value;
                    onStateChanged.Invoke(this);

                    if (state == AchievementState.InProgress)
                    {
                        SetUpAchievedListeners();
                    }
                    else
                    {
                        RemoveAchievedListeners();
                    }
                }
            }
        }
        
        [SerializeField] private int guid;
        [SerializeField] private LocalisationKey titleLocalisationKey;
        [SerializeField] private Condition achievedCondition;
        [SerializeField] private Reward reward;

        [Header("Events")] 
        [SerializeField] private GuaranteedAchievementEvent onStateChanged = new();

        [NonSerialized] private AchievementState state;
        [NonSerialized] private bool listenersHookedUp;
        
        #endregion

        public void Initialize()
        {
            Check();

            if (state == AchievementState.InProgress)
            {
                SetUpAchievedListeners();
            }
        }

        public void Shutdown()
        {
            RemoveAchievedListeners();
            onStateChanged.RemoveAllListeners();
        }

        private void SetUpAchievedListeners()
        {
            if (!listenersHookedUp)
            {
                achievedCondition.AddOnIsMetConditionChanged(OnAchievedConditionChanged);

                if (achievedCondition is IProgressCondition progressCondition)
                {
                    progressCondition.AddOnProgressChangedCallback(OnAchievedConditionProgressChanged);
                }

                listenersHookedUp = true;
            }
        }

        private void RemoveAchievedListeners()
        {
            if (listenersHookedUp)
            {
                if (achievedCondition is IProgressCondition progressCondition)
                {
                    progressCondition.RemoveOnProgressChangedCallback(OnAchievedConditionProgressChanged);
                }

                achievedCondition.RemoveOnIsMetConditionChanged(OnAchievedConditionChanged);

                listenersHookedUp = false;
            }
        }

        public void Check()
        {
            if (state == AchievementState.InProgress && achievedCondition.IsMet)
            {
                State = AchievementState.Achieved;
            }
        }

        public void Achieve()
        {
            if (State == AchievementState.InProgress)
            {
                State = AchievementState.Achieved;
            }
        }
        
        public void Collect()
        {
            if (State == AchievementState.Achieved)
            {
                reward.AwardReward(1);
                State = AchievementState.Collected;
            }
        }

        public void Reset()
        {
            State = AchievementState.InProgress;
        }

        public bool TryGetProgress(out ConditionProgress progress)
        {
            if (achievedCondition is IProgressCondition progressCondition)
            {
                progress = progressCondition.GetProgress();
                return true;
            }

            progress = new ConditionProgress();
            return false;
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
            Check();
        }

        private void OnAchievedConditionProgressChanged(IProgressCondition progressCondition)
        {
            Check();

            if (state == AchievementState.InProgress)
            {
                // Only fire this if we haven't already changed state from InProgress
                // Otherwise we'll get double firing
                onStateChanged.Invoke(this);
            }
        }
        
        #endregion
    }
}