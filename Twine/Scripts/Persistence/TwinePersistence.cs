using Celeste.Persistence;
using System;
using UnityEngine;

namespace Celeste.Twine.Persistence
{
    [AddComponentMenu("Celeste/Twine/Persistence/Twine Persistence")]
    public class TwinePersistence : PersistentSceneSingleton<TwinePersistence, TwineDTO>
    {
        #region Properties and Fields

        public static readonly string FILE_NAME = "Twine.dat";
        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(TwineDTO dto)
        {
        }

        protected override TwineDTO Serialize()
        {
            return new TwineDTO();
        }

        protected override void SetDefaultValues()
        {
        }

        #endregion
    }
}