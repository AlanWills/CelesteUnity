using Celeste.CloudSave.Persistence;
using Celeste.Log;
using Celeste.Persistence;
using Celeste.Persistence.Snapshots;
using System;
using UnityEngine;

namespace Celeste.CloudSave
{
    [AddComponentMenu("Celeste/Cloud Save/Cloud Save Manager")]
    public class CloudSaveManager : PersistentSceneManager<CloudSaveManager, CloudSaveDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "CloudSave.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private CloudSaveRecord cloudSaveRecord;

        [NonSerialized] private bool savingCloudSave = false;

        #endregion

        #region Save/Load

        protected override CloudSaveDTO Serialize()
        {
            return new CloudSaveDTO()
            {
                playtimeFirstStart = cloudSaveRecord.PlaytimeStart,
                implementation = (int)cloudSaveRecord.ActiveImplementation
            };
        }

        protected override void Deserialize(CloudSaveDTO dto)
        {
            cloudSaveRecord.Initialize(dto.playtimeFirstStart, (Implementation)dto.implementation);
        }

        protected override void SetDefaultValues()
        {
            cloudSaveRecord.Initialize(DateTimeOffset.UtcNow, Implementation.PlatformAppropriate);

            // Save this playtime first start value as soon as it's been set
            // We'll never have to update it once we've first set it so job's a goodun
            // However, if we Save() here it won't work because of our lock
            // We have to do a delayed save once we've finishing loading
            DelayedSave();
        }

        #endregion

        #region Callbacks

        public void RequestWriteCloudSave()
        {
            if (!savingCloudSave && cloudSaveRecord.IsAuthenticated)
            {
                savingCloudSave = true;
                DataSnapshot dataSnapshot = SnapshotRecord.CreateDataSnapshot();
                string saveDataString = dataSnapshot.Serialize();
                
                StartCoroutine(
                    cloudSaveRecord.WriteDefaultSaveGameAsync(
                    saveDataString,
                    () => savingCloudSave = false,
                    (error) => savingCloudSave = false));
            }
        }

        #endregion
    }
}
