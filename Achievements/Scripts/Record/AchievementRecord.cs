using System;
using System.Collections.Generic;
using Celeste.DataStructures;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Achievements.Objects
{
    [Serializable]
    public struct AchievementProgress
    {
        public int inProgressAchievements;
        public int achievedAchievements;
        public int collectedAchievements;
    }
    
    [CreateAssetMenu(fileName = nameof(AchievementRecord), menuName = "Celeste/Achievements/Achievement Record")]
    public class AchievementRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumAchievements => achievements.Count;

        [SerializeField] private Celeste.Events.Event onChanged;

        [NonSerialized] private readonly List<Achievement> achievements = new();

        #endregion

        public void Initialize(IIndexableItems<Achievement> achievementItems)
        {
            for (int i = 0, n = achievementItems.NumItems; i < n; ++i)
            {
                Achievement achievement = achievementItems.GetItem(i);
                achievement.Initialize();
                achievement.AddOnStateChangedCallback(OnAchievementStateChanged);
                achievements.Add(achievement);
            }
        }

        public void Shutdown()
        {
            foreach (var achievement in achievements)
            {
                achievement.RemoveOnStateChangedCallback(OnAchievementStateChanged);
                achievement.Shutdown();
            }
        }

        public Achievement GetAchievement(int index)
        {
            return achievements.Get(index);
        }

        public AchievementProgress GetProgress()
        {
            AchievementProgress progress = new AchievementProgress();

            for (int i = 0, n = NumAchievements; i < n; ++i)
            {
                switch (GetAchievement(i).State)
                {
                    case AchievementState.InProgress:
                        ++progress.inProgressAchievements;
                        break;
                    
                    case AchievementState.Achieved:
                        ++progress.achievedAchievements;
                        break;
                    
                    case AchievementState.Collected:
                        ++progress.collectedAchievements;
                        break;
                }
            }

            return progress;
        }

        #region Callbacks

        public void AddOnChangedCallback(UnityAction callback)
        {
            onChanged.AddListener(callback);
        }
        
        public void RemoveOnChangedCallback(UnityAction callback)
        {
           onChanged.RemoveListener(callback); 
        }

        private void OnAchievementStateChanged(Achievement achievement)
        {
            onChanged.Invoke();
        }
        
        #endregion
    }
}