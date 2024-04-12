using Celeste.Achievements.Objects;
using Celeste.Logic;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.Achievements.Logic
{
    [CreateAssetMenu(
        fileName = nameof(AchievementsStateCondition), 
        menuName = CelesteMenuItemConstants.ACHIEVEMENTS_MENU_ITEM + "Logic/Achievements State Condition", 
        order = CelesteMenuItemConstants.ACHIEVEMENTS_MENU_ITEM_PRIORITY)]
    public class AchievementsStateCondition : Condition
    {
        #region Properties and Fields

        [SerializeField] private AchievementRecord achievementRecord;
        
        [SerializeField] private bool needsAchievedAchievements = true;
        [SerializeField] private ConditionOperator targetAchievedAchievementsOperator = ConditionOperator.GreaterThan;
        [SerializeField, ShowIf(nameof(needsAchievedAchievements))] private int targetAchievedAchievements;
        
        #endregion

        protected override void DoInitialize()
        {
            achievementRecord.AddOnChangedCallback(OnAchievementRecordChanged);
        }

        protected override void DoShutdown()
        {
            achievementRecord.RemoveOnChangedCallback(OnAchievementRecordChanged);
        }
        
        protected override bool DoCheck()
        {
            var achievementProgress = achievementRecord.GetProgress();
            
            if (needsAchievedAchievements)
            {
                return achievementProgress.achievedAchievements.SatisfiesComparison(targetAchievedAchievementsOperator, targetAchievedAchievements);
            }

            // True by default, although we shouldn't get here
            return true;
        }

        protected override void DoSetVariable(object arg)
        {
            if (needsAchievedAchievements)
            {
                targetAchievedAchievements = (int)arg;
            }
        }

        public override void CopyFrom(Condition original)
        {
            AchievementsStateCondition achievementsStateCondition = original as AchievementsStateCondition;
            achievementRecord = achievementsStateCondition.achievementRecord;
            needsAchievedAchievements = achievementsStateCondition.needsAchievedAchievements;
            targetAchievedAchievementsOperator = achievementsStateCondition.targetAchievedAchievementsOperator;
            targetAchievedAchievements = achievementsStateCondition.targetAchievedAchievements;
        }
        
        #region Callbacks

        private void OnAchievementRecordChanged()
        {
            Check();
        }
        
        #endregion
    }
}