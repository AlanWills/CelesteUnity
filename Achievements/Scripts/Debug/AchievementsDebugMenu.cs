using Celeste.Achievements.Objects;
using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.Achievements.Debug
{
    [CreateAssetMenu(fileName = nameof(AchievementsDebugMenu), menuName = "Celeste/Achievements/Debug/Achievements Debug Menu")]
    public class AchievementsDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private AchievementRecord achievementRecord;
        
        #endregion
        
        protected override void OnDrawMenu()
        {
            for (int i = 0, n = achievementRecord.NumAchievements; i < n; ++i)
            {
                Achievement achievement = achievementRecord.GetAchievement(i);

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"({achievement.Guid}) {achievement.name}");

                    if (achievement.State == AchievementState.InProgress)
                    {
                        if (achievement.TryGetProgress(out var progress))
                        {
                            GUILayout.Label($"{progress.currentProgress}/{progress.requiredProgress}");
                        }

                        if (GUILayout.Button("Check"))
                        {
                            achievement.Check();
                        }

                        if (GUILayout.Button("Achieve"))
                        {
                            achievement.Achieve();
                        }
                    }
                    else if (achievement.State == AchievementState.Achieved)
                    {
                        if (GUILayout.Button("Collect"))
                        {
                            achievement.Achieve();
                        }
                        
                        if (GUILayout.Button("Reset"))
                        {
                            achievement.Reset();
                        }
                    }
                    else if (achievement.State == AchievementState.Collected)
                    {
                        if (GUILayout.Button("Reset"))
                        {
                            achievement.Reset();
                        }
                    }
                }
            }
        }
    }
}