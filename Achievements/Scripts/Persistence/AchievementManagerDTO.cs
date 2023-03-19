using System;
using System.Collections.Generic;
using Celeste.Achievements.Objects;

namespace Celeste.Achievements.Persistence
{
    [Serializable]
    public class AchievementDTO
    {
        public int guid;
        public int state;
    }
    
    [Serializable]
    public class AchievementManagerDTO
    {
        public List<AchievementDTO> achievements = new();
        
        public AchievementManagerDTO(AchievementRecord achievementRecord)
        {
            for (int i = 0, n = achievementRecord.NumAchievements; i < n; ++i)
            {
                Achievement achievement = achievementRecord.GetAchievement(i);
                achievements.Add(new AchievementDTO()
                {
                    guid = achievement.Guid,
                    state = (int)achievement.State
                });
            }
        }
    }
}