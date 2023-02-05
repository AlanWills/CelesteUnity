using Celeste.CloudSave.Persistence;
using Celeste.Log;
using Celeste.Persistence;
using Celeste.Persistence.Settings;
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

        [SerializeField] private SnapshotRecord snapshotRecord;
        [SerializeField] private CloudSaveRecord cloudSaveRecord;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (snapshotRecord == null)
            {
                snapshotRecord = PersistenceEditorSettings.GetOrCreateSettings().snapshotRecord;
            }
        }

        #endregion

        #region Save/Load

        protected override CloudSaveDTO Serialize()
        {
            return new CloudSaveDTO()
            {
                playtimeFirstStart = cloudSaveRecord.PlaytimeStart
            };
        }

        protected override void Deserialize(CloudSaveDTO dto)
        {
            cloudSaveRecord.PlaytimeStart = dto.playtimeFirstStart;
        }

        protected override void SetDefaultValues()
        {
            cloudSaveRecord.PlaytimeStart = DateTimeOffset.UtcNow;

            // Save this playtime first start value as soon as it's been set
            // We'll never have to update it once we've first set it so job's a goodun
            Save();
        }

        #endregion

        #region Callbacks

        public void OnWriteCloudSave()
        {
            DataSnapshot dataSnapshot = snapshotRecord.CreateDataSnapshot();
            string saveDataString = dataSnapshot.Serialize();
            cloudSaveRecord.WriteDefaultSaveGameAsync(
                saveDataString,
                () => HudLog.LogInfo("Successfully wrote cloud save."));
        }

        #endregion
    }
}
