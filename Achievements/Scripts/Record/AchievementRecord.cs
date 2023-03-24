using System;
using System.Collections.Generic;
using Celeste.DataStructures;
using UnityEngine;

namespace Celeste.Achievements.Objects
{
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

        #region Callbacks

        private void OnAchievementStateChanged(Achievement achievement)
        {
            onChanged.Invoke();
        }
        
        #endregion
    }
}