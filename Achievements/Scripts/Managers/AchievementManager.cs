using System.Linq;
using Celeste.Achievements.Catalogue;
using Celeste.Achievements.Objects;
using UnityEngine;
using Celeste.Persistence;
using Celeste.Achievements.Persistence;

namespace Celeste.Achievements.Managers
{
    [AddComponentMenu("Celeste/Achievements/Achievement Manager")]
    public class AchievementManager : PersistentSceneManager<AchievementManager, AchievementManagerDTO>
    {
        #region Properties and Fields
        
        public const string FILE_NAME = "Achievements.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private AchievementRecord achievementRecord;
        [SerializeField] private AchievementCatalogue achievementCatalogue;
        
        #endregion
        
        #region Unity Methods

        protected override void OnDestroy()
        {
            achievementRecord.Shutdown();
            
            base.OnDestroy();
        }

        #endregion
        
        #region Save/Load

        protected override AchievementManagerDTO Serialize()
        {
            return new AchievementManagerDTO(achievementRecord);
        }

        protected override void Deserialize(AchievementManagerDTO dto)
        {
            var lookup = dto.achievements.ToDictionary(x => x.guid);
            
            for (int i = 0, n = achievementCatalogue.NumItems; i < n; ++i)
            {
                Achievement achievement = achievementCatalogue.GetItem(i);

                if (lookup.TryGetValue(achievement.Guid, out AchievementDTO achievementDTO))
                {
                    achievement.State = (AchievementState)achievementDTO.state;
                }
            }
            
            achievementRecord.Initialize(achievementCatalogue);
        }

        protected override void SetDefaultValues()
        {
            achievementRecord.Initialize(achievementCatalogue);
        }
        
        #endregion
    }
}